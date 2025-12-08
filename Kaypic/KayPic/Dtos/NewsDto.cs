namespace KayPic.Dtos
{
    public class NewsDto
    {
        public int news_id { get; set; }
        public int news_ts_id { get; set; }
        public string news_status { get; set; }
        public string news_title { get; set; }
        public string news_body { get; set; }
        public char news_media_category { get; set; }
        public string news_url { get; set; }
        public int author_mp_id { get; set; }
        public string author_name { get; set; }
        public DateTime date_posted { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
    }
}
