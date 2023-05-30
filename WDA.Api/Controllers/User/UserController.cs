using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WDA.Shared;
using IAuthorizationService = WDA.Service.User.IAuthorizationService;

namespace WDA.Api.Controllers.User;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserManager<Domain.User.User> _userManager;
    private readonly IAuthorizationService _authorizationService;

    public UserController(UserManager<Domain.User.User> userManager, IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _authorizationService = authorizationService;
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string?>> Login(string identifier, string password, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(identifier))
        {
            return ValidationProblem($"Invalid {nameof(identifier)}");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            return ValidationProblem($"Invalid {nameof(password)}");
        }

        try
        {
            var jwt = await _authorizationService.AuthorizeUser(identifier, password, cancellationToken);
            return Ok(jwt);
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<string?>> Register(string? username, string email, string password, string firstName, string lastName, List<string> roles, CancellationToken _)
    {
        if (!Helper.ValidateEmailString(email))
        {
            return ValidationProblem($"Invalid {nameof(email)}");
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            return ValidationProblem($"Invalid {nameof(password)}");
        }
        if (string.IsNullOrEmpty(firstName))
        {
            return ValidationProblem($"Invalid {nameof(firstName)}");
        }
        if (string.IsNullOrEmpty(lastName))
        {
            return ValidationProblem($"Invalid {nameof(lastName)}");
        }

        var res = await _authorizationService.RegisterUser(username, email, password, firstName, lastName, roles, _);
        return Ok(res);
    }

    [HttpPost("change-password")]
    public async Task<ActionResult<bool?>> ChangePassword(Guid userId, string oldPassword, string newPassword, CancellationToken _)
    {
        if (string.IsNullOrWhiteSpace(oldPassword))
        {
            return ValidationProblem($"Invalid {nameof(oldPassword)}");
        }        
        if (string.IsNullOrWhiteSpace(newPassword))
        {
            return ValidationProblem($"Invalid {nameof(newPassword)}");
        }

        try
        {
            var res = await _authorizationService.ChangePassword(userId, oldPassword, newPassword, _);
            return Ok(res.Succeeded);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}