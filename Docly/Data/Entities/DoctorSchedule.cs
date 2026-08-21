using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class DoctorSchedule
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int DoctorId { get; set; }

    [Required]
    public DayOfWeek DayOfWeek { get; set; } // Enum C#: 0=Domenica, 1=Lunedì, etc.

    [Required]
    public TimeSpan StartTime { get; set; } // Es. 09:00:00

    [Required]
    public TimeSpan EndTime { get; set; } // Es. 13:00:00

    [Required]
    public int SlotDurationMinutes { get; set; } = 15; // Durata di base dello slot

    [ForeignKey(nameof(DoctorId))]
    public virtual Doctor Doctor { get; set; }
}