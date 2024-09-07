namespace BusinessLayer.Responses
{
    public class ApiResponseFactory
    {
        public static IApiResponse Create<T>(IEnumerable<T>? data, string message, int status, bool success)
        {
            return new ApiResponseListStrategy<T>
            {
                Data = data,
                Message = message,
                Status = status,
                Success = success
            };
        }

        public static IApiResponse Create(object? data, string message, int status, bool success)
        {
            return new ApiResponseSingleStrategy
            {
                Data = data,
                Message = message,
                Status = status,
                Success = true
            };
        }

        public static IApiResponse Create(string message, int status, bool success)
        {
            return new ApiResponseBase
            {
                Message = message,
                Status = status,
                Success = success
            };
        }
    }
}
