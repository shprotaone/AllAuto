using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Profile;
using AllAuto.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AllAuto.Service.Implementations
{
    public class ProfileService : IProfileService
    {

        private readonly IBaseRepository<Profile> _profileRepository;
        private readonly ILogger<ProfileService> _logger;

        public ProfileService(IBaseRepository<Profile> profileRepository,ILogger<ProfileService> logger)
        {
            _profileRepository = profileRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<ProfileViewModel>> GetProfile(string username)
        {
            try
            {
                var profile = await _profileRepository.GetAll()
                    .Select(x => new ProfileViewModel()
                    {
                        Id = x.Id,
                        Address = x.Address,
                        Age = x.Age,
                        UserName = x.User.Name
                    })
                    .FirstOrDefaultAsync(x => x.UserName == username);


                return new BaseResponse<ProfileViewModel>()
                {
                    Data = profile,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[UserService.GetUsers] error {ex.Message}");
                return new BaseResponse<ProfileViewModel>()
                {
                    StatusCode = Domain.Enum.StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<Profile>> Save(ProfileViewModel model)
        {
            try
            {
                var profile = await _profileRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                profile.Address = model.Address;
                profile.Age = model.Age;
                await _profileRepository.Update(profile);

                return new BaseResponse<Profile>()
                {
                    Data = profile,
                    StatusCode = Domain.Enum.StatusCode.OK,
                    Description = "Данные сохранены"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[UserService.GetUsers] error {ex.Message}");
                return new BaseResponse<Profile>()
                {
                    StatusCode = Domain.Enum.StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }
    }
}
