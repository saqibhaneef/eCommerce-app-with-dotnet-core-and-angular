namespace api.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int StatusCode, string message = null,string details=null) : base(StatusCode, message)
        {
        }

        public string Details { get; set; }
    }
}