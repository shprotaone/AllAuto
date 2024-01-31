using AllAuto.Domain.Entity;
using AllAuto.Domain.ViewModels.Order;
using AllAuto.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AllAuto.Controllers
{
    public class OrderController : Controller
    {
        private readonly  IOrderService _itemEntryService;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, IUserService userService)
        {
            _itemEntryService = orderService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> AddEntryItem(long partId, int amountCount)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userService.GetUser(User.Identity.Name);

            var itemEntryModel = new ItemEntry()
            {
                SparePartId = Convert.ToInt32(partId),
                Basket = user.Data.Basket,
                Quantity = amountCount,
            };           

            var response = await _itemEntryService.Create(itemEntryModel);
            if(response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Json(new { description = response.Description });
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _itemEntryService.Delete(id);
            if(response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("Detail","Basket");
            }

            return View("Error", $"{response.Description}");
        }
    }
}
