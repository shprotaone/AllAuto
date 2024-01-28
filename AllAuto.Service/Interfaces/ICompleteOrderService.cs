using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Order;

namespace AllAuto.Service.Interfaces
{
    public interface ICompleteOrderService
    {
        public Task<BaseResponse<CompleteOrderViewModel>> CreateCompleteOrder(CompleteOrderViewModel model);
    }
}
