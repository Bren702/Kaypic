namespace KayPic.Dtos
{
    public class PlayerDto
    {
        public int player_id { get; set; }
        public string player_fname { get; set; }
        public string player_lname { get; set; }
        public string player_email { get; set; }
        public string player_guardian_fname { get; set; }
        public string player_guardian_lname { get; set; }
        public string player_guardian_email { get; set; }
        public DateTime created_at { get; set; }
    }
}
