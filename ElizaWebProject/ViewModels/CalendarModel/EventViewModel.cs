using ElizaWebProject.Models;
using ElizaWebProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebEventApp.Models
{
    public class EventViewModel : EventBase
    {
        public EventStatus ActualStatus
        {
            get
            {
                if (End.CompareTo(DateTime.Now) < 0) return EventStatus.Expired;
                else if (CheckEventStatus(Start)) return EventStatus.Current;
                else return EventStatus.Upcoming;
            }
        }
        public EventViewModel() : this(new CalendarInfo()) { }
        public EventViewModel(CalendarInfo calendarInfo) : base(calendarInfo) { }
        public string Title { get; set; }
        //This field should be unique i.e in database column should mark with unique key constraint.
        public string SlugTitle { get { return Title.ApplySlug(); } }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Status
        {
            get
            {
                if (End.CompareTo(DateTime.Now) < 0) return EventStatus.Expired.GetBadge();
                else if (CheckEventStatus(Start)) return EventStatus.Current.GetBadge();
                else return EventStatus.Upcoming.GetBadge();
            }
        }
        private bool CheckEventStatus(DateTime start) => DateTime.Now.Month == start.Month && DateTime.Now.Year == start.Year;
        public string EventHost { get; set; }
        public string WebSiteUrl { get; set; }
        public string Location { get; set; }
        public string Body { get; set; }
        public string FormattedDate { get { return $"{Start.GetFormattedDate()} to {End.GetFormattedDate()}"; } }
        public List<EventTopic> Topics { get; set; } 
        public List<string> Attendees { get; set; } 
        public bool IsActive { get; set; } = true;
        public string EventImage { get; set; }
    }
}
