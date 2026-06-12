using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Docly.Data.Entities;

[Index(nameof(Email), IsUnique = true)]
public class Doctor
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Specialization { get; set; } = string.Empty;

    public string? Bio { get; set; }

    public bool IsActive { get; set; }

    public ICollection<FavoriteDoctor> FavoriteDoctors { get; set; } = new List<FavoriteDoctor>();

    public ICollection<DoctorAvailability> Availabilities { get; set; } = new List<DoctorAvailability>();

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public ICollection<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();
}