using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Docly.Data.Entities;

public class Rating
{
    [Key]
    public int RatingId { get; set; }

    [ForeignKey(nameof(Appointment))]
    public int AppointmentId { get; set; }

    [ForeignKey(nameof(Doctor))]
    public int DoctorId { get; set; }

    [ForeignKey(nameof(Patient))]
    public int PatientId { get; set; }

    public int Score { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public Appointment Appointment { get; set; } = null!;

    public Doctor Doctor { get; set; } = null!;

    public User Patient { get; set; } = null!;
}
