namespace AmisduMalade.Models
{
    public class VisitRating
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid VisitSessionId { get; set; }
        public VisitSession VisitSession { get; set; } = null!;
        public int Score { get; set; }
        // 1-5
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}