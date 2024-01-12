using AllAuto.Domain.Entity;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Order;

namespace AllAuto.Service.Interfaces
{
    public interface IOrderService
    {
        Task<BaseResponse<Order>> Create(CreateOrderViewModel model);

        Task<BaseResponse<bool>> Delete(long id);
    }
}
