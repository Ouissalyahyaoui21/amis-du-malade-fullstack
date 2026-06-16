namespace AmisduMalade.ViewModels
{
    public class ContributionResponseVM
    {
        public Guid    Id              { get; set; }
        public string  ContributorName { get; set; } = "";
        public string  Phone           { get; set; } = "";
        public string  Type            { get; set; } = "Money";
        public string? Amount          { get; set; }
        public string? Description     { get; set; }
        public string? Message         { get; set; }
        public string? ActivityCategory { get; set; }
        public string? PaymentMethod    { get; set; }
        public string  Status          { get; set; } = "Pending";
        public DateTime CreatedAt      { get; set; }
    }

    public class CreateContributionVM
    {
        public string  ContributorName { get; set; } = "";
        public string  Phone           { get; set; } = "";
        public string  Type            { get; set; } = "Money";
        public decimal? Amount         { get; set; }
        public string? Description     { get; set; }
        public string? Message         { get; set; }
        public string? ActivityCategory { get; set; }
        public string? PaymentMethod    { get; set; }
    }

    public class UpdateContributionStatusVM
    {
        public string Status { get; set; } = "";
    }
}
