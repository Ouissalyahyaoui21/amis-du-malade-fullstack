namespace AmisduMalade.Models
{
    public class VolunteerCoverageArea
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; } = null!;
        public string Municipality { get; set; } = "";
        public bool IsPrimary { get; set; } = false;
    }
}