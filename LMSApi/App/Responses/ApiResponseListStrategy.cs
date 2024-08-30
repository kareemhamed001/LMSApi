namespace LMSApi.App.Responses
{
    public class ApiResponseListStrategy<T> : ApiResponseBase, IApiResponse
    {
        public List<T>? Data { get; set; }
    }
}
