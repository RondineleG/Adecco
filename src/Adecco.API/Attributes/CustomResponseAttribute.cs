namespace Adecco.API.Attributes;

[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Method,
    AllowMultiple = true,
    Inherited = true
)]
public class CustomResponseAttribute : ProducesResponseTypeAttribute
{
    public CustomResponseAttribute(int statusCode)
        : base(typeof(ApiCustomResult), statusCode) { }
}
