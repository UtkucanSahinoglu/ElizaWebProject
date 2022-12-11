using ElizaWebProject.Models;
using ElizaWebProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using WebEventApp.Models;
using ElizaWebProject.Pages;

namespace ElizaWebProject.Pages
{
    public class EditEventModel : PageModel
    {
        private readonly IEventService _eventService;
        private readonly ILogger<EditEventModel> _logger;

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty]
        public EventPageModel EventModel { get; set; }

        public EditEventModel(IEventService eventService, ILogger<EditEventModel> logger)
        {
            _eventService = eventService; _logger = logger;
        }

        public void OnGet()
        {
            EventModel = GetViewModel();
        }

        public IActionResult OnPost()
        {
            ApplyCustomValidation();

            if (ModelState.IsValid)
            { 
                var eventModel = GetEventModel();
                _eventService.EditEvent(eventModel);
                _logger.LogInformation("Added new event to records: {@eventModel}", eventModel);
                return RedirectToPage("Events");
            }

            return Page();
        }

        private EventViewModel GetEventModel() => new EventViewModel(new CalendarInfo { BgColor = EventModel.Color })
        {
            Id = Id,
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

        private EventPageModel GetViewModel()
        {
            var model = _eventService.GetEventViewModel(Id, true);

            return new EventPageModel
            {
                Title = model.Title,
                Start = model.Start,
                End = model.End,
                Color = model.BgColor,
                Description = model.Body,
                Host = model.EventHost,
                WebsiteUrl = model.WebSiteUrl,
                EventMemo = model.Raw.Memo,
                Location = model.Location,
                EventImage = model.EventImage,
                EventAttendees = string.Join(",", model.Attendees),
                EventTopics = string.Join(",", model.Topics.Select(q => q.Value))
            };
        }

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

                if (!isErrorExists && _eventService.IsEventExists(EventModel.Title, Id))
                {
                    ModelState.AddModelError("", "Event title is already exists.");
                }
            }
        }
    }
}
