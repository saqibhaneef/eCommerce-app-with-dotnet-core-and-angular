namespace api.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int StatusCode, string message=null)
        {
            StatusCode=StatusCode;
            message=message ?? GetDefaultMessageForStatusCode(StatusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400=>"Bad request",
                401=>"You are not authorized",
                404=>"Not found",
                500=>"Error are path to dark side",
                _=>null,
            };
        }
    }
}