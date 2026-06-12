using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Docly.Data.Entities;

public class Attachment
{
    [Key]
    public int Id { get; set; }

    public int MessageId { get; set; }

    [ForeignKey(nameof(MessageId))]
    public Message Message { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string FileExtension { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string StoragePath { get; set; } = string.Empty;
}