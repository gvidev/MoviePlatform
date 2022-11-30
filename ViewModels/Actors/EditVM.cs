using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MoviePlatform.ViewModels.Actors
{
    public class EditVM
    {

        public int Id { get; set; }

        [DisplayName("Name: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Name { get; set; }

        [DisplayName("Age: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public int Age { get; set; }
    }
}
