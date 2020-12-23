using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Festival.Models
{
    public class EventStatisticsDTO
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public decimal SumPrice { get; set; }
    }
}