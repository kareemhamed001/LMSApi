namespace BusinessLayer.Responses
{
    public class ApiResponse<T>
    {
        public List<T> Data { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }

    }
}
