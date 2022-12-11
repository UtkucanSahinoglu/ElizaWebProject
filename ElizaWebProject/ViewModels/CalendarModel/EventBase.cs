using ElizaWebProject.Models;
using System;

namespace WebEventApp.Models
{
    public abstract class EventBase
    {
        private CalendarInfo Calendar { get; set; }
        public EventBase(CalendarInfo calendarInfo) => Calendar = calendarInfo;

        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string CalendarId { get => Calendar.Id; }
        public bool IsReadOnly { get => true; }
        public bool IsAllday { get => true; }
        public bool IsPrivate { get => false; }
        public string RecurrenceRule { get => string.Empty; }
        public string State => "Free";
        public string Color => Calendar.Color;
        public string BgColor => Calendar.BgColor;
        public string DragBgColor => Calendar.DragBgColor;
        public string BorderColor => Calendar.BorderColor;
        public string Category => "allday";
        public EventMetaData Raw { get; set; }
    }
}
