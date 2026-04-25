namespace AmisduMalade.Models
{
    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? RecipientUserId { get; set; }
        public Guid? RecipientVolunteerId { get; set; }
        public string Channel { get; set; } = "Push";
        // Push/SMS/Email
        public string? Title { get; set; }
        public string Body { get; set; } = "";
        public string? RelatedEntityType { get; set; }
        public Guid? RelatedEntityId { get; set; }
        public string DeliveryStatus { get; set; } = "Pending";
        // Pending/Sent/Delivered/Failed
        public DateTime? SentAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}