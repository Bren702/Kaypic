namespace KayPic.Dtos
{
    public class MessagingChatDto
    {
        public int mc_id { get; set; }
        public int mc_team_season_id { get; set; }
        public string mc_status { get; set; }
        public string mc_title { get; set; }
        public int created_by_mp_id { get; set; }
        public DateTime created_at { get; set; }
    }
}
