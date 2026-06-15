using System.ComponentModel.DataAnnotations;

public class Message
{
    public int Id { get; set; }

    [Required]
    public int ChatSessionId { get; set; }

    public ChatSession ChatSession { get; set; } = null!;

    public int SenderId { get; set; }

    public int SenderType { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTime SentAt { get; set; }

    public bool IsRead { get; set; }

    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}