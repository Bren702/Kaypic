using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KayPic.Models
{
    public class MessagingMedia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mm_id { get; set; }
        public char mm_media_category { get; set; }
        public string mm_url { get; set; }
        public DateTime created_at { get; set; }
    }
}
