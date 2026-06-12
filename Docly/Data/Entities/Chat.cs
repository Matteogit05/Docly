using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Docly.Data.Entities;

public class Chat
{
    [Key]
    public int ChatId { get; set; }

    [ForeignKey(nameof(Patient))]
    public int PatientId { get; set; }

    [ForeignKey(nameof(Doctor))]
    public int DoctorId { get; set; }

    [ForeignKey(nameof(Appointment))]
    public int? AppointmentId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Active";

    public DateTime? ExpiresAt { get; set; }

    [MaxLength(300)]
    public string? Subject { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public User Patient { get; set; } = null!;

    public Doctor Doctor { get; set; } = null!;

    [InverseProperty(nameof(Appointment.Chat))]
    public Appointment? Appointment { get; set; }

    [InverseProperty(nameof(ChatMessage.Chat))]
    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

    [InverseProperty(nameof(ChatFile.Chat))]
    public ICollection<ChatFile> Files { get; set; } = new List<ChatFile>();
}
