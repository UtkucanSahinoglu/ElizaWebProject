using ElizaWebProject.Models;
using ElizaWebProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebEventApp.Models;

namespace ElizaWebProject.Pages
{
    public class AddEventModel : PageModel
    {
        private readonly IEventService _eventService;
        private readonly ILogger<AddEventModel> _logger;

        public AddEventModel(IEventService eventService, ILogger<AddEventModel> logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        [BindProperty]
        public EventPageModel EventModel { get; set; } = new EventPageModel();
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            ApplyCustomValidation();

            if (ModelState.IsValid)
            {
                var eventModel = GetViewModel();
                _eventService.AddEvent(eventModel);
                _logger.LogInformation("Added new event to records: {@eventModel}", eventModel);
                return RedirectToPage("Events");
            }

            return Page();
        }

        private EventViewModel GetViewModel() => new EventViewModel(new CalendarInfo { BgColor = EventModel.Color })
        {
            Title = EventModel.Title,
            Start = EventModel.Start.Value,
            End = EventModel.End.Value,
            Body = EventModel.Description,
            EventHost = EventModel.Host,
            WebSiteUrl = EventModel.WebsiteUrl,
            Location = EventModel.Location,
            EventImage = EventModel.EventImage,
            Topics = EventModel.EventTopics.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(q => new EventTopic { Value = q }).ToList(),
            Attendees = EventModel.EventAttendees.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(q => q).ToList(),
            Raw = new EventMetaData
            {
                Memo = EventModel.EventMemo,
                //This information should get pull from claims i.e. current logged in user details.
                Creator = new EventCreateInfo
                {
                    Name = "Admin",
                    Email = "admin@event.com",
                    Phone = "+1681213478",
                    Company = "Private"
                }
            }
        };

        private void ApplyCustomValidation()
        {
            if (ModelState.IsValid && EventModel.Start.HasValue && EventModel.End.HasValue)
            {
                var isErrorExists = false;
                if (EventModel.Start.Value.CompareTo(EventModel.End.Value) > 0)
                {
                    ModelState.AddModelError("", "Start date should not be later than End date.");
                    isErrorExists = true;
                }

                if (!isErrorExists && _eventService.IsEventExists(EventModel.Title))
                {
                    ModelState.AddModelError("", "Event title is already exists.");
                }
            }
        }
    }

    public class EventPageModel
    {
        public string Color { get; set; } = "#9e5fff";

        [StringLength(100, ErrorMessage = "Maximum length is 50 characters."),
            Required(ErrorMessage = "Title is required."), RegularExpression(@"^[a-zA-Z0-9 ]*$", ErrorMessage = "No special character allowed in title.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Start date is required."),
            ModelBinder(BinderType = typeof(CustomModelBinder))]
        public DateTime? Start { get; set; }
        [Required(ErrorMessage = "End date is required."),
            ModelBinder(BinderType = typeof(CustomModelBinder))]
        public DateTime? End { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Host is required.")]
        public string Host { get; set; }
        [Required(ErrorMessage = "WebsiteUrl is required."), Url(ErrorMessage = "Invalid Url")]
        public string WebsiteUrl { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }
        public string EventImage { get; set; } = AppImages.DefaultImage;
        [Required(ErrorMessage = "Topic(s) is required.")]
        public string EventTopics { get; set; }

        [Required(ErrorMessage = "Attendee(s) is required.")]
        public string EventAttendees { get; set; }
        [Required(ErrorMessage = "Event memo is required.")]
        public string EventMemo { get; set; }
    }
}
