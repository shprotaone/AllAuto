using AllAuto.Domain.Entity;
using AllAuto.Domain.Response;

namespace AllAuto.Service.Interfaces
{
    public interface IOrderService
    {
        Task<BaseResponse<ItemEntry>> Create(ItemEntry model);

        Task<BaseResponse<bool>> Delete(long id);
    }
}
