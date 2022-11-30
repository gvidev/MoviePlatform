using System.ComponentModel.DataAnnotations;

namespace MoviePlatform.Entities
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        public string  Title { get; set; }
        public string  Description { get; set; }
        public List<Actor> Actors { get; set; }//maybe list !
        public int Duration { get; set; }//in minutes
        public string imageUrl { get; set; }



    }
}
