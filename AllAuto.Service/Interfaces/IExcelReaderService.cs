
namespace AllAuto.Service.Interfaces
{
    public interface IExcelReaderService<T> where T : class
    {
        List<T> Read();
        void ReadExcelFile();
    }
}
