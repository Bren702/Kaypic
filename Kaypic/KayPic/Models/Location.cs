namespace KayPic.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public required string Country { get; set; }
        public required string Region { get; set; }
        public required string City { get; set; }
        public string? PostalCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

    }
}
