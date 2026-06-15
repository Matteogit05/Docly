using System.ComponentModel.DataAnnotations;

public class Attachment
{
    public int Id { get; set; }

    [Required]
    public int MessageId { get; set; }

    public Message Message { get; set; } = null!;

    [Required]
    [StringLength(255)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [StringLength(10)]
    public string FileExtension { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string StoragePath { get; set; } = string.Empty;
}