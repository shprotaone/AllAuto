using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Response;
using AllAuto.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AllAuto.Service.Implementations
{
    public class OrderService : IOrderService 
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<ItemEntry> _itemEntryRepository;

        public OrderService(IBaseRepository<User> userRepository, IBaseRepository<ItemEntry> orderRepository)
        {
            _userRepository = userRepository;
            _itemEntryRepository = orderRepository;
        }

        public async Task<BaseResponse<ItemEntry>> AddEntry(long basketId, long partId, int amountCount)
        {
            try
            {
                var entry = await _itemEntryRepository.GetAll()
                    .Where(x => x.BasketId == basketId && x.SparePartId == partId).FirstOrDefaultAsync();

                entry.Quantity += amountCount;

                await _itemEntryRepository.Update(entry);

                return new BaseResponse<ItemEntry>()
                {
                    Description = "Заказ добавлен",
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ItemEntry>()
                {
                    Description = ex.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<ItemEntry>> Create(ItemEntry model)
        {
            try
            {
                var user = await _userRepository.GetAll()
                    .Include(x => x.Basket)
                    .FirstOrDefaultAsync(x => x.Basket.Id == model.Basket.Id);

                if(user == null)
                { 
                    return new BaseResponse<ItemEntry>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = Domain.Enum.StatusCode.UserNotFound
                    };
                }

                model.BasketId = user.Basket.Id;

                await _itemEntryRepository.Create(model);

                return new BaseResponse<ItemEntry>()
                {
                    Description = "Заказ создан",
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ItemEntry>()
                {
                    Description = ex.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> Delete(long id)
        {
            try
            {
                var order = _itemEntryRepository.GetAll()
                    .Include(x => x.Basket)
                    .FirstOrDefault(x => x.Id == id);

                if(order == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = Domain.Enum.StatusCode.OrderNotFound,
                        Description = "Заказ не найден"
                    };
                }

                await _itemEntryRepository.Delete(order);

                return new BaseResponse<bool>()
                {
                    StatusCode = Domain.Enum.StatusCode.OK,
                    Description = "Заказ удален"
                };
            }
            catch (Exception e)
            {
                return new BaseResponse<bool>()
                {
                    Description = e.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<bool> FindEntryInBasket(long basketId, long partId)
        {
            var entry = await _itemEntryRepository.GetAll()
                    .Where(x => x.BasketId == basketId && x.SparePartId == partId).FirstOrDefaultAsync();

            if (entry == null) 
                return false;

            else return true;
        }
    }
}
