namespace BusinessLayer.Responses
{
    public class ApiResponseSingleStrategy:ApiResponseBase, IApiResponse
    {
        public object? Data { get; set; }
    }
}
