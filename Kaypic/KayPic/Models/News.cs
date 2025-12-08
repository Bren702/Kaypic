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
        [ForeignKey("ts_id")]
        public int news_ts_id { get; set; }
        public TeamSeason news_ts { get; set; }
        public Status news_status { get; set; }
        public string news_title { get; set; }
        public string news_body { get; set; }
        public char news_media_category { get; set; }
        public string news_url { get; set; }
        [ForeignKey("mp_id")]
        public int author_mp_id { get; set; }
        public MessagingPersona author_mp { get; set; }
        public DateTime date_posted { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
    }
}
