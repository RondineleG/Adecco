namespace Adecco.Application.Constants;

public static class RegexPatterns
{
    public const string Telefone = @"^[0-9]{8,9}$";
    public const string CEP = @"^\d{8}$";
    public const string Email = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    public const string Celular = @"^9\d{8}$";
    public const string ResidencialComercial = @"^\d{4}\d{4}$";
    public const string RG = @"^[0-9A-Za-z]{5,9}$";
}