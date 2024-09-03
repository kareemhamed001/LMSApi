namespace BusinessLayer.Responses
{
    public class ApiResponseListStrategy<T> : ApiResponseBase, IApiResponse
    {
        public IEnumerable<T>? Data { get; set; }
    }
}
