using Microsoft.AspNetCore.Mvc;
using MoviePlatform.Entities;
using MoviePlatform.Extentions;
using MoviePlatform.Repo;
using MoviePlatform.ViewModels.Actors;
using MoviePlatform.ViewModels.Shared;
using System.Linq.Expressions;

namespace MoviePlatform.Controllers
{
    public class ActorsController : Controller
    {
        public IActionResult Index(IndexVM model)
        {

            MoviePlatformDbContext context = new MoviePlatformDbContext();



            model.Pager = model.Pager ?? new PagerVM();

            model.Pager.Page = model.Pager.Page <= 0
                                        ? 1
                                        : model.Pager.Page;

            model.Pager.ItemsPerPage = model.Pager.ItemsPerPage <= 0
                                        ? 12
                                        : model.Pager.ItemsPerPage;


            model.Filter = model.Filter ?? new FilterVM();

            Expression<Func<Actor, bool>> filter = u =>
            string.IsNullOrEmpty(model.Filter.Name) || u.Name.Contains(model.Filter.Name);

            model.Pager.PagesCount = (int)Math.Ceiling(context.Actors.Where(filter).Count() / (double)model.Pager.ItemsPerPage);


            model.Items = context.Actors
                                    .OrderBy(i => i.Id)
                                    .Where(filter)
                                    .Skip(model.Pager.ItemsPerPage * (model.Pager.Page - 1))
                                    .Take(model.Pager.ItemsPerPage)
                                    .ToList();


            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {

            if (this.HttpContext.Session.GetObject<User>("loggedUser") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();



        }
        [HttpPost]
        public IActionResult Create(CreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            MoviePlatformDbContext context = new MoviePlatformDbContext();


            Actor actor = new Actor();
            //actor info
            actor.Name = model.Name;
            actor.Age = model.Age;
           
            
           

            context.Actors.Add(actor);
            context.SaveChanges();
            return RedirectToAction("Index", "Actors");
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (this.HttpContext.Session.GetObject<User>("loggedUser") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            MoviePlatformDbContext context = new MoviePlatformDbContext();
            Actor item = context.Actors.First(p => p.Id == Id );
                                   
            if(item == null)
                return RedirectToAction("Index", "Actors");


            

            EditVM model = new EditVM();
            model.Id = item.Id;
            model.Name = item.Name;
            model.Age = item.Age;

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditVM model)
        {
            if (this.HttpContext.Session.GetObject<User>("loggedUser") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            MoviePlatformDbContext context = new MoviePlatformDbContext();
            Actor item = context.Actors.Where(p => p.Id == model.Id)
                                        .FirstOrDefault();

            item.Id=  model.Id;
            item.Name = model.Name;
            item.Age = model.Age;

            context.Actors.Update(item);
            context.SaveChanges();

            return RedirectToAction("Index", "Actors");
        }


    }
}
