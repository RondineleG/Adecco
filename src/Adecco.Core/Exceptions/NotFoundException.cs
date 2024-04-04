namespace Adecco.Core.Exceptions;
public class NotFoundException(string message) : CustomException(message)
{
}