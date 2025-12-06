using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KayPic.Models
{
    public class Parent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int parent_id { get; set; }
        public string parent_fname { get; set; }
        public string parent_lname { get; set; }
        public string parent_email { get; set; }
        public DateTime created_at { get; set; }
    }
}
