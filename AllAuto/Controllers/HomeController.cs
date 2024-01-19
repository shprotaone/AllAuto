using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using AllAuto.Models;
using AllAuto.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AllAuto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IExcelReaderService<SparePart> _excelReaderService;

        public HomeController(ILogger<HomeController> logger,
            IBaseRepository<SparePart> carRepository,
            IExcelReaderService<SparePart> excelReaderService)
        {
            _logger = logger;
            _excelReaderService = excelReaderService;
        }

        public async Task<IActionResult> Index()
        {
            //_excelReaderService.ReadExcelFile();
            return View();
        }

        public IActionResult About()
        {
            return View("../About/AboutView");
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