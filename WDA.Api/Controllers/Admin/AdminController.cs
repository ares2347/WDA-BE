using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WDA.Domain;
using WDA.Domain.Repositories;

namespace WDA.Api.Controllers.Admin;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public AdminController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<object?>> GetAnalysis(CancellationToken _)
    {
        var monthlyIncome = await _unitOfWork.AdminRepository.GetAnalysis(_);
        return monthlyIncome;
    }
}