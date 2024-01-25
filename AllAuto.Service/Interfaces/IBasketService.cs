using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Order;

namespace AllAuto.Service.Interfaces
{
    public interface IBasketService
    {
        Task<BaseResponse<ItemEntryViewModel>> GetItem(string? userName, long id);

        Task<BaseResponse<IEnumerable<ItemEntryViewModel>>> GetItems(string userName);

        Task<BaseResponse<ItemEntryViewModel>> AddItem(string userName,long id, int Amount);
        
        BaseResponse <Dictionary<int,string>> GetDeliveryTypes();
    }
}
