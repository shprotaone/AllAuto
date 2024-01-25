using AllAuto.Domain.Entity;
using AllAuto.Domain.ViewModels.Basket;
using AllAuto.Service.Implementations;
using AllAuto.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AllAuto.Controllers
{
    public class BasketController  : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IUserService _userService;

        public BasketController(IBasketService basketService,IUserService userService)
        {
            _basketService = basketService;
            _userService = userService; 
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

        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            var items = await _basketService.GetItems(User.Identity.Name);
            var user = await _userService.GetUser(User.Identity.Name);
            
            var basketView = new BasketViewModel()
            {
                Items = items.Data.ToList(),
                UserId = user.Data.Id
            };

            return View(basketView);
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

        [HttpPost]
        public JsonResult GetDeliveryTypes()
        {
            var types = _basketService.GetDeliveryTypes();
            return Json(types);
        }
    }
}
