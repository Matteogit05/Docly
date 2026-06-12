using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Docly.Data.Entities;

public class Appointment
{
    [Key]
    public int AppointmentId { get; set; }

    [ForeignKey(nameof(Patient))]
    public int PatientId { get; set; }

    [ForeignKey(nameof(Doctor))]
    public int DoctorId { get; set; }

    public DateTime AppointmentDateTime { get; set; }

    public int DurationMinutes { get; set; } = 30;

    [Required]
    [MaxLength(50)]
    public string AppointmentType { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Scheduled";

    public string? PatientNotes { get; set; }

    public string? DoctorNotes { get; set; }

    [ForeignKey(nameof(CancelledByUser))]
    public int? CancelledBy { get; set; }

    public string? CancellationReason { get; set; }

    public DateTime? CancelledAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public User Patient { get; set; } = null!;

    public Doctor Doctor { get; set; } = null!;

    public User? CancelledByUser { get; set; }

    [InverseProperty(nameof(Rating.Appointment))]
    public Rating? Rating { get; set; }

    [InverseProperty(nameof(Chat.Appointment))]
    public Chat? Chat { get; set; }
}
