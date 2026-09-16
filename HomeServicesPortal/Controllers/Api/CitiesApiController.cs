using HomeServicesPortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeServicesPortal.Controllers.Api;

[ApiController]
[Route("api/cities")]
[AllowAnonymous]
public class CitiesApiController : ControllerBase
{
    private const string CitiesConfigKey = "Cities";

    private readonly IConfigurationEntryService _configurations;

    public CitiesApiController(IConfigurationEntryService configurations)
    {
        _configurations = configurations;
    }

    /// <summary>
    /// Get the configured city options (admin-managed via Configurations, ConfigKey=Cities).
    /// Use these values for the register-provider City field and any other city picker.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<string>>> GetAll(CancellationToken cancellationToken)
    {
        var cities = await _configurations.GetValuesByKeyAsync(CitiesConfigKey, cancellationToken);
        return Ok(cities);
    }
}
