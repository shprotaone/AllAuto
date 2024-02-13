using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Enum;
using AllAuto.Domain.Extensions;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Order;
using AllAuto.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace AllAuto.Service.Implementations
{
    public class CompleteOrderService : ICompleteOrderService
    {
        private readonly IBaseRepository<CompleteOrder> _completeOrderRepository;
        private readonly IBasketService _basketService;

        public CompleteOrderService(IBaseRepository<CompleteOrder> completeOrderRepository,
            IBasketService basketService)
        {
            _completeOrderRepository = completeOrderRepository;
            _basketService = basketService;
        }

        public async Task<BaseResponse<CompleteOrderViewModel>> CreateCompleteOrder(CompleteOrderViewModel model)
        {
            var response = new BaseResponse<CompleteOrderViewModel>();

            try
            {
                CompleteOrder completeOrder = new CompleteOrder()
                {
                    UserId = model.UserId,
                    FullName = model.FullName,
                    Address = model.Address,
                    DateTime = model.DateTime,
                    DeliveryType = (DeliveryType)Convert.ToInt32(model.DeliveryType),
                    Sum = _basketService.GetItems(model.UserId).Result.Data.Sum(x => x.Sum)
                };

                await _completeOrderRepository.Create(completeOrder);
            }
            catch (Exception ex)
            {
                return new BaseResponse<CompleteOrderViewModel>()
                {
                    Description = $"[CreateCompleteOrder] : {ex.Message}"
                };
            }

            return response;
        }

        public BaseResponse<List<CompleteOrderView>> GetCompleteOrders(long id)
        {
            List<CompleteOrderView> orderViews= new List<CompleteOrderView>();

            try
            {
                var orders = _completeOrderRepository.GetAll().Where(x => x.UserId == id).ToList();

                foreach (var item in orders)
                {
                    orderViews.Add(new CompleteOrderView
                    {
                        Id = item.Id,
                        FullName = item.FullName,
                        Address = item.Address,
                        DateTime = item.DateTime,
                        DeliveryType = item.DeliveryType.GetDisplayName(),
                        Sum = item.Sum                       
                    });
                }

                return new BaseResponse<List<CompleteOrderView>>()
                {
                    Data = orderViews,
                    StatusCode = StatusCode.OK
                };
                                        
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<CompleteOrderView>>()
                {
                    Description = $"[CreateCompleteOrder] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<string> GetLastCompleteOrder(int count)
        {
            string result = "";
            try
            {
                var orders = await _completeOrderRepository.GetAll().ToListAsync();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < orders.Count; i++)
                {
                    if (i <= count)
                    {
                        
                        sb.AppendLine($"Заказчик: {orders[i].FullName}");
                        sb.AppendLine($"Адрес доставки: {orders[i].Address}");
                        sb.AppendLine($"Дата и время доставки : {orders[i].DateTime}");
                        sb.AppendLine($"Сумма: {orders[i].Sum}");
                        sb.AppendLine();
                        result = sb.ToString();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return $"[GetLastCompleteOrder] : {ex.Message}";
            }            
        }
    }
}
