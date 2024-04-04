namespace Adecco.Core.Exceptions;
public class ErrorOnValidationException(string message) : CustomException(message)
{
}