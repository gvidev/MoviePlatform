using System.ComponentModel.DataAnnotations;

namespace MoviePlatform.Entities
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }

       

    }
}
