using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Order;
using AllAuto.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AllAuto.Service.Implementations
{
    public class OrderService : IOrderService 
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Order> _orderRepository;

        public OrderService(IBaseRepository<User> userRepository, IBaseRepository<Order> orderRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }

        public async Task<BaseResponse<Order>> Create(CreateOrderViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll()
                    .Include(x => x.Basket)
                    .FirstOrDefaultAsync(x => x.Name == model.LoginId);

                if(user == null)
                {
                    return new BaseResponse<Order>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = Domain.Enum.StatusCode.UserNotFound
                    };
                }

                Order order = user.Basket.Orders.FirstOrDefault(x => x.IsClosed == false);
                if(order == default)
                {
                    order = new Order()
                    {
                        DateCreated = DateTime.Now,
                        BasketId = user.Basket.Id,
                        PartList = new List<SparePart>(),
                        Address = user.Profile.Address
                    };
                }
               

                await _orderRepository.Create(order);

                return new BaseResponse<Order>()
                {
                    Description = "Заказ создан",
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Order>()
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
