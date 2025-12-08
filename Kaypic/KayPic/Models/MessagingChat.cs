using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KayPic.Models
{
    public class MessagingChat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mc_id { get; set; }
        [ForeignKey("ts_id")]
        public int mc_team_season_id { get; set; }
        public TeamSeason mc_team_season { get; set; }
        public Status mc_status { get; set; }
        public string mc_title { get; set; }
        [ForeignKey("mp_id")]
        public int created_by_mp_id { get; set; }
        public MessagingPersona created_by_mp { get; set; }
        public DateTime created_at { get; set; }
    }
}
