using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Film.Models
{
    public class Comment//one to many, delete render in razor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage="Введіть коментар")]
        public string Body { get; set; }
        public int FilmId { get; set; }
        public Movie Film { get; set; }
    }
}