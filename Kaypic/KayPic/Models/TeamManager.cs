using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KayPic.Models
{
    public class TeamManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tm_id { get; set; }
        public string tm_fname { get; set; }
        public string tm_lname { get; set; }
        public string tm_email { get; set; }
        public DateTime created_at { get; set; }
    }
}
