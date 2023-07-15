using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WDA.Api.Dto.Feedback;
using WDA.Api.Dto.Forum;
using WDA.Domain.Enums;
using WDA.Domain.Repositories;
using WDA.Shared;

namespace WDA.Api.Controllers.Feedback;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FeedbackController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserContext _userContext;
    private readonly UserManager<Domain.Models.User.User> _userManager;

    public FeedbackController(IMapper mapper, IUnitOfWork unitOfWork, UserContext userContext,
        UserManager<Domain.Models.User.User> userManager)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _userManager = userManager;
    }

    [HttpGet]
    public ActionResult<IQueryable<FeedbackResponse>> GetFeedbacks(CancellationToken _)
    {
        var documents = _unitOfWork.FeedbackRepository.Get()
            .Include(x => x.Customer)
            .Include(x => x.CreatedBy)
            .Include(x => x.ModifiedBy)
            .Select(x => _mapper.Map<FeedbackResponse>(x));
        return Ok(documents);
    }

    [HttpPost]
    public async Task<ActionResult<FeedbackResponse?>> CreateFeedback(CreateFeedbackRequest request,
        CancellationToken _)
    {
        try
        {
            var newFeedback = _mapper.Map<Domain.Models.Feedback.Feedback>(request);
            var customer = await _unitOfWork.CustomerRepository.GetById(request.CustomerId);
            if (customer is null) return NotFound("Customer not found.");
            var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString());
            newFeedback.Customer = customer;
            newFeedback.FeedbackStatus = FeedbackStatus.Open;
            newFeedback.CreatedBy = user;
            newFeedback.ModifiedBy = user;
            var feedback = await _unitOfWork.FeedbackRepository.Create(newFeedback, _);
            await _unitOfWork.SaveChangesAsync(_);
            var result = _mapper.Map<FeedbackResponse>(feedback);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<IQueryable<FeedbackResponse>>> GetFeedbackById([FromRoute] Guid id,
        CancellationToken _)
    {
        var feedback = await _unitOfWork.FeedbackRepository.GetById(id, _);
        if (feedback is null) return NotFound();
        var res = _mapper.Map<FeedbackResponse>(feedback);
        return Ok(res);
    }
}