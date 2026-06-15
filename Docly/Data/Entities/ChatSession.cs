using System.ComponentModel.DataAnnotations;

public class ChatSession
{
    public int Id { get; set; }

    [Required]
    public int PatientId { get; set; }

    public Patient Patient { get; set; } = null!;

    [Required]
    public int DoctorId { get; set; }

    public Doctor Doctor { get; set; } = null!;

    public int? AppointmentId { get; set; }

    public Appointment? Appointment { get; set; }

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public ICollection<Message> Messages { get; set; } = new List<Message>();
}