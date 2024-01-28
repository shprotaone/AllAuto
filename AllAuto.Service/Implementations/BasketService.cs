using AllAuto.DAL.Interfaces;
using AllAuto.DAL.Repositories;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Enum;
using AllAuto.Domain.Extensions;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Order;
using AllAuto.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AllAuto.Service.Implementations
{
    public class BasketService : IBasketService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<SparePart> _sparePartRepository;
        private readonly IBaseRepository<ItemEntry> _itemEntryRepository;
        private readonly IBaseRepository<Basket> _basketRepository;

        public BasketService(IBaseRepository<User> userRepository, 
            IBaseRepository<SparePart> sparePartRepository,
            IBaseRepository<ItemEntry> itemEntryRepository,
            IBaseRepository<Basket> basketRepository)
        {
            _userRepository = userRepository;
            _sparePartRepository = sparePartRepository;
            _itemEntryRepository = itemEntryRepository;
            _basketRepository = basketRepository;
        }


        public async Task<BaseResponse<bool>> ClearBasket(long userId)
        {
            try
            {
                User? user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = Domain.Enum.StatusCode.UserNotFound
                    };
                }

                Basket? basket = await _basketRepository.GetAll().FirstOrDefaultAsync(x => x.UserId == userId);
                var entries = _itemEntryRepository.GetAll().Where(x => x.BasketId == basket.Id).ToList();

                foreach (var entry in entries)
                {
                    await _itemEntryRepository.Delete(entry);
                }

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<ItemEntryViewModel>>> GetItems(string name)
        {
            try
            {
               User user = FindUser(name).Result;

                if (user == null)
                {
                    return new BaseResponse<IEnumerable<ItemEntryViewModel>>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = Domain.Enum.StatusCode.UserNotFound
                    };
                }

                IEnumerable <ItemEntryViewModel> entries = GetEntries(user);

                return new BaseResponse<IEnumerable<ItemEntryViewModel>>()
                {
                    Data = entries,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<ItemEntryViewModel>>()
                {
                    Description = ex.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        private IEnumerable<ItemEntryViewModel> GetEntries(User user)
        {
            List<ItemEntryViewModel> entries = new List<ItemEntryViewModel>();

            foreach (var item in user.Basket.ItemEntries)
            {
                item.SparePart = _sparePartRepository.GetAll()
                    .FirstOrDefault(x => x.Id == item.SparePartId);

                entries.Add(new ItemEntryViewModel
                {
                    Id = item.Id,
                    Name = item.SparePart.Name,
                    Manufactor = item.SparePart.Model,
                    DateCreate = item.DateCreated.ToShortDateString(),
                    Quantity = item.Quantity,
                    Sum = item.Quantity * item.SparePart.Price
                }) ;
            }

            return entries;
        }

        public async Task<BaseResponse<ItemEntryViewModel>> GetItem(string? name, long id)
        {
            try
            {
                var user = await _userRepository.GetAll()
                    .Include(x => x.Basket) // ленивая загрузка, Lazy load
                    .ThenInclude(x => x.ItemEntries)
                    .FirstOrDefaultAsync(x => x.Name == name);

                if (user == null)
                {
                    return new BaseResponse<ItemEntryViewModel>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = Domain.Enum.StatusCode.UserNotFound
                    };
                }

                var orders = user.Basket?.ItemEntries.Where(x => x.Id == id).ToList();
                if (orders == null || orders.Count == 0)
                {
                    return new BaseResponse<ItemEntryViewModel>()
                    {
                        Description = "Заказов нет",
                        StatusCode = Domain.Enum.StatusCode.OrderNotFound
                    };
                }

                return new BaseResponse<ItemEntryViewModel>()
                {
                    Data = new ItemEntryViewModel(),        //заглушка
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ItemEntryViewModel>()
                {
                    Description = ex.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<ItemEntryViewModel>> AddItem(string userName, long id, int Amount)
        {
            try
            {
                //найти юзера
                User user = FindUser(userName).Result;

                if (user == null)
                {
                    return new BaseResponse<ItemEntryViewModel>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = Domain.Enum.StatusCode.UserNotFound
                    };
                }

                //взять экземпляр корзины
                Basket basket = FindBasket(user).Result;
                ItemEntry order = FindOrder(user);

            }
            catch (Exception ex)
            {
                return new BaseResponse<ItemEntryViewModel>()
                {
                    Description = ex.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }

            return new BaseResponse<ItemEntryViewModel>()
            {
                Description = "Заглушка",
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }

        public BaseResponse<Dictionary<int, string>> GetDeliveryTypes()
        {
            try
            {
                var types = ((DeliveryType[])Enum.GetValues(typeof(DeliveryType)))
                    .ToDictionary(key => (int)key, type => type.GetDisplayName());

                return new BaseResponse<Dictionary<int, string>>
                {
                    Data = types,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<int, string>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private async Task<User> FindUser(string name)
        {
            User? user = await _userRepository.GetAll()
                   .Include(x => x.Basket)
                   .ThenInclude(x => x.ItemEntries)
                   .FirstOrDefaultAsync(x => x.Name == name);

            return user;
        }

        private ItemEntry FindOrder(User user)
        {
            ItemEntry? order = user.Basket?.ItemEntries.FirstOrDefault();
            if (order == null)
            {
                order = new ItemEntry()
                {
                    DateCreated = DateTime.Now,
                };
            }

            return order;
        }

        private async Task<Basket> FindBasket(User user)
        {
            return user.Basket;
        }

    }
}
