namespace Adecco.Core.Exceptions;
public abstract class CustomException(string message) : SystemException(message)
{
}