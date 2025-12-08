namespace KayPic.Dtos
{
    public class MessagingPersonaDto
    {
        public int mp_id { get; set; }
        public int mp_team_season_id { get; set; }
        public string mp_status { get; set; }
        public string mp_category { get; set; }
        public string mp_fname { get; set; }
        public string mp_lname { get; set; }
        public string mp_email { get; set; }
        public DateTime created_at { get; set; }
    }
}
