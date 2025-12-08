using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KayPic.Models
{
    public class MessagingMedia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mm_id { get; set; }
        [ForeignKey("mcp_id")]
        public int mm_mcp_id { get; set; }
        public MessagingChatPersona mm_mcp { get; set; }
        [ForeignKey("ts_id")]
        public int mm_ts_id { get; set; }
        public TeamSeason mm_ts { get; set; }
        public MediaCategory mm_media_category { get; set; }
        public string mm_url { get; set; }
        [ForeignKey("mp_id")]
        public int created_by_mp_id { get; set; }
        public MessagingPersona created_by_mp { get; set; }
        public DateTime created_at { get; set; }
    }
}
