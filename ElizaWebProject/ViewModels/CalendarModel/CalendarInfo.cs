namespace ElizaWebProject.Models
{
    public class CalendarInfo
    {
        public string Id { get; set; } = "1";
        public string Name { get; set; } = "Event Calendar";
        public string Color { get; set; } = "#ffffff";
        public string BgColor { get; set; } = "#9e5fff";
        public string DragBgColor { get { return BgColor; } }
        public string BorderColor { get { return BgColor; } }
        public bool Checked { get; set; } = true;
    }
}
