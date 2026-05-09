namespace AmisDuMaladeApp.Models;

public class CreatePatientModel
{
    public string FullName { get; set; } = "";
    public string? Phone { get; set; }
    public int? Age { get; set; }
    public string? Gender { get; set; }
    public string? Municipality { get; set; }
    public string? Address { get; set; }
    public string? MedicalNotes { get; set; }
}

public class PatientResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = "";
    public string? Phone { get; set; }
    public int? Age { get; set; }
    public string? Municipality { get; set; }
    public DateTime CreatedAt { get; set; }
}
