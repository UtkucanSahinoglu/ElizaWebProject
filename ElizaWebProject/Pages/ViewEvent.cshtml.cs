using ElizaWebProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using WebEventApp.Models;

namespace ElizaWebProject.Pages
{
    public class ViewEventModel : PageModel
    {
        private readonly IEventService _eventService;
        private readonly ILogger<ViewEventModel> _logger;
        public EventViewModel Event { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Title { get; set; }
        public ViewEventModel(IEventService eventService, ILogger<ViewEventModel> logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        public void OnGet()
        {
            Event = _eventService.GetEventViewModel((string.IsNullOrWhiteSpace(Id) ? Title : Id), string.IsNullOrWhiteSpace(Title));
            if (Event != null)
                _logger.LogInformation("Found event entry in records: {@Event}", Event);
            else
                _logger.LogInformation("No entry found in records on view event at: {@Now}, request-path : {@Path}", DateTime.Now, Request?.Path);
        }
    }
}
