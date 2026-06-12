using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Docly.Data.Entities;

public class DoctorAvailability
{
    [Key]
    public int AvailabilityId { get; set; }

    [ForeignKey(nameof(Doctor))]
    public int DoctorId { get; set; }

    public int DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int SlotDurationMinutes { get; set; } = 30;

    public Doctor Doctor { get; set; } = null!;
}
