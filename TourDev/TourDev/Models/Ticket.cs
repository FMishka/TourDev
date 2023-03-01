namespace TourDev.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int? TouristId { get; set; }
        public DateTime Arrival_date { get; set; }
        public DateTime Departure_date { get; set; }
        public string Country { get; set; }
        public int? BagId { get; set; }
        public int? TourId { get; set; }
        public int? GroupId { get; set; }
        public int? HotelId { get; set; }
        public int? Cost { get; set; } 
        public int? FlightId { get; set; }
        public Tourist Tourist { get; set; }
        public Hotel Hotel { get; set; }
        public Flight Flight { get; set; }
        public Bag Bag { get; set; }
        public Tour Tour { get; set; }
        public Group Group { get; set; }
    }
}
