namespace Film.Models
{
    public class DirectorFilm
    {
        public int FilmId { get; set; }
        public Movie Film { get; set; }
        public int DirectorId { get; set; }
        public Director Director { get; set; }
        public Movie Name;
    }
}
