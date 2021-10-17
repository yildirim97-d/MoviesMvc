﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _036_MoviesMvcBilgeAdam.Entities
{
    public class Director
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        public bool Retired { get; set; }

        //public virtual List<Movie> Movies { get; set; } // List<Movie> Movies yerine ilişkileri tutan entity, yani MovieDirector, üzerinden List<MovieDirector> MovieDirectors tanımlanmalı

        public virtual List<MovieDirector> MovieDirectors { get; set; } // virtual: Entity Framework Lazy Loading için
    }
}