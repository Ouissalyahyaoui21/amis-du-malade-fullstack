namespace AmisduMalade.Models
{
    public class CareRequestRequiredSkill
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CareRequestId { get; set; }
        public CareRequest CareRequest { get; set; } = null!;
        public Guid SkillId { get; set; }
        public Skill Skill { get; set; } = null!;
        public string? RequiredLevel { get; set; }
        public bool Mandatory { get; set; } = true;
    }
}