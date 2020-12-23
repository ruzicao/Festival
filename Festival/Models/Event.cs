using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Festival.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        [Range(1950,2018)]
        public int Year { get; set; }

        public Place Place { get; set; }
        public int PlaceId { get; set; }
    }
}