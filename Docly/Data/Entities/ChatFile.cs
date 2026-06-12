using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Docly.Data.Entities;

public class ChatFile
{
    [Key]
    public int FileId { get; set; }

    [ForeignKey(nameof(Chat))]
    public int ChatId { get; set; }

    [ForeignKey(nameof(Message))]
    public int? MessageId { get; set; }

    [ForeignKey(nameof(UploadedByUser))]
    public int UploadedBy { get; set; }

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    public string FileUrl { get; set; } = string.Empty;

    public long? FileSize { get; set; }

    [MaxLength(50)]
    public string? FileType { get; set; }

    public DateTime UploadedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Chat Chat { get; set; } = null!;

    public ChatMessage? Message { get; set; }

    public User UploadedByUser { get; set; } = null!;
}
