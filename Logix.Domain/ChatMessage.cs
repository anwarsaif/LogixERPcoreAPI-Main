using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Chat_Message")]
    [Index("SenderId", "ReceiverId", "IsRead", Name = "Ind_chat_msg")]
    public partial class ChatMessage
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("Message_Text")]
        public string? MessageText { get; set; }
        [Column("Sender_ID")]
        public int? SenderId { get; set; }
        [Column("Receiver_ID")]
        public int? ReceiverId { get; set; }
        [Column("Message_Status")]
        public bool? MessageStatus { get; set; }
        public bool? IsRead { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
    }
}
