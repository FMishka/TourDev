using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TourDev.Models;

namespace TourDev.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private ApplicationContext db;
        public AdminController(ApplicationContext context)
        {
            db = context;
        }
        private List<SelectListItem> GetSelectListItems<T>(List<T> massOfItmes, string selected = null)
        {
            var list = new List<SelectListItem>();
            foreach (var item in massOfItmes)
            {
                if (selected == Convert.ToString(item))
                {
                    list.Add(new SelectListItem { Text = Convert.ToString(item), Value = Convert.ToString(item), Selected = true });
                }
                else
                {
                    list.Add(new SelectListItem { Text = Convert.ToString(item), Value = Convert.ToString(item) });
                }
            }
            return list;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Tickets(int? id)
        {
            var model = db.Ticket.Include(t => t.Tourist)
                .Include(h => h.Hotel)
                .Include(b => b.Bag)
                .Include(to => to.Tour)
                .Include(f => f.Flight);
            if (id != null)
            {
                //удаление
            }
            return View(model);
        }

        public IActionResult Add(int? tourist, DateTime? startDate, DateTime? endDate, string country, int? bagId, int? tourId, int? groupId, int? hotelId, int? costs, int? fly)
        {
            var tourists = db.Tourist.Select(t => new { t.Id, t.Initials });
            ViewBag.Tourists = new SelectList(tourists, "Id", "Initials");
            var bags = db.Bag.Select(t => t.Id).ToList();
            ViewBag.Bag = GetSelectListItems(bags);
            var tour = db.Tour.Select(t => new { t.Id, t.Name });
            ViewBag.Tour = new SelectList(tour, "Id", "Name");
            var group = db.Group.Select(g => g.Id).Distinct().ToList();
            ViewBag.Group = GetSelectListItems(group);
            var hotel = db.Hotel.Select(h => new {h.Id, h.Name});
            ViewBag.Hotel = new SelectList(hotel, "Id", "Name");
            var f = db.Flight.Select(f => new {f.Id, f.Name});
            ViewBag.F = new SelectList(f, "Id", "Name");
            if (tourist != null && startDate != null && endDate != null && country != null && bagId != null && tourId != null && groupId != null && hotelId != null && costs != null && fly != null)
            {
                Ticket t = new Ticket { 
                    TouristId = tourist, 
                    Arrival_date = (DateTime)startDate,
                    Departure_date = (DateTime)endDate,
                    Country = country,
                    BagId = bagId,
                    TourId = tourId,
                    GroupId = groupId,
                    HotelId = hotelId,
                    Cost = costs,
                    FlightId = fly
                };
                db.Add(t);
                db.SaveChanges();
                return Redirect("Tickets");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id, int? tourist, DateTime? startDate, DateTime? endDate, string country, int? bagId, int? tourId, int? groupId, int? hotelId, int? costs, int? fly)
        {
            Ticket tick = db.Ticket.Find(id);
            var tourists = db.Tourist.Select(t => new { t.Id, t.Initials });
            ViewBag.Tourists = new SelectList(tourists, "Id", "Initials", tick.TouristId);
            var bags = db.Bag.Select(t => t.Id).ToList();
            ViewBag.Bag = GetSelectListItems(bags, Convert.ToString(tick.BagId));
            var tour = db.Tour.Select(t => new { t.Id, t.Name });
            ViewBag.Tour = new SelectList(tour, "Id", "Name", tick.TourId);
            var group = db.Group.Select(g => g.Id).Distinct().ToList();
            ViewBag.Group = GetSelectListItems(group, Convert.ToString(tick.GroupId));
            var hotel = db.Hotel.Select(h => new { h.Id, h.Name });
            ViewBag.Hotel = new SelectList(hotel, "Id", "Name", tick.HotelId);
            var f = db.Flight.Select(f => new { f.Id, f.Name });
            ViewBag.F = new SelectList(f, "Id", "Name", tick.FlightId);

            return View(tick);
        }
        [HttpPost]
        public IActionResult EditLogic(int id, int? tourist, DateTime? startDate, DateTime? endDate, string country, int? bagId, int? tourId, int? groupId, int? hotelId, int? costs, int? fly)
        {
            Ticket t = new Ticket
            {
                Id = id,
                TouristId = tourist,
                Arrival_date = (DateTime)startDate,
                Departure_date = (DateTime)endDate,
                Country = country,
                BagId = bagId,
                TourId = tourId,
                GroupId = groupId,
                HotelId = hotelId,
                Cost = costs,
                FlightId = fly
            };
            db.Entry(t).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("Tickets");
        }
        public IActionResult Update(int id, int? tourist, DateTime? startDate, DateTime? endDate, string country, int? bagId, int? tourId, int? groupId, int? hotelId, int? costs, int? fly)
        {
            Ticket t = new Ticket
            {
                TouristId = tourist,
                Arrival_date = (DateTime)startDate,
                Departure_date = (DateTime)endDate,
                Country = country,
                BagId = bagId,
                TourId = tourId,
                GroupId = groupId,
                HotelId = hotelId,
                Cost = costs,
                FlightId = fly
            };
            Console.WriteLine("Тикет сформирован!");
            return Redirect("Tickets");
        }
        public IActionResult View2(int id)
        {
            Ticket tick = db.Ticket.Find(id);
            return View(tick);
        }
        public IActionResult Delete(int id)
        {
            Ticket t = db.Ticket.Find(id);
            db.Ticket.Remove(t);
            db.SaveChanges();
            return Redirect("Tickets");
        }
    }
}
