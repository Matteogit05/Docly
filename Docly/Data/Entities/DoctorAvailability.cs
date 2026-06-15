using System.ComponentModel.DataAnnotations;

public class DoctorAvailability
{
    public int Id { get; set; }

    [Required]
    public int DoctorId { get; set; }

    public Doctor Doctor { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public bool IsBooked { get; set; }
}