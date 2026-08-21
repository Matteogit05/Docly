using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class DoctorAbsence
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int DoctorId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [MaxLength(200)]
    public string? Reason { get; set; } // Es. "Ferie estive", "Malattia" (Nullable)

    [ForeignKey(nameof(DoctorId))]
    public virtual Doctor Doctor { get; set; }
}
