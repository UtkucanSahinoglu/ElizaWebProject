using ElizaWebProject.Services;
using ElizaWebProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using WebEventApp.Models;

namespace ElizaWebProject.Apis
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppDataController : ControllerBase
    {
        private readonly IEventService _eventService;

        public AppDataController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        [ActionName("getevents")]
        public IActionResult GetEvents([FromBody] EventFilter eventFilter)
        {
            var events = _eventService.GetEventViewModels();

            if ((bool)eventFilter?.From.HasValue)
            {
                var filter = events.ApplyFilter(eventFilter.From.Value, eventFilter.To.Value);
                return Ok(new { Events = filter });
            }
            else if ((bool)eventFilter?.CurrentViewDate.HasValue)
            {
                var filter = events.ApplyFilter(eventFilter.CurrentViewDate.Value);
                return Ok(new { Events = filter });
            }
            else
                return Ok(new { Events = events });
        }

        [DisableRequestSizeLimit, HttpPost]
        [ActionName("getimagedata")]
        public async Task<IActionResult> ProcessImageFile()
        {
            var imageData = string.Empty;
            var formCollection = await Request.ReadFormAsync();

            var file = formCollection.Files?[0];

            if (file != null)
            {
                using var ms = new MemoryStream();
                file.CopyTo(ms);
                ms.Position = 0;
                var base64 = Convert.ToBase64String(ms.ToArray());
                imageData = $"data:image/{file.FileName.Split('.')[1]};base64,{base64}";
            }

            return Ok(new { ImageData = imageData });
        }
    }
}
