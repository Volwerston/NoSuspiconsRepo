using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Film.Models
{
    public class Director
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DirectorFilm> Films { get; set; }//many to many

        public Director()
        {
            Films = new List<DirectorFilm>();
        }
    }
}
