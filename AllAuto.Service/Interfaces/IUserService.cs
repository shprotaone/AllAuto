using AllAuto.Domain.Entity;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.User;

namespace AllAuto.Service.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<IEnumerable<UserViewModel>>> GetUsers();
        
        Task<BaseResponse<User>> GetUser(string name);

        Task<BaseResponse<User>> Create(UserViewModel model);

        BaseResponse<Dictionary<int,string>> GetRoles();

        Task<BaseResponse<bool>> DeleteUser(long id);
        
    }
}
