using Microsoft.AspNetCore.Mvc;
using MoviePlatform.Entities;
using MoviePlatform.Extentions;
using MoviePlatform.Repo;
using MoviePlatform.ViewModels.Users;
using System.Diagnostics;

namespace MoviePlatform.Controllers
{
    public class HomeController : Controller
    {

        //Home
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Brand()
        {
            return View();
        }

        //login get
        [HttpGet]
        public IActionResult Login()
        {
            if (this.HttpContext.Session.GetObject<User>("loggedUser") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        //Login post
        [HttpPost]
        public IActionResult Login(LoginVM model)
        {

            if (!this.ModelState.IsValid)
                return View(model);


            MoviePlatformDbContext context = new MoviePlatformDbContext();
            User loggedUser = context.Users.Where(u => u.Username == model.Username &&
                                                       u.Password == model.Password)
                                           .FirstOrDefault();

            if (loggedUser == null)
            {
                this.ModelState.AddModelError("authError", "*Invalid username or password!");
                return View(model);
            }

            HttpContext.Session.SetObject("loggedUser", loggedUser);

            return RedirectToAction("Index", "Home");

        }


        //Registration get
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }


        //Registration post
        //Get the elements from the form and put info into database
        [HttpPost]
        public IActionResult Registration(RegistrationVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            UserRepository repo = new UserRepository();
            User item = new User();
            item.FirstName = model.FirstName;
            item.LastName = model.LastName;
            item.Username = model.Username;
            item.Password = model.Password;
            repo.Save(item);

            

            return RedirectToAction("Login", "Home");
        }

        //Logout get
        [HttpGet]
        public IActionResult Logout()
        {
            if (this.HttpContext.Session.GetObject<User>("loggedUser") == null)
                return RedirectToAction("Index", "Home");

            this.HttpContext.Session.Remove("loggedUser");

            return RedirectToAction("Index", "Home");
        }



    }
}