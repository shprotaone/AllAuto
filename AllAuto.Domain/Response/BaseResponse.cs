using AllAuto.Domain.Enum;

namespace AllAuto.Domain.Response
{
    //класс обратной связи? 
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string Description { get; set; }//описание ошибки

        public StatusCode StatusCode { get; set; }//Статус ошибки

        public T Data { get; set; }//результат запроса
    }

    public interface IBaseResponse<T>
    {
        T Data { get; }
    }
}
