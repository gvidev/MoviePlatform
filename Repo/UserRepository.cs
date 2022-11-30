using MoviePlatform.Entities;
using MoviePlatform.Repo;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MoviePlatform.Repo
{
    public class UserRepository
    {


        public List<User> GetAll(Expression<Func<User, bool>> filter = null)
        {
            MoviePlatformDbContext context = new MoviePlatformDbContext();

            IQueryable<User> query = context.Users;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            //where doesnt work properly with null values
            //Linq method
            //return items
            //    .Where(filter)
            //    .ToList();

            return query.ToList();
        }

        public User GetFirstOrDefault(Expression<Func<User, bool>> filter)
        {
         MoviePlatformDbContext context = new MoviePlatformDbContext();
            IQueryable<User> query = context.Users;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.FirstOrDefault();
        }


        public void Save(User item)
        {

            MoviePlatformDbContext context = new MoviePlatformDbContext();

            if (item.Id <= 0)
            {
                context.Users.Add(item);
            }
            else
            {
                context.Users.Update(item);
            }

            context.SaveChanges();


        }

        public void Delete(User item)
        {
            MoviePlatformDbContext context = new MoviePlatformDbContext();

            context.Users.Remove(item);

            context.SaveChanges();
        }

    }
}
