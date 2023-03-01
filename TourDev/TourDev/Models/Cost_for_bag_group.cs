namespace TourDev.Models
{
    public class Cost_for_bag_group
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public int Transportation_cost { get; set; }
        public int Loading_cost { get; set; }
        public int Unloading_cost { get; set; }
        public ICollection<Bag> Bag { get; set; }
    }
}
