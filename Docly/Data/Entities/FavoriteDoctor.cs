using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Docly.Data.Entities;

public class FavoriteDoctor
{
    [Key]
    public int FavoriteDoctorId { get; set; }

    [ForeignKey(nameof(Patient))]
    public int PatientId { get; set; }

    [ForeignKey(nameof(Doctor))]
    public int DoctorId { get; set; }

    public DateTime AddedAt { get; set; }

    public string? Notes { get; set; }

    public User Patient { get; set; } = null!;

    public Doctor Doctor { get; set; } = null!;
}
