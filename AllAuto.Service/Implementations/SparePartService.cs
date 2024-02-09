using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Enum;
using AllAuto.Domain.Extensions;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.SparePart;
using AllAuto.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace AllAuto.Service.Implementations
{
    /// <summary>
    /// Прослойка между БД и View
    /// </summary>
    public class SparePartService : ISparePartService
    {
        private const string CarNotFoundMessage = "Car not found";
        private const string ZeroCarsMessage = "Найдено 0 элементов";

        private readonly IBaseRepository<SparePart> _carRepository;
        private readonly ILogger<SparePartService> _logger;

        public SparePartService(IBaseRepository<SparePart> carRepository,ILogger<SparePartService> logger)
        {
            _carRepository = carRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<SparePartViewModel>> CreatePart(SparePartViewModel sparePartViewModel, byte[] image)
        {
            var baseResponse = new BaseResponse<SparePartViewModel>();

            try
            {
                var car = new SparePart()
                {
                    Description = sparePartViewModel.ShortDesription,
                    Model = sparePartViewModel.Manufacture,
                    Price = sparePartViewModel.Price,
                    Name = sparePartViewModel.Name,
                    Avatar = image,
                    Amount = sparePartViewModel.Amount,
                    TypeSparePart = (TypePart)Convert.ToInt32(sparePartViewModel.TypeSparePart)
                };

                await _carRepository.Create(car);
            }
            catch (Exception ex)
            {
                return new BaseResponse<SparePartViewModel>()
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

        public async Task<IBaseResponse<SparePart>> Edit(int id, SparePartViewModel model)
        {
            var baseResponse = new BaseResponse<SparePart>();

            try
            {
                var car = await _carRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if(car == null)
                {
                    baseResponse.StatusCode = StatusCode.CarNotFound;
                    baseResponse.Description = CarNotFoundMessage;
                    return baseResponse;
                }

                car.Description = model.ShortDesription;
                car.Model = model.Manufacture;
                car.Price = model.Price;
                car.Name = model.Name;

                await _carRepository.Update(car);

                return baseResponse;
                //TypeCar
            }
            catch (Exception ex)
            {
                return new BaseResponse<SparePart>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError       
                };
            }
        }

        public async Task<BaseResponse<SparePartOverviewViewModel>> GetCar(int id)
        {
            var baseResponse = new BaseResponse<SparePartOverviewViewModel>();

            try
            {
                var car = await _carRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

                if (car == null)
                {
                    baseResponse.Description = CarNotFoundMessage;
                    baseResponse.StatusCode = StatusCode.CarNotFound;

                    return baseResponse;
                }

                var viewModel = new SparePartOverviewViewModel
                {
                    Name = car.Name,
                    Description = car.Description,
                    Model = car.Model,
                    Price = car.Price,
                    DateCreate = car.DateCreate,
                    Amount = car.Amount,
                    TypeSparePart = car.TypeSparePart,
                    Avatar = car.Avatar,
                };

                baseResponse.Data = viewModel;
                return baseResponse;


            }
            catch (Exception ex)
            {
                return new BaseResponse<SparePartOverviewViewModel>()
                {
                    Description = $"[GetCar] : {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<SparePart>> GetCarByName(string name)
        {
            var baseResponse = new BaseResponse<SparePart>();

            try
            {
                var part = await _carRepository.GetAll().FirstOrDefaultAsync(x => x.Name == name);

                if (part == null)
                {
                    baseResponse.Description = CarNotFoundMessage;
                    baseResponse.StatusCode = StatusCode.CarNotFound;

                    return baseResponse;
                }

                baseResponse.Data = part;
                return baseResponse;


            }
            catch (Exception ex)
            {
                return new BaseResponse<SparePart>()
                {
                    Description = $"[GetCar] : {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<SparePart>>> GetParts()
        {
            var baseResponse = new BaseResponse<IEnumerable<SparePart>>();

            _logger.LogError($"[Login]: Message");
            try
            {
                var parts = _carRepository.GetAll();
                
                if(parts.Count() == 0)
                {
                    baseResponse.Description = ZeroCarsMessage;
                    baseResponse.StatusCode = StatusCode.OK;
                }

                baseResponse.Data = parts;
                baseResponse.StatusCode = StatusCode.OK;

                return baseResponse;
            }
            catch(Exception ex)
            {
                return new BaseResponse<IEnumerable<SparePart>>()
                {
                    Description = $"[GetCars] : {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<SparePart>>> GetPartsToType(TypePart type)
        {
            var baseResponse = new BaseResponse<IEnumerable<SparePart>>();

            try
            {
                var parts = _carRepository.GetAll().Where(x => x.TypeSparePart == type);

                if (parts.Count() == 0)
                {
                    baseResponse.Description = ZeroCarsMessage;
                    baseResponse.StatusCode = StatusCode.OK;
                }

                baseResponse.Data = parts;
                baseResponse.StatusCode = StatusCode.OK;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<SparePart>>()
                {
                    Description = $"[GetCars] : {ex.Message}"
                };
            }
        }

        public BaseResponse<Dictionary<int, string>> GetTypes()
        {
            try
            {
                var types = ((TypePart[])Enum.GetValues(typeof(TypePart)))
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
    }
}
