using AllAuto.Domain.ViewModels.Order;
using AllAuto.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AllAuto.Controllers
{
    public class CompleteOrderController : Controller
    {
        private readonly ICompleteOrderService _completeOrderService;
        private readonly IBasketService _basketService;

        public CompleteOrderController(ICompleteOrderService completeOrderService, IBasketService basketService)
        {
            _completeOrderService = completeOrderService;
            _basketService = basketService;
        }

        [HttpPost]
        public async Task<IActionResult> SetOrderComplete(CompleteOrderViewModel model)
        {
            ModelState.Remove("Id");
            ModelState.Remove("UserId");
            ModelState.Remove("User");
            ModelState.Remove("ItemEntries");

            if (ModelState.IsValid)
            {
                await _completeOrderService.CreateCompleteOrder(model);
                await _basketService.ClearBasket(model.UserId);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
