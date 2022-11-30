using MoviePlatform.Entities;
using MoviePlatform.ViewModels.Actors;
using MoviePlatform.ViewModels.Shared;

namespace MoviePlatform.ViewModels.Actors
{
    public class IndexVM
    {

        public FilterVM Filter { get; set; }

        public List<Actor> Items { get; set; }

        public PagerVM Pager { get; set; }

    }
}
