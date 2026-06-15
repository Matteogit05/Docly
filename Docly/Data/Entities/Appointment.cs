using System.ComponentModel.DataAnnotations;

public class Appointment
{
    public int Id { get; set; }

    [Required]
    public int PatientId { get; set; }

    public Patient Patient { get; set; } = null!;

    [Required]
    public int DoctorId { get; set; }

    public Doctor Doctor { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int Status { get; set; }

    public string SymptomNotes { get; set; } = string.Empty;

    public ChatSession? ChatSession { get; set; }
}