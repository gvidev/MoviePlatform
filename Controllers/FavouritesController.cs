using Microsoft.AspNetCore.Mvc;
using MoviePlatform.Entities;
using MoviePlatform.Extentions;
using MoviePlatform.Repo;
using MoviePlatform.ViewModels.Favourite;
using MoviePlatform.ViewModels.Shared;
using System.Linq.Expressions;

namespace MoviePlatform.Controllers
{
    public class FavouritesController : Controller
    {
        public IActionResult Index(IndexVM model)
        {
            MoviePlatformDbContext context = new MoviePlatformDbContext();

            if (this.HttpContext.Session.GetObject<User>("loggedUser") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var currentUser = this.HttpContext.Session.GetObject<User>("loggedUser");



            model.Pager = model.Pager ?? new PagerVM();


            model.Pager.Page = model.Pager.Page <= 0
                                        ? 1
                                        : model.Pager.Page;

            model.Pager.ItemsPerPage = model.Pager.ItemsPerPage <= 0
                                        ? 12
                                        : model.Pager.ItemsPerPage;


            model.Filter = model.Filter ?? new FilterVM();

            Expression<Func<Movie, bool>> filter = u =>
            string.IsNullOrEmpty(model.Filter.Title) || u.Title.Contains(model.Filter.Title);

            model.Pager.PagesCount = (int)Math.Ceiling(context.Favourites.Select(x => x.Movie).Where(filter).Count() /
                (double)model.Pager.ItemsPerPage);



            model.Items = context.Favourites
                                .Where(x => x.userId == currentUser.Id)
                                .Select(movie => movie.Movie)
                                .Where(filter)
                                .Skip(model.Pager.ItemsPerPage * (model.Pager.Page - 1))
                                .Take(model.Pager.ItemsPerPage)
                                .ToList();



            foreach (Movie movie in model.Items)
            {
                movie.Actors = context.ActorsToMovies.Where(i => i.MovieId == movie.Id).Select(atm => atm.Actor).ToList();
            }


            return View(model);
        }

        public IActionResult DeleteFromFavourite(int id)
        {

            MoviePlatformDbContext context = new MoviePlatformDbContext();
            var user = this.HttpContext.Session.GetObject< User >("loggedUser");
            var movie = context.Favourites.FirstOrDefault(x => x.movieId == id && x.userId == user.Id);

            context.Favourites.Remove(movie);
            context.SaveChanges();  


            return RedirectToAction("Index", "Favourites", new IndexVM());

        }
    }
}
