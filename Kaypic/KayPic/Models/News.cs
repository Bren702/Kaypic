using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KayPic.Models
{
    public class News
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int news_id { get; set; }
        public Status news_status { get; set; }
        public string news_title { get; set; }
        public string news_body { get; set; }
        public char news_media_category { get; set; }
        public string news_url { get; set; }
        public DateTime date_posted { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
    }
}
