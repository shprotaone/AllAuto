namespace AllAuto.Domain.Enum
{
    public enum StatusCode
    {
        CarNotFound = 0,

        OK = 200,
        InternalServerError = 500,
        UserAlreadyExist = 501,
        UserNotFound = 502,
        OrderNotFound = 503,
    }
}
