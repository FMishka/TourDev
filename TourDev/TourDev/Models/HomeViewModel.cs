using Microsoft.AspNetCore.Mvc.Rendering;

namespace TourDev.Models
{
    public class HomeViewModel
    {
        public string Name { get; set; }
        public List<SelectListItem> List { get; set; }
    }
}
