namespace Adecco.Core.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string entityName, int id) : base($"Entidade '{entityName}' com o Id:'{id}' não encontrado.") { }
}
