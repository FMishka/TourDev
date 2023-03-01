namespace TourDev.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Cost { get; set; }
        public ICollection<Ticket> Ticket;
    }
}
