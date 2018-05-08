using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Film.Models
{
    public class Mark
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName {get;set;}
        public int MarkValue { get; set; }
        public int FilmId { get; set; }
        public Movie Film { get; set; }
    }  
}