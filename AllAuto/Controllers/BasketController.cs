using AllAuto.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AllAuto.Controllers
{
    public class BasketController  : Controller
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<IActionResult> Detail()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");    
            }

            var response = await _basketService.GetItems(User.Identity.Name);
            if(response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> AddItem(long id, int Amount)
        {
            var response = await _basketService.AddItem(User.Identity.Name, id,Amount);
            
            if(response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet]
        public async Task<IActionResult> GetItem(long id)
        {
            var response = await _basketService.GetItem(User.Identity.Name, id);
            if(response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return PartialView(response.Data);
            }

            return RedirectToAction("Index", "Home");

        }
    }
}
