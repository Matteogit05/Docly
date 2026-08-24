using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(PatientId), nameof(DoctorId))]
public class FavoriteDoctor
{
    public int PatientId { get; set; }

    public Patient Patient { get; set; } = null!;

    public int DoctorId { get; set; }

    public Doctor Doctor { get; set; } = null!;
}