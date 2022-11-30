using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MoviePlatform.ViewModels.Users
{
    public class RegistrationVM
    {

        //RegistrationVM using annotations and gets the data from form
        //primary key
        
        public int Id { get; set; }

        [DisplayName("FirstName: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string FirstName { get; set; }

        [DisplayName("Last Name: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string LastName { get; set; }

        [DisplayName("Username: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Username { get; set; }

        [DisplayName("Password: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Password { get; set; }



    }
}
