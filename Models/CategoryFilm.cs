namespace Film.Models
{
    public class CategoryFilm
    {
        public int FilmId{get;set;}
        public Movie Film{get;set;}
        public int CategoryId{get;set;}
        public Category Category{get;set;}
        public Movie Name;
    }
}