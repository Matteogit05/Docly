using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Docly.Data.Entities;

public class Message
{
    [Key]
    public int Id { get; set; }

    public int ChatSessionId { get; set; }

    [ForeignKey(nameof(ChatSessionId))]
    public ChatSession ChatSession { get; set; } = null!;

    public int SenderId { get; set; }

    public int SenderType { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    [Column(TypeName = "datetime")]
    public DateTime SentAt { get; set; }

    public bool IsRead { get; set; }

    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}