using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using WebEventApp.Models;

namespace ElizaWebProject.ViewModels
{
    public static class AppExtensions
    {
        private static readonly EventStatus Flags = EventStatus.Current | EventStatus.Expired | EventStatus.Upcoming;

        public static string GetBadge(this EventStatus eventStatus)
        {
            string htmlclass = eventStatus switch
            {
                EventStatus.Current => "badge badge-pill badge-success",
                EventStatus.Upcoming => "badge badge-pill badge-dark",
                EventStatus.Expired => "badge badge-pill badge-danger",
                _ => string.Empty,
            };

            return @$"<span class=""{htmlclass}"">{eventStatus}</span>";
        }

        public static string GetFormattedDate(this DateTime dateTime)
        {
            var cultureInfo = new CultureInfo("en-US");
            return $"{cultureInfo.DateTimeFormat.GetMonthName(dateTime.Month)} {dateTime.Day}, {dateTime.Year}";
        }

        public static IEnumerable<EventViewModel> ApplyFilter(this IEnumerable<EventViewModel> eventViewModels, bool isCurrentMonth, EventStatus? eventStatus)
        {
            if (!isCurrentMonth && !eventStatus.HasValue)
                return eventViewModels.Where(q => q.IsActive);

            else if (isCurrentMonth && !eventStatus.HasValue)
                return eventViewModels.Where(q => q.IsActive && q.Start.IsCurrentMonthEvent() || q.End.IsCurrentMonthEvent());

            else if (!isCurrentMonth && eventStatus.HasValue)
                return eventViewModels.Where(q => q.IsActive && q.ActualStatus == eventStatus.Value);

            else
                return eventViewModels.Where(q => q.IsActive && q.Start.IsCurrentMonthEvent() || q.End.IsCurrentMonthEvent() && Flags.HasFlag(eventStatus));
        }

        public static string ApplySlug(this string value) => Regex.Replace(value.Replace(" ", "-").ToString()!, @"[^\w-]+", "", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(100)).ToLowerInvariant();

        public static IEnumerable<EventViewModel> ApplyFilter(this IEnumerable<EventViewModel> eventViewModels, DateTime selectedMonth)
        => eventViewModels.Where(q => q.Start.IsCurrentMonthEvent(selectedMonth) || q.End.IsCurrentMonthEvent(selectedMonth));

        public static IEnumerable<EventViewModel> ApplyFilter(this IEnumerable<EventViewModel> eventViewModels, DateTime fromDate, DateTime todate)
        => eventViewModels.Where(q => q.Start.FromEventFilter(fromDate) && q.End.ToEventFilter(todate));
         
        private static bool FromEventFilter(this DateTime dateTime, DateTime fromDate) => dateTime.CompareTo(fromDate) >= 0;
        private static bool ToEventFilter(this DateTime dateTime, DateTime toDate) => dateTime.CompareTo(toDate) <= 0;
         
        private static bool IsCurrentMonthEvent(this DateTime dateTime) => dateTime.Month == DateTime.Now.Month && dateTime.Year == DateTime.Now.Year;
        private static bool IsCurrentMonthEvent(this DateTime dateTime, DateTime selectedMonth) => dateTime.Month == selectedMonth.Month && dateTime.Year == selectedMonth.Year;
    }

    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            if (value is null)
            {
                return null;
            }

            if (value is string)
            {
                return value.ToString().ApplySlug();
            }

            return null;
        }
    }
}
