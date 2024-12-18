namespace CadeirasDentistas.midleware
{
    public class ApiException
    {
        public string StatusCode {get; set;}

        public string Message {get; set;}


        public ApiException(string statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    };
    
}