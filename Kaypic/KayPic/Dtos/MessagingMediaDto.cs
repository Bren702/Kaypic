namespace KayPic.Dtos
{
    public class MessagingMediaDto
    {
        public int mm_id { get; set; }
        public int mm_mcp_id { get; set; }
        public int mm_ts_id { get; set; }
        public string mm_media_category { get; set; }
        public string mm_url { get; set; }
        public int created_by_mp_id { get; set; }
        public DateTime created_at { get; set; }
    }
}
