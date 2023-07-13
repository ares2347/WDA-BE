using System.Net;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WDA.Api.Dto.Attachment;
using WDA.Api.Dto.User.Request;
using WDA.Api.Dto.User.Response;
using WDA.Domain.Models.Attachment;
using WDA.Domain.Models.User;
using WDA.Domain.Repositories;
using WDA.Service.Attachment;
using WDA.Shared;
using IAuthorizationService = WDA.Service.User.IAuthorizationService;

namespace WDA.Api.Controllers.User;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserContext _userContext;
    private readonly UserManager<Domain.Models.User.User> _userManager;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMapper _mapper;
    private readonly IAttachmentService _attachmentService;
    private readonly IUnitOfWork _unitOfWork;

    public UserController(
        UserContext userContext,
        UserManager<Domain.Models.User.User> userManager,
        IAuthorizationService authorizationService,
        IMapper mapper,
        IAttachmentService attachmentService,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _userManager = userManager;
        _authorizationService = authorizationService;
        _mapper = mapper;
        _attachmentService = attachmentService;
        _unitOfWork = unitOfWork;
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

        var user = new Domain.Models.User.User()
        {
            UserName = request.Username ?? request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Department = request.Department,
            DateOfBirth = request.DateOfBirth,
            // Position = request.Position,
            // Only force users to change password at first login if the account password is not identified
            PasswordChangeRequired = request.Password is null,
        };

        var res = await _authorizationService.RegisterUser(user, request.Roles, request.Password, _);
        var token = await _authorizationService.IssueToken(res, _);
        return Ok(token);
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
            var res = await _authorizationService.ChangePassword(request.UserId, request.OldPassword,
                request.NewPassword, _);
            return Ok(res.Succeeded);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserInfoResponse>> GetUserInfo(CancellationToken _)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString());
            if (user is null) return NotFound();
            user.Roles = (await _userManager.GetRolesAsync(user)).ToList();
            var res = _mapper.Map<UserInfoResponse>(user);
            return Ok(res);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("ProfilePicture")]
    [RequestSizeLimit(5 * 1014 * 1024)]
    public async Task<IdentityResult?> UploadProfilePicture(IFormFile file,
        CancellationToken _)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString());
            var attachmentId = NewId.NextGuid();
            var filePath =
                await _attachmentService.SaveFileAsync(file.OpenReadStream(), attachmentId.ToString());

            var attachment = new Attachment
            {
                AttachmentId = attachmentId,
                Path = filePath,
                Name = file.FileName,
                Size = file.Length,
                ContentType = file.ContentType,
                CreatedBy = user,
                CreatedAt = DateTimeOffset.Now,
                ModifiedBy = user,
                ModifiedAt = DateTimeOffset.Now
            };

            var res = await _unitOfWork.AttachmentRepository.Create(attachment, _);
            await _unitOfWork.SaveChangesAsync(_);
            user!.ProfilePictureId = res?.AttachmentId;
            return await _userManager.UpdateAsync(user);
        }
        catch (Exception e)
        {
            throw new HttpException("Upload profile picture failed.", HttpStatusCode.BadRequest);
        }
    }
}