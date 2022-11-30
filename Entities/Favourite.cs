using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviePlatform.Entities
{
    public class Favourite
    {
        [Key]
        public int Id { get; set; }


        public int userId { get; set; }
        [ForeignKey(nameof(userId))]
        public User User { get; set; }

        public int movieId { get; set; }
        [ForeignKey(nameof(movieId))]
        public Movie Movie { get; set; }

    }
}
