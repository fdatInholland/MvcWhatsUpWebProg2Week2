using Microsoft.AspNetCore.Mvc;
using MvcWhatsUp.Exceptions;
using MvcWhatsUp.Models;
using MvcWhatsUp.Models.Extensions;
using MvcWhatsUp.Models.VM;
using MvcWhatsUp.Repositories.Interfaces;

namespace MvcWhatsUp.Controllers
{
    public class ChatsController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IChatsRepository _chatsRepository;

        public ChatsController(IUsersRepository usersRepository, IChatsRepository chatsRepository)
        {
            _usersRepository = usersRepository;
            _chatsRepository = chatsRepository;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public IActionResult AddMessage(int? id)
        {
            //Oh boy - its a mess - this should all go to a servicelayer!!!
            if (id is null)
            {
                return NotFound();
            }

            User? loggedInUser = HttpContext.Session.GetObject<User>("LoggedInUser");

            if (loggedInUser is null)
            {
                return RedirectToAction("Index", "Users");
            }

            //get the receiver user
            User? receiverUser = _usersRepository.GetUserByID(Convert.ToInt32(id));
            if (receiverUser is null)
            {
                return RedirectToAction("Index", "Users");
            }

            ViewData["ReceiverUser"] = receiverUser;

            Message message = new Message();
            message.SenderUserId = loggedInUser.UserID;
            message.ReceiverUserId = (int)id;

            return View(message);
        }

        [HttpPost]
        public IActionResult AddMessage(Message message)
        {
            try
            {
                message.SendAt = DateTime.UtcNow;
                _chatsRepository.AddMessage(message);

                ViewBag.ConfirmMessage = "Message has been added correctly!";

                //confirm
                TempData["ConfirmMessage"] = "Message has been added correctly!";

                return RedirectToAction("DisplayChat", new { id = message.ReceiverUserId });
            }
            catch (Exception ex)
            {
                //Insert Viewbag here for error message
                ViewBag.ErrorMessage = "Error adding message: " + ex.Message;
                return View(message);
            }
        }

        public IActionResult DisplayChat(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            string? loggedInUserId = Request.Cookies["UserId"];

            User? loggedInUser = HttpContext.Session.GetObject<User>("LoggedInUser");

            if (loggedInUserId is null)
            {
                return RedirectToAction("Index", "Users");
            }

            User? sendingUser = _usersRepository.GetUserByID(int.Parse(loggedInUserId));
            User? receiverUser = _usersRepository.GetUserByID((int)id);

            if ((sendingUser is null) || (receiverUser is null))
            {
                return RedirectToAction("Index", "Users");
            }

            ViewData["sendingUser"] = sendingUser;
            ViewData["receivingUser"] = receiverUser;

            ChatViewModel chatViewModel = new ChatViewModel(
                _chatsRepository.GetMessages(loggedInUser.UserID, receiverUser.UserID),
                sendingUser,
                receiverUser
            );

            return View(chatViewModel);
        }
    }
}
