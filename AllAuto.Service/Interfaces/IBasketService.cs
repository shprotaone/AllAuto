using AllAuto.Domain.Entity;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Order;

namespace AllAuto.Service.Interfaces
{
    public interface IBasketService
    {
        Task<BaseResponse<OrderViewModel>> GetItem(string? userName, long id);

        Task<BaseResponse<IEnumerable<OrderViewModel>>> GetItems(string userName);
        Task<BaseResponse<OrderViewModel>> AddItem(string userName,long id, int Amount);

    }
}
