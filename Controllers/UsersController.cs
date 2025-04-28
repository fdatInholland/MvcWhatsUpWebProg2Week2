using Microsoft.AspNetCore.Mvc;
using MvcWhatsUp.Models;
using MvcWhatsUp.Models.Extensions;
using MvcWhatsUp.Repositories.Interfaces;

namespace MvcWhatsUp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            User? user = _usersRepository.GetUserByLoginCredentials(loginModel.Username, loginModel.Password);

            if (user is null)
            {
                ViewBag.ErrorMessage = "Bad username/password combination";

                return View(loginModel);
            }
            else
            {
                HttpContext.Session.SetObject("LoggedInUser", user);
                return RedirectToAction("Index", "Users");
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            //With the extensionmethod
            User? loggedInUser = HttpContext.Session.GetObject<User>("LoggedInUser");
            ViewData["LoggedInUser"] = loggedInUser;

            List<User> users = _usersRepository.GetAllUsers();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            try
            {
                //add users via repository
                _usersRepository.AddUser(user);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //Insert Viewbag here for error message
                return View(user);
            }
        }

        //Get user
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            else
            {
                //get user via repository
                User? user = _usersRepository.GetUserByID((int)id);
                return View(user);
            }
        }

        public IActionResult Delete(User user)
        {
            try
            {
                //delete user via repository
                _usersRepository.DeleteUser(user);

                //go back to user list(via Index)
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //something went wrong, go back to view with user
                return View(user);
            }
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {

            if (id is null)
            {
                return NotFound();
            }
            else
            {
                //get user via repository
                User? user = _usersRepository.GetUserByID((int)id);
                return View(user);
            }
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            try
            {
                //add users via repository
                _usersRepository.UpdateUser(user);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(user);
            }
        }

        public IActionResult UserDetails()
        {
            //ViewData["Message"] = "In an bottle";
            //ViewData["User"] = new User(12, "Peter", "Sauber", "06 12345", "peter@gmail.com");

            //ViewBag["Meesage"] = "In an bottle ";
            //ViewBag["User"] = new User(12, "Peter", "Sauber", "06 12345", "peter@gmail.com");

            //UserVM userVM = new UserVM("Peter Sauber", "06 12345", "peter@gmail.com");
            //return View(userVM);

            TempData["ConfirmationMessage"] = "This is some confirmation data";
            return RedirectToAction("Index", "Users");
            
            //return View();
        }
    }
}
