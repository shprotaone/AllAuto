using AllAuto.Domain.Entity;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Profile;

namespace AllAuto.Service.Interfaces
{
    public interface IProfileService
    {
        Task<BaseResponse<ProfileViewModel>> GetProfile(string username);
        Task<BaseResponse<Profile>> Save(ProfileViewModel model);
    }
}
