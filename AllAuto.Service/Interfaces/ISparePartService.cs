using AllAuto.Domain.Entity;
using AllAuto.Domain.Enum;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.SparePart;

namespace AllAuto.Service.Interfaces
{
    public interface ISparePartService
    {
        Task<BaseResponse<IEnumerable<SparePart>>> GetParts();

        Task<BaseResponse<SparePartOverviewViewModel>> GetPart(int id);
        Task<BaseResponse<SparePartViewModel>> GetPartForEdit(int id);
        Task<BaseResponse<SparePart>> GetCarByName(string name);

        Task<BaseResponse<SparePartViewModel>> CreatePart(SparePartViewModel spareViewModel, byte[] imageData);

        Task<IBaseResponse<bool>> Delete(int id);

        Task<IBaseResponse<SparePart>> Edit(int id,SparePartViewModel model, byte[] image);

        BaseResponse<Dictionary<int, string>> GetTypes();

        Task<BaseResponse<IEnumerable<SparePart>>> GetPartsToType(TypePart type);
    }
}
