using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Docly.Data.Entities;

[PrimaryKey(nameof(PatientId), nameof(DoctorId))]
public class FavoriteDoctor
{
    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    [ForeignKey(nameof(PatientId))]
    public Patient Patient { get; set; } = null!;

    [ForeignKey(nameof(DoctorId))]
    public Doctor Doctor { get; set; } = null!;
}