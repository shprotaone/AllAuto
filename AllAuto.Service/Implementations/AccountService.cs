using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Helpers;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Account;
using AllAuto.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AllAuto.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Profile> _profileRepository;
        private readonly IBaseRepository<Basket> _basketRepository;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IBaseRepository<User> userRepository,
            ILogger<AccountService> logger,IBaseRepository<Profile> profileRepository, 
            IBaseRepository<Basket> basketRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _profileRepository = profileRepository;
            _basketRepository = basketRepository;
        }

        public async Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model)
        {
            try
            {
                var user = _userRepository.GetAll().FirstOrDefault(x => x.Name == model.Name);
                
                if (user != null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь с таким именем уже существует",
                    };
                }

                user = await CreateUser(model);
                await CreateProfile(user);
                await CreateBasket(user);

                ClaimsIdentity result = Authenticate(user);
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    Description = "Пользователь добавлен",
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Register]: {ex.Message}");
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        private async Task CreateBasket(User? user)
        {
            Basket basket = new Basket
            {
                UserId = user.Id

            };
            await _basketRepository.Create(basket);
        }

        private async Task CreateProfile(User? user)
        {
            var profile = new Profile()
            {
                UserId = user.Id
            };

            await _profileRepository.Create(profile);
        }

        private async Task<User?> CreateUser(RegisterViewModel model)
        {
            User? user = new User()
            {
                Name = model.Name,
                Role = Domain.Enum.Role.User,
                Password = HashPasswordHelper.HasPassword(model.Password)
            };
            await _userRepository.Create(user);
            return user;
        }

        public async Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == model.Name);

                if  (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь не найден"
                    };
                }

                if (user.Password != HashPasswordHelper.HasPassword(model.Password))
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Неверный пароль"
                    };
                }

                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Login]: {ex.Message}");
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == model.UserName);
                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Пользователь не найден"
                    };
                }

                user.Password = HashPasswordHelper.HasPassword(model.NewPassword);
                await _userRepository.Update(user);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = Domain.Enum.StatusCode.OK,
                    Description = "Пароль обновлен"
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Login]: {ex.Message}");
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                };
            }
        }

        /// <summary>
        /// куки пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType,user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType,user.Role.ToString())
            };

            return new ClaimsIdentity(claims, "ApplicationCookie",  //тип подлинности
                ClaimsIdentity.DefaultNameClaimType,ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}
