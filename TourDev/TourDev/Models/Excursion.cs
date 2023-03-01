using System.ComponentModel.DataAnnotations.Schema;

namespace TourDev.Models
{
    public class Excursion
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Excursions { get; set; }
        public DateTime Time { get; set; }
        public int AgentId { get; set; }
        public int Cost { get; set; }
        public ICollection<Group> Group { get; set; }
        
    }
}
