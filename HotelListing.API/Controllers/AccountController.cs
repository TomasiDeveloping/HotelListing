using HotelListing.Core.Contracts;
using HotelListing.Core.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class AccountController : ControllerBase
{
    private readonly IAuthManager _authManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAuthManager authManager, ILogger<AccountController> logger)
    {
        _authManager = authManager;
        _logger = logger;
    }

    // POST: api/account/register
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Register([FromBody] ApiUserDto userDto)
    {
        _logger.LogInformation($"Registration Attempt for {userDto.Email}");


        var errors = await _authManager.RegisterAsync(userDto);
        var identityErrors = errors.ToList();
        if (!identityErrors.Any()) return Ok();
        foreach (var error in identityErrors) ModelState.AddModelError(error.Code, error.Description);

        return BadRequest(ModelState);
    }

    // POST: api/account/login
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        _logger.LogInformation($"Login Attempt for {loginDto.Email}");


        var authResponse = await _authManager.LoginAsync(loginDto);

        if (authResponse is null) return Unauthorized();

        return Ok(authResponse);
    }

    // POST: api/account/refreshtoken
    [HttpPost("refreshtoken")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] AuthResponseDto authResponseDto)
    {
        var authResponse = await _authManager.VerifyRefreshTokenAsync(authResponseDto);

        if (authResponse is null) return Unauthorized();

        return Ok(authResponse);
    }
}