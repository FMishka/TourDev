namespace TourDev.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Rating { get; set; }
        public int Cost { get; set; }
        public ICollection<Tourist> Tourists { get; set; }
        public ICollection<Ticket> Ticket { get; set; }
    }
}
