using AllAuto.Domain.Entity;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Order;

namespace AllAuto.Service.Interfaces
{
    public interface ICompleteOrderService
    {
        Task<BaseResponse<CompleteOrderViewModel>> CreateCompleteOrder(CompleteOrderViewModel model);
        BaseResponse<List<CompleteOrderView>> GetCompleteOrders(long id);
        Task<string> GetLastCompleteOrder(int name);
    }
}
