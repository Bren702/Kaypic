using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KayPic.Models
{
    public class MessagingChatPersonaMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mcpm_id { get; set; }
        [ForeignKey("mcp_id")]
        public int mcpm_mcp_id { get; set; }
        public MessagingChatPersona mcpm_mcp { get; set; }
        [ForeignKey("ts_id")]
        public int mcpm_ts_id { get; set; }
        public TeamSeason mcpm_ts { get; set; }
        public string mcpm_message { get; set; }
        public bool is_deleted { get; set; }
        public DateTime edited_at { get; set; }
        public DateTime created_at { get; set; }
    }
}
