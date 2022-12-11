using System;

namespace WebEventApp.Models
{
    public class EventFilter 
    {
        public DateTime? CurrentViewDate { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
