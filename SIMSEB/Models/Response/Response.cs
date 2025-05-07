namespace CineProyecto.WebApi.Models.Response
{
    public class Response<T>
    {
        public int code { get; set; }
        public required string message { get; set; }
        public required T data { get; set; }

    }
    public static class MessagesCode
    {
        public const int Success = 200;
        public const int Created = 201;
        public const int Updated = 204;
        public const int NotFound = 404;
        public const int BadRequest = 400;
        public const int Error = 500;
    }
}
