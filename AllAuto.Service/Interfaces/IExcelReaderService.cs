
using AllAuto.Domain.Response;
using Microsoft.AspNetCore.Http;

namespace AllAuto.Service.Interfaces
{
    public interface IExcelReaderService<T> where T : class
    {
        Task ReadExcelFile();
        Task<BaseResponse<bool>> ReadExcelFile(IFormFile file);
    }
}
