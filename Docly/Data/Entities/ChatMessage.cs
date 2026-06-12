using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Docly.Data.Entities;

public class ChatMessage
{
    [Key]
    public int MessageId { get; set; }

    [ForeignKey(nameof(Chat))]
    public int ChatId { get; set; }

    [ForeignKey(nameof(Sender))]
    public int SenderId { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    public bool IsRead { get; set; } = false;

    public DateTime? ReadAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public Chat Chat { get; set; } = null!;

    public User Sender { get; set; } = null!;

    [InverseProperty(nameof(ChatFile.Message))]
    public ICollection<ChatFile> Files { get; set; } = new List<ChatFile>();
}
