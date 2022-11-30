using MoviePlatform.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MoviePlatform.ViewModels.Movies
{
    public class CreateVM
    {
        [DisplayName("Title: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Title { get; set; }

        [DisplayName("Description: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Description { get; set; }

        [DisplayName("Duration: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public int Duration { get; set; }


        [DisplayName("imageUrl: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string imageUrl { get; set; }


        [DisplayName("Actors: ")]
        public  List<Actor> Actors { get; set; }

        








    }
}
