using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WDA.Api.Dto.User.Request;
using WDA.Shared;
using IAuthorizationService = WDA.Service.User.IAuthorizationService;

namespace WDA.Api.Controllers.User;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserManager<Domain.Models.User.User> _userManager;
    private readonly IAuthorizationService _authorizationService;

    public UserController(UserManager<Domain.Models.User.User> userManager, IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _authorizationService = authorizationService;
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string?>> Login(LoginRequest request, CancellationToken _)
    {
        if (string.IsNullOrWhiteSpace(request.Identifier))
        {
            return ValidationProblem($"Invalid {nameof(request.Identifier)}");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return ValidationProblem($"Invalid {nameof(request.Password)}");
        }

        try
        {
            var jwt = await _authorizationService.AuthorizeUser(request.Identifier, request.Password, _);
            return Ok(jwt);
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<string?>> Register(RegisterRequest request, CancellationToken _)
    {
        if (!Helper.ValidateEmailString(request.Email))
        {
            return ValidationProblem($"Invalid {nameof(request.Email)}");
        }
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return ValidationProblem($"Invalid {nameof(request.Password)}");
        }
        if (string.IsNullOrEmpty(request.FirstName))
        {
            return ValidationProblem($"Invalid {nameof(request.FirstName)}");
        }
        if (string.IsNullOrEmpty(request.LastName))
        {
            return ValidationProblem($"Invalid {nameof(request.LastName)}");
        }

        var res = await _authorizationService.RegisterUser(request.Username, request.Email, request.Password, request.FirstName, request.LastName, request.Roles, _);
        return Ok(res);
    }

    [HttpPost("change-password")]
    public async Task<ActionResult<bool?>> ChangePassword(ChangePasswordRequest request, CancellationToken _)
    {
        if (string.IsNullOrWhiteSpace(request.OldPassword))
        {
            return ValidationProblem($"Invalid {nameof(request.OldPassword)}");
        }        
        if (string.IsNullOrWhiteSpace(request.NewPassword))
        {
            return ValidationProblem($"Invalid {nameof(request.NewPassword)}");
        }

        try
        {
            var res = await _authorizationService.ChangePassword(request.UserId, request.OldPassword, request.NewPassword, _);
            return Ok(res.Succeeded);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}