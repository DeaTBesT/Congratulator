using Congratulator.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Congratulator.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBManager dbManager = new DBManager();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            //dbManager = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            DateTime nearerDate = DateTime.Now.AddDays(7);

            HomeViewModel model = new HomeViewModel(
                dbManager.GetPeopleByCurrentDate(),
                dbManager.GetPeopleByNerarerDate(nearerDate)
                );

            return View(model);
        }

        public IActionResult LessBirthdays()
        {
            return View(dbManager.GetPeopleByLessDateFromToday());
        }

        public IActionResult ListBirthdays()
        {
            return View(dbManager.GetPeopleSortByDate(true));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}