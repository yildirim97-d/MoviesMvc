namespace _036_MoviesMvcBilgeAdam.Entities
{
    public class MovieDirector
    {
        public int Id { get; set; }

        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; } // virtual: Entity Framework Lazy Loading için

        public int DirectorId { get; set; }
        public virtual Director Director { get; set; } // virtual: Entity Framework Lazy Loading için
    }
}