namespace AmisduMalade.Models
{
    public class VolunteerAvailability
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; } = null!;
        public string DayOfWeek { get; set; } = "";
        // Monday/Tuesday/.../Sunday
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsRecurring { get; set; } = true;
    }
}