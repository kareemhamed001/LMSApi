namespace BusinessLayer.Responses
{
    public interface IApiResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }
    }
}
