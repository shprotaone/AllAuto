using AllAuto.Domain.Entity;
using AllAuto.Domain.Enum;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.SparePart;

namespace AllAuto.Service.Interfaces
{
    public interface ISparePartService
    {
        Task<BaseResponse<IEnumerable<SparePart>>> GetParts();

        Task<BaseResponse<SparePartOverviewViewModel>> GetCar(int id);

        Task<BaseResponse<SparePart>> GetCarByName(string name);

        Task<BaseResponse<SparePartViewModel>> CreatePart(SparePartViewModel carViewModel, byte[] imageData);

        Task<IBaseResponse<bool>> DeleteCar(int id);

        Task<IBaseResponse<SparePart>> Edit(int id,SparePartViewModel model);

        BaseResponse<Dictionary<int, string>> GetTypes();
        Task<BaseResponse<IEnumerable<SparePart>>> GetPartsToType(TypePart type);
    }
}
