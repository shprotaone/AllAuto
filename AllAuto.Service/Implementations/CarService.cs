using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Enum;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Car;
using AllAuto.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AllAuto.Service.Implementations
{
    /// <summary>
    /// Прослойка между БД и View
    /// </summary>
    public class CarService : ICarService
    {
        private const string CarNotFoundMessage = "Car not found";
        private const string ZeroCarsMessage = "Найдено 0 элементов";

        private readonly IBaseRepository<Car> _carRepository;
        private readonly ILogger<CarService> _logger;

        public CarService(IBaseRepository<Car> carRepository,ILogger<CarService> logger)
        {
            _carRepository = carRepository;
            _logger = logger;
        }

        public async Task<IBaseResponse<CarViewModel>> CreateCar(CarViewModel carViewModel, byte[] image)
        {
            var baseResponse = new BaseResponse<CarViewModel>();

            try
            {
                var car = new Car()
                {
                    Description = carViewModel.Description,
                    DateCreate = carViewModel.DateCreate,
                    MaxSpeed = carViewModel.MaxSpeed,
                    Model = carViewModel.Model,
                    Price = carViewModel.Price,
                    Name = carViewModel.Name,
                    Avatar = image,
                    TypeCar = (TypeCar)Convert.ToInt32(carViewModel.TypeCar)
                };

                await _carRepository.Create(car);
            }
            catch (Exception ex)
            {
                return new BaseResponse<CarViewModel>()
                {
                    Description = $"[CreateCar] : {ex.Message}"
                };
            }

            return baseResponse;
        }

        public async Task<IBaseResponse<bool>> DeleteCar(int id)
        {
            var baseResponse = new BaseResponse<bool>();

            try
            {
                var car = await _carRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

                if (car == null)
                {
                    baseResponse.Description = "Car not found";
                    baseResponse.StatusCode = StatusCode.CarNotFound;

                    return baseResponse;
                }

                await _carRepository.Delete(car);
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[DeleteCar] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<Car>> Edit(int id, CarViewModel model)
        {
            var baseResponse = new BaseResponse<Car>();

            try
            {
                var car = await _carRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if(car == null)
                {
                    baseResponse.StatusCode = StatusCode.CarNotFound;
                    baseResponse.Description = CarNotFoundMessage;
                    return baseResponse;
                }

                car.Description = model.Description;
                car.Model = model.Model;
                car.Price = model.Price;
                car.MaxSpeed = model.MaxSpeed;
                car.DateCreate = model.DateCreate;
                car.Name = model.Name;

                await _carRepository.Update(car);

                return baseResponse;
                //TypeCar
            }
            catch (Exception ex)
            {
                return new BaseResponse<Car>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError       
                };
            }
        }

        public async Task<BaseResponse<Car>> GetCar(int id)
        {
            var baseResponse = new BaseResponse<Car>();

            try
            {
                var car = await _carRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                
                if(car == null)
                {
                    baseResponse.Description = CarNotFoundMessage;
                    baseResponse.StatusCode = StatusCode.CarNotFound;

                    return baseResponse;
                }

                baseResponse.Data = car;
                return baseResponse;


            }
            catch (Exception ex)
            {
                return new BaseResponse<Car>()
                {
                    Description = $"[GetCar] : {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<Car>> GetCarByName(string name)
        {
            var baseResponse = new BaseResponse<Car>();

            try
            {
                var car = await _carRepository.GetAll().FirstOrDefaultAsync(x => x.Name == name);

                if (car == null)
                {
                    baseResponse.Description = CarNotFoundMessage;
                    baseResponse.StatusCode = StatusCode.CarNotFound;

                    return baseResponse;
                }

                baseResponse.Data = car;
                return baseResponse;


            }
            catch (Exception ex)
            {
                return new BaseResponse<Car>()
                {
                    Description = $"[GetCar] : {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<Car>>> GetCars()
        {
            var baseResponse = new BaseResponse<IEnumerable<Car>>();

            try
            {
                var cars = _carRepository.GetAll();
                
                if(cars.Count() == 0)
                {
                    baseResponse.Description = ZeroCarsMessage;
                    baseResponse.StatusCode = StatusCode.OK;
                }

                baseResponse.Data = cars;
                baseResponse.StatusCode = StatusCode.OK;

                return baseResponse;
            }
            catch(Exception ex)
            {
                return new BaseResponse<IEnumerable<Car>>()
                {
                    Description = $"[GetCars] : {ex.Message}"
                };
            }
        }

        public BaseResponse<Dictionary<int, string>> GetTypes()
        {
            try
            {
                var types = ((TypeCar[])Enum.GetValues(typeof(TypeCar)))
                    .ToDictionary(key => (int)key, type => type.ToString());

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
    }
}
