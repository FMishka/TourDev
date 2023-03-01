namespace TourDev.Models
{
    public class Group
    {
        public int Id { get; set; }
        public int TouristId { get; set; }
        public Tourist Tourist { get; set; }
        public ICollection<Ticket> Ticket { get; set; }
        public Excursion Excursion { get; set; }
    }
}
