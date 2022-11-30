using MoviePlatform.Entities;
using MoviePlatform.ViewModels.Movies;
using MoviePlatform.ViewModels.Shared;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MoviePlatform.ViewModels.Users
{
    public class IndexVM
    {

        public List<User> Items { get; set; }
        public FilterVM Filter { get; set; }
        public PagerVM Pager { get; set; }



    }
}
