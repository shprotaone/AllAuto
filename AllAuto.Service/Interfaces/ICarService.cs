using AllAuto.Domain.Entity;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Car;

namespace AllAuto.Service.Interfaces
{
    public interface ICarService
    {
        Task<BaseResponse<IEnumerable<Car>>> GetCars();

        Task<BaseResponse<Car>> GetCar(int id);

        Task<BaseResponse<Car>> GetCarByName(string name);

        Task<IBaseResponse<CarViewModel>> CreateCar(CarViewModel carViewModel, byte[] imageData);

        Task<IBaseResponse<bool>> DeleteCar(int id);

        Task<IBaseResponse<Car>> Edit(int id,CarViewModel model);

        BaseResponse<Dictionary<int, string>> GetTypes();
    }
}
