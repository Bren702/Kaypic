using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KayPic.Models
{
    public class TeamSeason
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ts_id { get; set; }
        public Status ts_status { get; set; }
        public string ts_chat_key { get; set; }
        public string ts_name { get; set; }
        public DateTime created_at { get; set; }
    }
}
