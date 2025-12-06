using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KayPic.Models
{
    public class MessagingPersona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mp_id { get; set; }
        public Status mp_status { get; set; }
        public string mp_category { get; set; }
        public string mp_fname { get; set; }
        public string mp_lname { get; set; }
        public string mp_email { get; set; }
        public DateTime created_at { get; set; }
    }
}
