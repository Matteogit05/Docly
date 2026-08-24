using System.ComponentModel.DataAnnotations;

namespace Docly.Data.Entities;

public class Notification
{
    public int Id { get; set; }
    
    [Required]
    public string IdentityUserId { get; set; } = string.Empty;
    public ApplicationUser? ApplicationUser { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public bool IsRead { get; set; } = false;

    [MaxLength(500)]
    public string? TargetUrl { get; set; }
}