using MoviePlatform.Entities;
using MoviePlatform.ViewModels.Shared;

namespace MoviePlatform.ViewModels.Movies
{
    public class IndexVM
    {
        public FilterVM Filter { get; set; }

        public List<Movie> Items { get; set; }
       

        public PagerVM Pager { get; set; }

       
    }
}
