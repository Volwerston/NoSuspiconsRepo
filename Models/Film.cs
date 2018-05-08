using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Film.Models
{
    public class Movie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }

        public ICollection<Comment> Comments {get;set;}

        public ICollection<CategoryFilm> Categories { get; set; }
        public ICollection<Mark> Marks { get; set; }
        public Movie()
        {
            Categories = new List<CategoryFilm>();
            Comments = new List<Comment>();
            Marks = new List<Mark>();
        }
    }
}