using System.Collections.Generic;
using WebEventApp.Models;

namespace ElizaWebProject.Services
{
    public interface IEventService
    {
        IEnumerable<EventViewModel> GetEventViewModels();
        void RemoveEvent(string id);
        void AddEvent(EventViewModel eventViewModel);
        void EditEvent(EventViewModel eventViewModel);
        bool IsEventExists(string eventName);
        bool IsEventExists(string eventName, string id);
        EventViewModel GetEventViewModel(string value, bool queryById = false);
        IEnumerable<EventViewModel> GetEventViewModels(EventFilter eventFilter);
    }
}
