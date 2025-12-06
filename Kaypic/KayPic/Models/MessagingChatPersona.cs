using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KayPic.Models
{
    public class MessagingChatPersona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mcp_id { get; set; }
        public Status mcp_status { get; set; }
        public DateTime added_at { get; set; }
        public char mcp_media_category { get; set; }
        public string mcp_url { get; set; }
        public DateTime created_at { get; set; }
    }
}
