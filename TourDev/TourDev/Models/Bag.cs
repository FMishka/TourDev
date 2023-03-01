namespace TourDev.Models
{
    public class Bag
    {
        public int Id { get; set; }
        public int Count_of_place { get; set; }
        public int Weight { get; set; }
        public int TouristId { get; set; }
        public int Cost_for_bag_groupId { get; set; }
        public string Type { get; set; }
        public string Mark { get; set; }
        public string Tag { get; set; }
        public ICollection<Ticket> Ticket { get; set; }
        public Cost_for_bag_group Cost_for_bag_group { get; set; }
    }
}
