namespace LMSApi.App.Responses
{
    public class ApiResponseListStrategy<T> : ApiResponseBase, IApiResponse
    {
        public IEnumerable<T>? Data { get; set; }
    }
}
