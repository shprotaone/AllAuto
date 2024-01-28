using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Enum;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Order;
using AllAuto.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace AllAuto.Service.Implementations
{
    public class CompleteOrderService : ICompleteOrderService
    {
        private readonly IBaseRepository<CompleteOrder> _completeOrderRepository;
        private readonly ILogger<CompleteOrderService> _logger;

        public CompleteOrderService(IBaseRepository<CompleteOrder> completeOrderRepository,ILogger<CompleteOrderService> logger)
        {
            _completeOrderRepository = completeOrderRepository;
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
    }
}
