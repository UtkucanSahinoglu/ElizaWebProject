using ElizaWebProject.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using WebEventApp.Models;

namespace ElizaWebProject.Services
{
    public class EventService : IEventService
    {
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "_eventViewModelsKey";

        public EventService(IMemoryCache memoryCache)
         => _memoryCache = memoryCache;

        public void AddEvent(EventViewModel eventViewModel)
        {
            if (_memoryCache.TryGetValue(CacheKey, out List<EventViewModel> eventViewModels))
            {
                eventViewModels.Add(eventViewModel);

                _memoryCache.Set(CacheKey, eventViewModels);
            }
            else
                return;
        }

        public IEnumerable<EventViewModel> GetEventViewModels()
        {
            if (_memoryCache.TryGetValue(CacheKey, out List<EventViewModel> eventViewModels))
                return eventViewModels;

            eventViewModels = SeedData();

            _memoryCache.Set(CacheKey, eventViewModels);

            return eventViewModels;
        }

        private static List<EventViewModel> SeedData() => new List<EventViewModel>
            {
                new EventViewModel
                {
                    Title = "Advanced Java Oracle Academy workshops",
                    Start = DateTime.Now.AddDays(5),
                    End = DateTime.Now.AddDays(6),
                    Body = "Get students started with and excited about computing. Oracle Academy hands-on workshops make first experiences with computing fun and engaging for students—while serving educators by leveraging best academic curriculum practices like project-based learning and assessment tools. Suitable for students in late primary school grades and secondary school.",
                    EventHost = "Java Workshops By Oracle",
                    WebSiteUrl = "https://academy.oracle.com/en/",
                    Location = "Austin, Texas",
                    EventImage = AppImages.Java,
                    Topics = new List<EventTopic> { new EventTopic { Value = "Getting Started with Java" }, new EventTopic { Value = "Oops" }, new EventTopic { Value = "Threading" }, new EventTopic { Value = "Classes and Structs" } },
                    Attendees = new List<string> { "Alice" ,  "Stefan", "Wilson" ,  "N.Kely"  },
                    Raw = new EventMetaData { Memo = "Tech", Creator = new EventCreateInfo { Name = "Admin", Email = "admin@event.com", Phone = "+1123456789", Company = "Private" } }
                },


                new EventViewModel(new CalendarInfo { Id = "1", BgColor = "#0099cc" })
                {
                    Title = "CSharp Workshop By Microsoft",
                    Start = DateTime.Now.AddDays(10),
                    End = DateTime.Now.AddDays(13),
                    Body = "C# is a object-oriented programming language that enables developers to build a variety of secure and robust application that run on the .NET.",
                    EventHost = "C# Programming Guide By Microsoft",
                    WebSiteUrl = "https://docs.microsoft.com/en-us/dotnet/csharp/",
                    Location = "Washington DC",
                    EventImage = AppImages.CSharp,
                    Topics = new List<EventTopic> { new EventTopic { Value = "What is C#?" }, new EventTopic { Value = "Hello World - Introduction to C#" }, new EventTopic { Value = "Self-guided C# Tutorials" }, new EventTopic { Value = "C# reference" }, new EventTopic { Value = "C# Concepts (Programming Guide)" } },
                    Attendees = new List<string> { "Kely", "Gerry"  },
                    Raw = new EventMetaData { Memo = "Tech", Creator = new EventCreateInfo { Name = "Admin", Email = "admin@event.com", Phone = "+19389949595", Company = "Private" } }
                },

                new EventViewModel(new CalendarInfo { Id = "2", Name = "Event Calendar 1", BgColor = "#b33c00" })
                {
                    Title = "jQuery Conference 2022",
                    Start = DateTime.Now.AddDays(3),
                    End = DateTime.Now.AddDays(5),
                    Body = "jQuery has become the most popular JavaScript library for developers because of it’s easy to learn and write. This course takes students through the basics of jQuery focused front-end development. This material is meant to establish a core foundation for developers. With a solid basis of jQuery and JavaScript understanding a developer will feel confident that they can add richness to their web applications.",
                    EventHost = "jQuery Training Workshops UK",
                    WebSiteUrl = "https://blog.jquery.com/",
                    Location = "England United Kingdom",
                    EventImage = AppImages.JQuery,
                    Topics = new List<EventTopic> { new EventTopic { Value = "Introduction to jQuery" }, new EventTopic { Value = "Find Something, Do Something" }, new EventTopic { Value = "The jQuery Function" }, new EventTopic { Value = "The jQuery Object" }, new EventTopic { Value = "Events" }, new EventTopic { Value = "Ajax" } },
                    Attendees = new List<string> { "Ralph Whitbeck",  "Thijs Kramer","Lantar"  },
                    Raw = new EventMetaData { Memo = "Tech", Creator = new EventCreateInfo { Name = "Admin", Email = "admin@event.com", Phone = "+19389949595", Company = "Private" } }
                },

                new EventViewModel(new CalendarInfo { Id = "2", Name = "Event Calendar 1", BgColor = "#b33c00" })
                {
                    Title = "Go Workshop By Google Inc",
                    Start = DateTime.Now.AddDays(-5),
                    End = DateTime.Now.AddDays(-3),
                    Body = "Find guides, code samples, architectural diagrams, best practices, tutorials, API references, and more to learn and Discover best practices and tutorials.",
                    EventHost = "Go Programming Language On Google Workspace",
                    WebSiteUrl = "https://go.dev/",
                    Location = "Australia",
                    EventImage = AppImages.Go,
                    Topics = new List<EventTopic> { new EventTopic { Value = "Get started" }, new EventTopic { Value = "Code samples" }, new EventTopic { Value = "Architecture" }, new EventTopic { Value = "Release notes" } },
                    Attendees = new List<string> { "Alena"  ,"G.Louis" },
                    Raw = new EventMetaData { Memo = "Tech", Creator = new EventCreateInfo { Name = "Admin", Email = "admin@event.com", Phone = "+19389949595", Company = "Private" } }
                },

                new EventViewModel(new CalendarInfo { Id = "2", Name = "Event Calendar 1", BgColor = "#b33c00" })
                {
                    Title = "JavaScript Workshop By MDN",
                    Start = DateTime.Now.AddDays(-18),
                    End = DateTime.Now.AddDays(-15),
                    Body = "JavaScript frameworks are an essential part of modern front-end web development, providing developers with proven tools for building scalable, interactive web applications. This module gives you some fundamental background knowledge about how client-side frameworks work and how they fit into your toolset, before moving on to tutorial series covering some of today's most popular ones.",
                    EventHost = "Learn web development JavaScript — Mozilla Developer Network",
                    WebSiteUrl = "https://developer.mozilla.org/en-US/docs/Learn/JavaScript",
                    Location = "Canada",
                    EventImage = AppImages.DefaultImage,
                    Topics = new List<EventTopic> { new EventTopic { Value = "JavaScript first steps" }, new EventTopic { Value = "JavaScript building blocks" }, new EventTopic { Value = "Introducing JavaScript objects" }, new EventTopic { Value = "Advanced" } },
                    Attendees = new List<string> { "Alena"  ,"G.Louis" },
                    Raw = new EventMetaData { Memo = "Tech", Creator = new EventCreateInfo { Name = "Admin", Email = "admin@event.com", Phone = "+19389949595", Company = "Private" } }
                },

                new EventViewModel(new CalendarInfo { Id = "2", Name = "Event Calendar 1", BgColor = "#E90849" })
                {
                    Title = "Angular Workshop By Architects",
                    Start = DateTime.Now.AddMonths(1),
                    End = DateTime.Now.AddDays(3).AddMonths(1),
                    Body = "Angular is a great framework for web-based business and industrial applications. It comes with numerous ready-made solutions such as data binding, form support, routing or test automation. With TypeScript, you can create type-safe applications.",
                    EventHost = "Go Programming Language On Google Workspace",
                    WebSiteUrl = "https://go.dev/",
                    Location = "Australia",
                    EventImage = AppImages.DefaultImage,
                    Topics = new List<EventTopic> { new EventTopic { Value = "Get started" }, new EventTopic { Value = "Advanced Angular" }, new EventTopic { Value = "Structure with monorepos" }, new EventTopic { Value = "Reactive architectures with RxJS notes" }, new EventTopic { Value = "Web Components with Angular Elements" } },
                    Attendees = new List<string> { "Alena"  ,"G.Louis" },
                    Raw = new EventMetaData { Memo = "Tech", Creator = new EventCreateInfo { Name = "Admin", Email = "admin@event.com", Phone = "+19389949595", Company = "Private" } }
                }
            };

        public IEnumerable<EventViewModel> GetEventViewModels(EventFilter eventFilter)
        {
            throw new NotImplementedException();
        }

        public bool IsEventExists(string eventName)
        {
            var events = GetEventViewModels();

            return events.Any(q => q.Title.Equals(eventName, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool IsEventExists(string eventName, string id)
        {
            var events = GetEventViewModels();

            return events.Any(q => q.Title.Equals(eventName, StringComparison.InvariantCultureIgnoreCase) && q.Id != id);
        }

        public EventViewModel GetEventViewModel(string value, bool queryById = false)
        {
            var events = GetEventViewModels();

            return events.FirstOrDefault(q =>
            {
                if (!queryById)
                    return q.SlugTitle.Equals(value, StringComparison.InvariantCultureIgnoreCase);
                else
                    return q.Id.Equals(value, StringComparison.InvariantCultureIgnoreCase);
            });
        }

        public void EditEvent(EventViewModel eventViewModel)
        {
            if (_memoryCache.TryGetValue(CacheKey, out List<EventViewModel> eventViewModels)) 
            {
                var eventModel = eventViewModels.FirstOrDefault(q => q.Id == eventViewModel.Id);

                var index = eventViewModels.IndexOf(eventModel);

                eventViewModels.RemoveAt(index);

                eventViewModels.Insert(index, eventViewModel);

                _memoryCache.Set(CacheKey, eventViewModels);
            }
        }

        public void RemoveEvent(string id)
        {
            if (_memoryCache.TryGetValue(CacheKey, out List<EventViewModel> eventViewModels)) 
            {
                var eventModel = eventViewModels.FirstOrDefault(q => q.Id == id);

                eventViewModels.Remove(eventModel);

                _memoryCache.Set(CacheKey, eventViewModels);
            }
        }
    }
}
