using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Docly.Data.Entities;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? PhoneNumber { get; set; }

    [MaxLength(500)]
    public string? ProfileImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [InverseProperty(nameof(Doctor.User))]
    public Doctor? DoctorProfile { get; set; }

    [InverseProperty(nameof(Appointment.Patient))]
    public ICollection<Appointment> PatientAppointments { get; set; } = new List<Appointment>();

    [InverseProperty(nameof(Appointment.CancelledByUser))]
    public ICollection<Appointment> CancelledAppointments { get; set; } = new List<Appointment>();

    [InverseProperty(nameof(FavoriteDoctor.Patient))]
    public ICollection<FavoriteDoctor> FavoriteDoctors { get; set; } = new List<FavoriteDoctor>();

    [InverseProperty(nameof(Chat.Patient))]
    public ICollection<Chat> PatientChats { get; set; } = new List<Chat>();

    [InverseProperty(nameof(ChatMessage.Sender))]
    public ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();

    [InverseProperty(nameof(ChatFile.UploadedByUser))]
    public ICollection<ChatFile> UploadedFiles { get; set; } = new List<ChatFile>();

    [InverseProperty(nameof(Rating.Patient))]
    public ICollection<Rating> RatingsGiven { get; set; } = new List<Rating>();
}
