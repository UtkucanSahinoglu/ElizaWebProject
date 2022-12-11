using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElizaWebProject.Models;
using ElizaWebProject.Services;
using WebEventApp.Models;
using ElizaWebProject.ViewModels;

namespace ElizaWebProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IEventService _eventService;
        private readonly ILogger<IndexModel> _logger;
        public CalendarViewModel Calendar { get; set; } = new CalendarViewModel();
        public JsonSerializerOptions SerializerOptions { get; set; }
        = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        public IndexModel(ILogger<IndexModel> logger, IEventService eventService)
        {
            _logger = logger; 
            _eventService = eventService;
        }

        public void OnGet()
        {
            Calendar.EventViewModels.AddRange(_eventService.GetEventViewModels());

            UpdateUpcomingEvents();
        }

        private void UpdateUpcomingEvents()
        {
            var upcomigs = Calendar.EventViewModels.ApplyFilter(false, EventStatus.Upcoming).OrderBy(q => q.Start).Take(2).ToList();
            Calendar.EventViewModels = Calendar.EventViewModels.ApplyFilter(true, EventStatus.Current | EventStatus.Expired).ToList();
            Calendar.Upcomings = upcomigs;
        }

        public class CalendarViewModel
        {
            public List<CalendarInfo> CalendarList { get; set; } = new List<CalendarInfo> { new CalendarInfo(), new CalendarInfo { Id = "2", Name = "Event Calendar 1" } };
            public List<EventViewModel> EventViewModels { get; set; } = new List<EventViewModel>();
            public List<EventViewModel> Upcomings { get; set; }
        }
    }
}
