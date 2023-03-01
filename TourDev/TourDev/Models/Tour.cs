namespace TourDev.Models
{
    public class Tour
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public int Cost { get; set; }
        public ICollection<Ticket> Ticket { get; set; }
    }
}
