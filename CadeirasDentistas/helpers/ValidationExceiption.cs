
namespace CadeirasDentistas.Helper
{
    public class ValidationException : Exception
    {
        public string PropertyName { get; }
        public object InvalidValue { get; }
        public ValidationException(string message, string propertyName, object invalidValue, Exception innerException = null)
            : base(message, innerException)
        {
            PropertyName = propertyName;
            InvalidValue = invalidValue;
        }

    }
    
}