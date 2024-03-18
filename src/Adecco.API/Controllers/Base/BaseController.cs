namespace Adecco.API.Controllers.Base;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseController : ControllerBase { }
