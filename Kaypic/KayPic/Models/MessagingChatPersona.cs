using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KayPic.Models
{
    public class MessagingChatPersona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mcp_id { get; set; }
        [ForeignKey("mc_id")]
        public int mcp_mc_id { get; set; }
        public MessagingChat mcp_mc { get; set; }
        [ForeignKey("ts_id")]
        public int mcp_ts_id { get; set; }
        public TeamSeason mcp_ts { get; set; }
        [ForeignKey("mp_id")]
        public int mcp_mp_id { get; set; }
        public MessagingPersona mcp_mp { get; set; }
        public Status mcp_status { get; set; }
        public DateTime added_at { get; set; }
        public string mcp_url { get; set; }
        public DateTime created_at { get; set; }
    }
}
