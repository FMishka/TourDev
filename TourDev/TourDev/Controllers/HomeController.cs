using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using TourDev.Hubs;
using TourDev.Models;


namespace TourDev.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            db = context;
        }
        private List<string> GetPurpose()
        {
            var purpose = db.Tourist.Select(m => m.Visit_purpose).Distinct().ToList();
            return purpose;
        }
        private List<string> GetCountry()
        {
            var c = db.Ticket.Select(c => c.Country).Distinct().ToList();
            return c;
        }
        private List<SelectListItem> GetSelectListItems(List<string> massOfItmes)
        {
            var list = new List<SelectListItem>();
            foreach (var item in massOfItmes)
            {
                list.Add(new SelectListItem { Text = item, Value = item });
            }
            return list;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Hotels()
        {
            var hotel = db.Hotel.ToList();
            return View(hotel);
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Tourists()
        {
            var tourists = db.Tourist
                .Include(h => h.Hotel)
                .ToListAsync();
            return View(await tourists);
        }
        public async Task<IActionResult> TouristList(string searchPurpose)
        {
            ViewBag.Purposes = GetSelectListItems(GetPurpose());
            var tourists = db.Tourist
                .Include(h => h.Hotel)
                .Where(v => EF.Functions.Like(v.Visit_purpose, $"%{searchPurpose}%"))
                .ToListAsync();
            return View(await tourists);
        }
        public async Task<IActionResult> Resettlement(string searchPurpose)
        {
            ViewBag.Purposes = GetSelectListItems(GetPurpose());
            var tourists = db.Tourist.Include(h => h.Hotel)
                .Where(v => EF.Functions.Like(v.Visit_purpose, $"%{searchPurpose}%"));
            return View(await tourists.ToListAsync());
        }
        public IActionResult TouristsInCountry(string purpose, DateTime? startDate, DateTime? endDate)
        {
            ViewBag.Purpose = GetSelectListItems(GetPurpose());
            ViewBag.tt = db.Ticket.GroupBy(c => c.Country).Select(g => new { Name = g.Key, Count = g.Count() });
            ViewBag.Country = GetSelectListItems(db.Ticket.Select(co => co.Country).Distinct().ToList());
            if ((startDate != null) && (endDate != null)) //Если все поля заполнены
            {
                var tickets = db.Ticket.Where(v => EF.Functions
                .Like(v.Tourist.Visit_purpose, $"%{purpose}%") && v.Arrival_date >= startDate && v.Departure_date <= endDate)
                                    .GroupBy(c => c.Country)
                                    .Select(g => new { Name = g.Key, Count = g.Count() }); ViewBag.tt = tickets;
                ViewBag.tt = tickets;
            }
            else if (startDate != null) //Если заполнено только начало
            {
                var tickets = db.Ticket.Where(v => EF.Functions.Like(v.Tourist.Visit_purpose, $"%{purpose}%") && v.Arrival_date >= startDate)
                    .GroupBy(c => c.Country)
                    .Select(g => new { Name = g.Key, Count = g.Count() });
                ViewBag.tt = tickets;
            }
            else if (endDate != null) //Если заполнен только конец
            {
                var tickets = db.Ticket.Where(v => EF.Functions.Like(v.Tourist.Visit_purpose, $"%{purpose}%") && v.Departure_date <= endDate)
                    .GroupBy(c => c.Country)
                    .Select(g => new { Name = g.Key, Count = g.Count() });
                ViewBag.tt = tickets;
            }
            else //Если заполненена только цель
            {
                var tickets = db.Ticket.Where(v => EF.Functions.Like(v.Tourist.Visit_purpose, $"%{purpose}%"))
                    .GroupBy(c => c.Country)
                    .Select(g => new { Name = g.Key, Count = g.Count() });
                ViewBag.tt = tickets;
            }
            return View();
        }
        public IActionResult AboutTourist(string country, int currentTourist)
        {
            var t = db.Tourist.Select(t => new { t.Id, t.Initials });
            ViewBag.Tourists = new SelectList(t, "Id", "Initials");
            ViewBag.Country = GetSelectListItems(GetCountry());
            var tourist = db.Ticket.Where(t => t.TouristId == currentTourist).Include(t => t.Tourist).Include(h => h.Hotel).ToList();
            var gr = db.Group.Where(e => e.TouristId == currentTourist).Include(e => e.Excursion).ToList();
            var l = new List<string>();
            foreach (var item in gr)
            {
                l = db.Excursion.Where(e => e.GroupId == item.Id).Select(r => r.Excursions).ToList();
            }
            ViewBag.Ex = GetSelectListItems(l);
            return View(tourist);
        }
        public async Task<IActionResult> HotelsForTourists(DateTime? startDate, DateTime? endDate)
        {
            var tickets = db.Ticket
                .Include(h => h.Hotel).Include(t => t.Tourist).AsQueryable();
            if (startDate != null && endDate != null)
            {
                tickets = tickets.Where(t => t.Arrival_date >= startDate && t.Departure_date <= endDate);
            }
            else if (startDate != null)
            {
                tickets = tickets.Where(t => t.Arrival_date >= startDate);
            }
            else if (endDate != null)
            {
                tickets = tickets.Where(t => t.Departure_date <= endDate);
            }
            return View(await tickets.ToListAsync());
        }
        public IActionResult CountTourists(DateTime? startDate, DateTime? endDate)
        {
            ViewBag.CountTourists = null;
            if ((startDate != null) && (endDate != null)) //Если все поля заполнены
            {
                var exx = db.Group.Include(e => e.Excursion)
                    .Where(v => v.Excursion.Time >= startDate && v.Excursion.Time <= endDate).Select(g => g.TouristId);
                ViewBag.CountTourists = exx.Count();
            }
            else if (startDate != null) //Если заполнено только начало
            {
                var exx = db.Group.Include(e => e.Excursion).Where(v => v.Excursion.Time >= startDate).Select(g => g.TouristId);
                ViewBag.CountTourists = exx.Count();
            }
            else if (endDate != null) //Ксли заполнен только конец
            {
                var exx = db.Group.Include(e => e.Excursion).Where(v => v.Excursion.Time <= endDate).Select(g => g.TouristId);
                ViewBag.CountTourists = exx.Count();
            }
            else
            {
                var exx = db.Group.Include(e => e.Excursion).Select(g => g.TouristId);
                ViewBag.CountTourists = exx.Count();
            }
            return View();
        }
        public IActionResult PopularExcursions(int agentId)
        {
            var agents = db.Agent.Select(a => new { id = a.Id, name = a.Name }).ToList();
            ViewBag.agents = new SelectList(agents, "id", "name");
            var ex = db.Excursion.Include(g => g.Group)
                .Where(g => g.AgentId == agentId)
                .GroupBy(t => t.Excursions)
                .Select(g => new { name = g.Key, count = g.Count() })
                .OrderByDescending(p => p.count);
            ViewBag.ex = ex;
            return View();
        }
        public IActionResult AboutFlight(int fly)
        {
            var flies = db.Flight.Select(a => new { id = a.Id, name = a.Name }).ToList();
            ViewBag.flies = new SelectList(flies, "id", "name");
            var flight = db.Ticket.Include(b => b.Bag).Include(f => f.Flight).Where(g => g.FlightId == fly);
            ViewBag.weight = flight.Sum(t => t.Bag.Weight);
            ViewBag.count = flight.Count();
            ViewBag.place = flight.Sum(t => t.Bag.Count_of_place);
            return View();
        }
        public IActionResult Warehouse(DateTime? startDate, DateTime? endDate)
        {
            var tickets = db.Ticket.Include(b => b.Bag).Include(f => f.Flight).AsQueryable();
            if (startDate != null && endDate != null)
            {
                tickets = db.Ticket.Include(b => b.Bag).Include(f => f.Flight)
                    .Where(t => t.Arrival_date >= startDate && t.Departure_date <= endDate);
            }
            else if (startDate != null)
            {
                tickets = db.Ticket.Include(b => b.Bag)
                    .Include(f => f.Flight)
                    .Where(t => t.Arrival_date >= startDate);
            }
            else if (endDate != null)
            {
                tickets = db.Ticket.Include(b => b.Bag)
                    .Include(f => f.Flight)
                    .Where(t => t.Departure_date <= endDate);
            }
            ViewBag.count = tickets.Sum(b => b.Bag.Count_of_place);
            ViewBag.weight = tickets.Sum(t => t.Bag.Weight);
            ViewBag.countFly = tickets.Count();
            ViewBag.warehouse = tickets.Where(w => w.Flight.Type == "Грузовой").Count();
            ViewBag.pass = tickets.Where(p => p.Flight.Type == "Пассажирский").Count();
            return View();
        }
        public IActionResult GroupFinance(string searchPurpose)
        {
            ViewBag.Purposes = GetSelectListItems(GetPurpose());
            var tickets = db.Ticket.Include(b => b.Bag).Include(t => t.Tour).Include(to => to.Tourist).AsQueryable();
            if (searchPurpose != null)
            {
                tickets = tickets.Where(v => EF.Functions.Like(v.Tourist.Visit_purpose, $"%{searchPurpose}%"));
            }
            ViewBag.costTickets = tickets.Sum(t => t.Cost);
            ViewBag.costTour = tickets.Sum(t => t.Tour.Cost);
            ViewBag.costBag = tickets.Sum(b => b.Bag.Cost_for_bag_group.Cost);
            ViewBag.transpCost = tickets.Sum(b => b.Bag.Cost_for_bag_group.Transportation_cost);
            ViewBag.loadingCost = tickets.Sum(b => b.Bag.Cost_for_bag_group.Loading_cost);
            ViewBag.unloadingCost = tickets.Sum(b => b.Bag.Cost_for_bag_group.Unloading_cost);
            return View();
        }
        public IActionResult AboutFinance(DateTime? startDate, DateTime? endDate)
        {

            var tickets = db.Ticket.Include(b => b.Bag)
                .Include(t => t.Tour)
                .Include(to => to.Tourist)
                .Include(h => h.Hotel)
                .Include(f => f.Flight)
                .Include(g => g.Group)
                .AsQueryable();
            if (startDate != null && endDate != null)
            {
                tickets = tickets.Where(t => t.Arrival_date >= startDate && t.Departure_date <= endDate);
            }
            else if (startDate != null)
            {
                tickets = tickets.Where(t => t.Arrival_date >= startDate);
            }
            else if (endDate != null)
            {
                tickets = tickets.Where(t => t.Departure_date <= endDate);
            }
            ViewBag.costTickets = tickets.Sum(t => t.Cost);
            ViewBag.costTour = tickets.Sum(t => t.Tour.Cost);
            ViewBag.costBag = tickets.Sum(b => b.Bag.Cost_for_bag_group.Cost);
            ViewBag.transpCost = tickets.Sum(b => b.Bag.Cost_for_bag_group.Transportation_cost);
            ViewBag.loadingCost = tickets.Sum(b => b.Bag.Cost_for_bag_group.Loading_cost);
            ViewBag.unloadingCost = tickets.Sum(b => b.Bag.Cost_for_bag_group.Unloading_cost);
            ViewBag.costHotel = tickets.Sum(h => h.Hotel.Cost);
            ViewBag.costFly = tickets.Sum(f => f.Flight.Cost);
            ViewBag.excusrsionCost = tickets.Sum(ex => ex.Group.Excursion.Cost);
            return View();
        }
        public IActionResult TypeBag(string type)
        {
            var bag = db.Bag.Include(p => p.Cost_for_bag_group).AsQueryable();
            if (type != null)
            {
                bag = bag.Where(p => p.Type == type);
            }
            ViewBag.type = GetSelectListItems(db.Bag.Select(p => p.Type).Distinct().ToList());
            ViewBag.countPlace = bag.Sum(c => c.Count_of_place);
            ViewBag.count = bag.Count();
            ViewBag.weight = bag.Sum(c => c.Weight);
            ViewBag.cost = bag.Sum(p => p.Cost_for_bag_group.Loading_cost)
                + bag.Sum(p => p.Cost_for_bag_group.Unloading_cost)
                + bag.Sum(p => p.Cost_for_bag_group.Transportation_cost);
            ViewBag.pay = bag.Sum(p => p.Cost_for_bag_group.Cost);
            ViewBag.udel = Math.Round((double)bag.Count() / (double)db.Bag.Count() * 100);
            return View();
        }
        public IActionResult Profit()
        {
            ViewBag.profit = db.Ticket.Sum(c => c.Cost)// доходы
                + db.Tour.Sum(c => c.Cost)
                + db.Cost_for_bag_group.Sum(c => c.Cost)
                - db.Excursion.Sum(c => c.Cost)// расходы
                - db.Hotel.Sum(c => c.Cost)
                - db.Flight.Sum(c => c.Cost)
                - db.Cost_for_bag_group.Sum(c => c.Transportation_cost + c.Loading_cost + c.Unloading_cost);
            return View();
        }
        public IActionResult TouristPr(DateTime? startDate, DateTime? endDate)
        {
            var tickets = db.Ticket.Include(t => t.Tourist).Where(t => t.Tourist.Visit_purpose == "Отдых");
            var period = db.Ticket.Include(t => t.Tourist).AsQueryable();
            ViewBag.pr = "";
            if (startDate != null && endDate != null)
            {
                tickets = tickets.Where(t => t.Arrival_date >= startDate && t.Departure_date <= endDate);
                period = period.Where(t => t.Arrival_date >= startDate && t.Departure_date <= endDate);
            }
            else if (startDate != null)
            {
                tickets = tickets.Where(t => t.Arrival_date >= startDate);
                period = period.Where(t => t.Arrival_date >= startDate);
            }
            else if (endDate != null)
            {
                tickets = tickets.Where(t => t.Departure_date <= endDate);
                period = period.Where(t => t.Departure_date <= endDate);
            }
            if (period.Count() == 0)
            {
                ViewBag.pr = "Нет туристов в этом диапазоне";
            }
            else
            {
                ViewBag.pr = Math.Round((double)tickets.Count() / (double)period.Count() * 100);
            }
            return View();
        }
        public IActionResult TouristsFly(int? fly)
        {
            var flies = db.Flight.Select(a => new { id = a.Id, name = a.Name }).ToList();
            ViewBag.flies = new SelectList(flies, "id", "name");
            var t = db.Ticket.Include(h => h.Hotel).Include(t => t.Tourist).Include(b => b.Bag).AsQueryable();
            if (fly != null)
            {
                t = t.Where(t => t.FlightId == fly);
            }
            return View(t.ToList());
        }
        public IActionResult MyLayout()
        {
            return View();
        }
    }
}