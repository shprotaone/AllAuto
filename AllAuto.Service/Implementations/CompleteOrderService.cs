using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Enum;
using AllAuto.Domain.Extensions;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Order;
using AllAuto.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace AllAuto.Service.Implementations
{
    public class CompleteOrderService : ICompleteOrderService
    {
        private readonly IBaseRepository<CompleteOrder> _completeOrderRepository;
        private readonly IBasketService _basketService;
        private readonly ILogger<CompleteOrderService> _logger;

        public CompleteOrderService(IBaseRepository<CompleteOrder> completeOrderRepository,ILogger<CompleteOrderService> logger,
            IBasketService basketService)
        {
            _completeOrderRepository = completeOrderRepository;
            _basketService = basketService;
            _logger = logger;
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
    }
}
