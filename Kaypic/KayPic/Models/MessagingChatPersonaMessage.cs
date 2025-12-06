using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KayPic.Models
{
    public class MessagingChatPersonaMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mcpm_id { get; set; }
        public string mcpm_message { get; set; }
        public bool is_delected { get; set; }
        public DateTime edited_at { get; set; }
        public DateTime created_at { get; set; }
    }
}
