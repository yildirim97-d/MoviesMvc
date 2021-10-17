using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _036_MoviesMvcBilgeAdam.Entities
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(4)]
        public string ProductionYear { get; set; }

        public double? BoxOfficeReturn { get; set; }

        //public virtual List<Director> Directors { get; set; } // List<Director> Directors yerine ilişkileri tutan entity, yani MovieDirector, üzerinden List<MovieDirector> MovieDirectors tanımlanmalı
        public virtual List<MovieDirector> MovieDirectors { get; set; } // virtual: Entity Framework Lazy Loading için

        public virtual List<Review> Reviews { get; set; } // virtual: Entity Framework Lazy Loading için
    }
}