namespace KayPic.Dtos
{
    public class MessagingChatPersonaDto
    {
        public int mcp_id { get; set; }
        public int mcp_mc_id { get; set; }
        public int mcp_ts_id { get; set; }
        public int mcp_mp_id { get; set; }
        public string mcp_status { get; set; }
        public DateTime added_at { get; set; }
        public string mcp_url { get; set; }
        public DateTime created_at { get; set; }
    }
}
