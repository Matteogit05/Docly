using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Docly.Data.Entities;

public class Doctor
{
    [Key]
    [ForeignKey(nameof(User))]
    public int DoctorId { get; set; }

    [Required]
    [MaxLength(100)]
    public string LicenseNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Specialization { get; set; } = string.Empty;

    public string? Biography { get; set; }

    [Precision(3, 2)]
    public decimal? AverageRating { get; set; }

    public int TotalRatings { get; set; } = 0;

    public bool IsVerified { get; set; } = false;

    public DateTime? VerifiedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public User User { get; set; } = null!;

    [InverseProperty(nameof(DoctorAvailability.Doctor))]
    public ICollection<DoctorAvailability> Availabilities { get; set; } = new List<DoctorAvailability>();

    [InverseProperty(nameof(Appointment.Doctor))]
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    [InverseProperty(nameof(FavoriteDoctor.Doctor))]
    public ICollection<FavoriteDoctor> FavoriteDoctors { get; set; } = new List<FavoriteDoctor>();

    [InverseProperty(nameof(Chat.Doctor))]
    public ICollection<Chat> Chats { get; set; } = new List<Chat>();

    [InverseProperty(nameof(Rating.Doctor))]
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
