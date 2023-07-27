using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WDA.Api.Dto.User.Request;
using WDA.Api.Dto.User.Response;
using WDA.Domain;
using WDA.Domain.Enums;
using WDA.Domain.Repositories;
using WDA.Shared;
using IAuthorizationService = WDA.Service.User.IAuthorizationService;

namespace WDA.Api.Controllers.Employee;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Models.User.User> _userManager;
    private readonly IAuthorizationService _authorizationService;

    public EmployeeController(IMapper mapper,
        UserManager<Domain.Models.User.User> userManager, IAuthorizationService authorizationService)
    {
        _mapper = mapper;
        _userManager = userManager;
        _authorizationService = authorizationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserInfoResponse>>> GetEmployees(CancellationToken _)
    {
        var hr = await _userManager.GetUsersInRoleAsync(RoleName.Hr);
        var hrManager = await _userManager.GetUsersInRoleAsync(RoleName.HrManager);
        var sale = await _userManager.GetUsersInRoleAsync(RoleName.Sale);
        var saleManager = await _userManager.GetUsersInRoleAsync(RoleName.SaleManager);
        var users = hr.Concat(hrManager).Concat(sale).Concat(saleManager);
        var enumerable = users.ToList();
        foreach (var user in enumerable)
        {
            user.Roles = (await _userManager.GetRolesAsync(user)).ToList();
        }        

        var response = enumerable.Select( employee => _mapper.Map<UserInfoResponse>(employee)).AsEnumerable();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<UserInfoResponse?>> Register(CreateNewAccountRequest request, CancellationToken _)
    {
        if (!Helper.ValidateEmailString(request.Email))
        {
            return ValidationProblem($"Invalid {nameof(request.Email)}");
        }

        if (string.IsNullOrEmpty(request.FullName))
        {
            return ValidationProblem($"Invalid {nameof(request.FullName)}");
        }

        if (string.IsNullOrEmpty(request.Department))
        {
            return ValidationProblem($"Invalid {nameof(request.Department)}");
        }

        if (string.IsNullOrEmpty(request.Position))
        {
            return ValidationProblem($"Invalid {nameof(request.Position)}");
        }

        var employee = new Domain.Models.User.User()
        {
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.Phone,
            Department = request.Department,
            FullName = request.FullName,
            // Only force users to change password at first login if the account password is not identified
            PasswordChangeRequired = true
        };

        var user =  await _authorizationService.RegisterUser(employee, new List<string> { RoleName.Hr }, null, _);
        var res = _mapper.Map<UserInfoResponse>(user);
        return Ok(res);
    }
}