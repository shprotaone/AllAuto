using AllAuto.Domain.Entity;
using AllAuto.Domain.Response;
using AllAuto.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AllAuto.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        /// <summary>
        /// добавление в корзину
        /// </summary>
        /// <param name="partId"></param>
        /// <param name="amountCount"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AddEntryItem(long partId, int amountCount)
        {
            BaseResponse<ItemEntry> response;
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userService.GetUser(User.Identity.Name);
            bool inBasket = await _orderService.FindEntryInBasket(user.Data.Basket.Id, partId);

            if (inBasket)
            {
                response = await _orderService.AddEntry(user.Data.Basket.Id, partId, amountCount);
            }
            else
            {
                var itemEntryModel = new ItemEntry()
                {
                    SparePartId = Convert.ToInt32(partId),
                    Basket = user.Data.Basket,
                    Quantity = amountCount,

                };

                response = await _orderService.Create(itemEntryModel);
            }
         
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
                return RedirectToAction("Detail","Basket");
            }

            return View("Error", $"{response.Description}");
        }
    }
}
