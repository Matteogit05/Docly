using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

[Index(nameof(IdentityUserId), IsUnique = true)]
public class Doctor
{
    public int Id { get; set; }

    [Required]
    [StringLength(450)]
    public string IdentityUserId { get; set; } = string.Empty;

    public ApplicationUser ApplicationUser { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string Specialization { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public ICollection<FavoriteDoctor> FavoriteDoctors { get; set; } = new List<FavoriteDoctor>();

    public ICollection<DoctorAvailability> DoctorAvailabilities { get; set; } = new List<DoctorAvailability>();

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public ICollection<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();
}