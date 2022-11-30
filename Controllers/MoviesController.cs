using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviePlatform.Entities;
using MoviePlatform.Extentions;
using MoviePlatform.Repo;
using MoviePlatform.ViewModels.Movies;
using MoviePlatform.ViewModels.Shared;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace MoviePlatform.Controllers
{
    public class MoviesController : Controller
    {

        private HttpWebRequest request;
        private HttpWebResponse response = null;





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

            Expression<Func<Movie, bool>> filter = u =>
            string.IsNullOrEmpty(model.Filter.Title) || u.Title.Contains(model.Filter.Title);

            model.Pager.PagesCount = (int)Math.Ceiling(context.Movies.Where(filter).Count() / (double)model.Pager.ItemsPerPage);


            model.Items = context.Movies
                                    .OrderBy(i => i.Id)
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

        [HttpGet]
        public IActionResult Create(CreateVM model)
        {
            MoviePlatformDbContext context = new MoviePlatformDbContext();

            var getAll = context.Actors.Select(MapToActors()).ToList();
            model.Actors = new List<Actor>();

            foreach (var item in getAll)
            {
                model.Actors.Add(item);
            }

            if (this.HttpContext.Session.GetObject<User>("loggedUser") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View(model);
        }


        [HttpPost]
        public IActionResult Create(CreateVM model, List<int> actorId)
        {
            MoviePlatformDbContext context = new MoviePlatformDbContext();
            var takenActors = new List<Actor>();
            foreach (var item in actorId)
            {
                takenActors.Add(context.Actors.FirstOrDefault(x => x.Id == item));
            }

            model.Actors = takenActors;

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Create));
            }






            string imageUrl = ImageUrl(model.imageUrl);
            Movie movie = new Movie();
            //movie info
            movie.Title = model.Title;
            movie.Description = model.Description;
            movie.Duration = model.Duration;
            movie.imageUrl = imageUrl;
            movie.Actors = model.Actors;

            context.Movies.Add(movie);
            context.SaveChanges();
            //then save the info to the manyToMany table

            if (model.Actors != null)
            {
                foreach (var item in model.Actors)
                {
                    ActorsToMovies actorsToMovies = new ActorsToMovies();
                    actorsToMovies.MovieId = movie.Id;
                    actorsToMovies.ActorId = item.Id;
                    context.ActorsToMovies.Add(actorsToMovies);
                }

                context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("How did we get here");
            }



            return RedirectToAction("Index", "Movies");
        }


        public IActionResult AddToFavourite(FavouriteVM model)
        {
            MoviePlatformDbContext context = new MoviePlatformDbContext();

            if (this.HttpContext.Session.GetObject<User>("loggedUser") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var userId = this.HttpContext.Session.GetObject<User>("loggedUser").Id;
            var favourites = new List<Favourite>();
            using (var contexts = new MoviePlatformDbContext())
            {
                var userMovies = contexts.Favourites.Where(x => x.userId == userId).ToList();
                favourites = userMovies;
            }

            var exists = favourites.FirstOrDefault(x => x.movieId == model.movieId && x.userId == userId);


            if (exists == null)
            {
                Favourite favourite = new Favourite();
                favourite.movieId = model.movieId;
                favourite.userId = userId;
                context.Favourites.Add(favourite);
                context.SaveChanges();
            }

            


            return RedirectToAction("Index", "Movies");
        }



        private int imagesCount()
        {
            MoviePlatformDbContext context = new MoviePlatformDbContext();
            int count = context.Movies.Count();
            return count;
        }

        private string ImageUrl(string url)
        {
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 1000;
                request.AllowWriteStreamBuffering = false;
                response = (HttpWebResponse)request.GetResponse();

                Stream s = response.GetResponseStream();

                //Write to disk
                string fileName = $"{(imagesCount() + 1)}.jpg";
                FileStream fs = new FileStream($"./wwwroot/images/moviesImage/{fileName}", FileMode.Create, FileAccess.ReadWrite);

                byte[] read = new byte[256];

                int count = s.Read(read, 0, read.Length);


                while (count > 0)

                {
                    fs.Write(read, 0, count);

                    count = s.Read(read, 0, read.Length);

                }
                //Close everything

                fs.Close();

                s.Close();

                response.Close();

                return fileName;
            }

            catch (System.Net.WebException)

            {
                if (response != null)

                    response.Close();
                return null;
            }
        }

        private Expression<Func<Actor, Actor>> MapToActors()
        {
            return x => new Actor()
            {
                Id = x.Id,
                Name = x.Name,
                Age = x.Age
            };
        }



    }
}
