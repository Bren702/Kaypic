namespace KayPic.Dtos
{
    public class MessagingChatPersonaMessageDto
    {
        public int mcpm_id { get; set; }
        public int mcpm_mcp_id { get; set; }
        public int mcpm_ts_id { get; set; }
        public string mcpm_message { get; set; }
        public bool is_deleted { get; set; }
        public DateTime edited_at { get; set; }
        public DateTime created_at { get; set; }
    }
}
