using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KayPic.Models
{
    public class MessagingChat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mc_id { get; set; }
        public Status mc_status { get; set; }
        public string mc_title { get; set; }
        public DateTime created_at { get; set; }
    }
}
