using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourDev.Models
{
    public class Tourist
    {
        public int Id { set; get; }
        public string Initials { set; get; }
        public string Passport { set; get; }
        public string Gender { set; get; }
        public int Age { set; get; }
        public int Children_count { set; get; }
        public string Visit_purpose { set; get; }
        public int HotelId { set; get; }
        public Hotel Hotel { set; get; }
        public ICollection<Ticket> Ticket { get; set; }
        public ICollection<Group> Group { get; set; }

    }
}
