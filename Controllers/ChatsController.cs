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
            //Oh boy - its a mess
            if (id is null)
            {
                return NotFound();
            }

            //user has to be logged in
            string? loggedInUserId = Request.Cookies["UserID"];
            
            //TODO
            //User? loggedInUser = HttpContext.Session.GetObject<User>("LoggedInUser");

            if (loggedInUserId is null)
            {
                return RedirectToAction("Login", "Users");
            }

            //get the receiver user
            User? receiverUser = _usersRepository.GetUserByID(Convert.ToInt32(id));
            ViewData["ReceiverUser"] = receiverUser;

            Message message = new Message();
            
            //TODO
            //message.SenderUserId = loggedInUser.UserId;

            message.ReceiverUserId = (int)id;
            message.SenderUserId = Convert.ToInt32(loggedInUserId);

            return View(message);
        }

        [HttpPost]
        public IActionResult AddMessage(Message message)
        {
            //EXAMPLE use of tailored exceptions
            //if (string.IsNullOrEmpty(message.MessageText))
            //{
            //    throw new AddMessageException("Message text cannot be empty.");
            //}

            try
            {
                message.SendAt = DateTime.UtcNow;
                _chatsRepository.AddMessage(message);

                //ViewBag.ConfirmMessage = "Message has been added correctly!";

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

            //TODO
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

            //Was
            //List<Message> chatMessages = _chatsRepository.GetMessages(sendingUser.UserID, receiverUser.UserID);

            //REDONE to ChatViewModel
            ChatViewModel chatViewModel = new ChatViewModel(
                _chatsRepository.GetMessages(sendingUser.UserID, receiverUser.UserID),
                sendingUser,
                receiverUser
            );

            //TODO
            ChatViewModel chatViewModel2 = new ChatViewModel(
            _chatsRepository.GetMessages(loggedInUser.UserID, receiverUser.UserID),
                sendingUser,
                receiverUser
                );


            //Was
            //return View(chatMessages);

            //REDONE to ChatViewModel
            return View(chatViewModel);
        }
    }
}
