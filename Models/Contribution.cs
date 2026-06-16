namespace AmisduMalade.Models
{
    public class Contribution
    {
        public Guid   Id              { get; set; } = Guid.NewGuid();
        public string ContributorName { get; set; } = "";
        public string Phone           { get; set; } = "";
        public string Type            { get; set; } = "Money"; // Money | Goods | Time
        public decimal? Amount        { get; set; }
        public string? Description    { get; set; }
        public string? Message        { get; set; }
        public string? ActivityCategory { get; set; } // فئة النشاط المستهدف بالمساهمة
        public string? PaymentMethod    { get; set; } // طريقة الدفع/التسليم
        public string Status          { get; set; } = "Pending"; // Pending | Confirmed | Distributed
        public Guid?  ConfirmedByUserId { get; set; }
        public DateTime? ConfirmedAt  { get; set; }
        public DateTime CreatedAt     { get; set; } = DateTime.UtcNow;
    }
}
