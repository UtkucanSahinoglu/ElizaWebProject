using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Collections.Generic;
using WebEventApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ElizaWebProject.Services;

namespace ElizaWebProject.Pages
{
    public class EventsModel : PageModel
    {
        private readonly IEventService _eventService;
        public EventsModel(IEventService eventService)
         => _eventService = eventService;

        public List<SelectListItem> SelectListItems { get; set; }
        public IEnumerable<EventViewModel> Events { get; set; }

        [BindProperty]
        public Search SearchModel { get; set; } = new Search();

        public void OnGet()
        {
            FillValues();
        }

        private void FillValues()
        {
            Initialization();

            Events = _eventService.GetEventViewModels();

            ApplyFilter();
        }

        public IActionResult OnPost()
        {
            FillValues();

            return Page();
        }

        public async Task<IActionResult> OnGetDelete(string id) 
        {
            _eventService.RemoveEvent(id);
            await Task.CompletedTask;
            return RedirectToPage();
        }

        private void ApplyFilter()
        {
            var selectedStatus = (EventStatus)int.Parse(SelectListItems.FirstOrDefault(q => q.Selected).Value);

            if (selectedStatus != EventStatus.None)
                Events = Events.Where(q => q.ActualStatus == selectedStatus);
            
            ApplySearch();
        }

        private void ApplySearch()
        {
            if (!string.IsNullOrWhiteSpace(SearchModel.SearchKey))
            {
                var comparison = StringComparison.InvariantCultureIgnoreCase;

                Events = Events.Where(q => q.Title.Contains(SearchModel.SearchKey, comparison) ||
                    q.Start.ToShortDateString().Contains(SearchModel.SearchKey, comparison) ||
                    q.End.ToShortDateString().Contains(SearchModel.SearchKey, comparison) ||
                    q.ActualStatus.ToString().Contains(SearchModel.SearchKey, comparison));
            }
        }

        public class Search
        {
            public int StatusId { get; set; } = 1;
            public string SearchKey { get; set; }
        }

        private void Initialization()
        {
            SelectListItems = Enum.GetNames(typeof(EventStatus)).Select(s =>
            {
                var status = (short)(EventStatus)Enum.Parse(typeof(EventStatus), s);
                var selectListItem = new SelectListItem
                {
                    Text = status == 1 ? "All" : s,
                    Value = status.ToString(),
                    Selected = SearchModel.StatusId == status
                };
                return selectListItem;
            }).ToList();
        }
    }
}
