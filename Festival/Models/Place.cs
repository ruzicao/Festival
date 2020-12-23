using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Festival.Models
{
    public class Place
    {
        [Key]
        public int Id { get; set; }
        public string Location { get; set; }

        [Range(0, 99999)]
        public int ZipCode { get; set; }
    }
}