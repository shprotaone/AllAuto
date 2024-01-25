using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Response;
using AllAuto.Service.Interfaces;
using IronXL.Styles;
using Microsoft.EntityFrameworkCore;

namespace AllAuto.Service.Implementations
{
    public class OrderService : IOrderService 
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<ItemEntry> _orderRepository;

        public OrderService(IBaseRepository<User> userRepository, IBaseRepository<ItemEntry> orderRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
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

                await _orderRepository.Create(model);

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
                var order = _orderRepository.GetAll()
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

                await _orderRepository.Delete(order);

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
    }
}
