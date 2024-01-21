using AllAuto.Domain.ViewModels.Order;
using AllAuto.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AllAuto.Controllers
{
    public class OrderController : Controller
    {
        private readonly  IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrder(long partId, int amountCount)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var orderModel = new CreateOrderViewModel()
            {
                SparePartIdToAdd = Convert.ToInt32(partId),
                LoginId = User.Identity.Name,
                Quantity = amountCount,
            };           

            var response = await _orderService.Create(orderModel);
            if(response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Json(new { description = response.Description });
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _orderService.Delete(id);
            if(response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("Index","Home");
            }

            return View("Error", $"{response.Description}");
        }
    }
}
