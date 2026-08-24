using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

[Index(nameof(IdentityUserId), IsUnique = true)]
[Index(nameof(TaxId), IsUnique = true)]
public class Patient
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

    public DateTime DateOfBirth { get; set; }

    [Required]
    [StringLength(16)]
    public string TaxId { get; set; } = string.Empty;

    public ICollection<FavoriteDoctor> FavoriteDoctors { get; set; } = new List<FavoriteDoctor>();

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public ICollection<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();
}