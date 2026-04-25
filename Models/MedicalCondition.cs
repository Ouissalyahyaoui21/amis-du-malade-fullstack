namespace AmisduMalade.Models
{
    public class MedicalCondition
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public string? Category { get; set; }
        public string? Description { get; set; }
    }
}