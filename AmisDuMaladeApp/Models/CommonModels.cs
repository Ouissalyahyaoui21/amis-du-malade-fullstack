namespace AmisDuMaladeApp.Models;

public class ApiIdResponse
{
    public Guid Id { get; set; }
    public string Message { get; set; } = "";
}

public class ApiMessageResponse
{
    public string Message { get; set; } = "";
}

public class AlertResponse
{
    public Guid Id { get; set; }
    public string Type { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

public class AssignmentResponse
{
    public Guid Id { get; set; }
    public string Status { get; set; } = "";
    public Guid VolunteerId { get; set; }
    public Guid CareRequestId { get; set; }
    public DateTime AssignedAt { get; set; }
}
