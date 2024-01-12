using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Account;
using System.Security.Claims;

namespace AllAuto.Service.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);
        Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);
        Task<BaseResponse<bool>> ChangePassword(ChangePasswordViewModel model);

    }
}
