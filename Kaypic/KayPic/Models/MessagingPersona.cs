using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KayPic.Models
{
    public class MessagingPersona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mp_id { get; set; }
        [ForeignKey("ts_id")]
        public int mp_team_season_id { get; set; }
        public TeamSeason mp_team_season { get; set; }
        public Status mp_status { get; set; }
        public PersonaCategory mp_category { get; set; }
        public string mp_fname { get; set; }
        public string mp_lname { get; set; }
        public string mp_email { get; set; }
        public DateTime created_at { get; set; }
    }
}
