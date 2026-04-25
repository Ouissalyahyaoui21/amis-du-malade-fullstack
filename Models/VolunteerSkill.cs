namespace AmisduMalade.Models
{
    public class VolunteerSkill
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; } = null!;
        public Guid SkillId { get; set; }
        public Skill Skill { get; set; } = null!;
        public string? Level { get; set; }
        // Beginner / Intermediate / Advanced
    }
}