
using AllAuto.Domain.Response;
using Microsoft.AspNetCore.Http;

namespace AllAuto.Service.Interfaces
{
    public interface IExcelReaderService<T> where T : class
    {
        List<T> Read();
        Task ReadExcelFile();
        Task<BaseResponse<bool>> ReadExcelFile(IFormFile file);
    }
}
