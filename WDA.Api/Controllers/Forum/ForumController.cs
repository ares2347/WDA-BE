using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WDA.Api.Dto.Document.Request;
using WDA.Api.Dto.Document.Response;
using WDA.Api.Dto.Forum;
using WDA.Domain.Models.Thread;
using WDA.Domain.Repositories;
using WDA.Shared;
using Thread = WDA.Domain.Models.Thread.Thread;

namespace WDA.Api.Controllers.Forum;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ForumController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserContext _userContext;
    private readonly UserManager<Domain.Models.User.User> _userManager;
    private readonly IAuthorizationService _authorizationService;

    public ForumController(IMapper mapper, IUnitOfWork unitOfWork, UserContext userContext,
        UserManager<Domain.Models.User.User> userManager, IAuthorizationService authorizationService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _userManager = userManager;
        _authorizationService = authorizationService;
    }
    
    [HttpGet]
    public ActionResult<IQueryable<ThreadResponse>> GetThreads(CancellationToken _)
    {
        var documents = _unitOfWork.ThreadRepository.Get()
            .Include(x => x.Replies)
            .Include(x => x.CreatedBy)
            .Include(x=> x.ModifiedBy)
            .Select(x => _mapper.Map<ThreadResponse>(x));
        return Ok(documents);
    }

    [HttpPost]
    public async Task<ActionResult<ThreadResponse?>> CreateThread(CreateThreadRequest request,
        CancellationToken _)
    {
        try
        {
            var newThread = _mapper.Map<Thread>(request);
            var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString());
            newThread.CreatedBy = user;
            newThread.ModifiedBy = user;
            var thread = await _unitOfWork.ThreadRepository.Create(newThread, _);
            await _unitOfWork.SaveChangesAsync(_);
            var result = _mapper.Map<ThreadResponse>(thread);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IQueryable<ThreadResponse>>> GetThreadById([FromRoute] Guid id,
        CancellationToken _)
    {
        var thread = await _unitOfWork.ThreadRepository.GetById(id, _);
        if (thread is null) return NotFound();
        var res = _mapper.Map<ThreadResponse>(thread);
        return Ok(res);
    }

    [HttpPost("Reply/{id}")]
    public async Task<ActionResult<ReplyResponse>> CreateReply([FromRoute] Guid id, CreateReplyRequest request, CancellationToken _)
    {
        var thread = await _unitOfWork.ThreadRepository.GetById(id, _);
        if (thread is null) return NotFound();
        var newReply = _mapper.Map<Reply>(request);
        newReply.Thread = thread;
        var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString());
        newReply.CreatedBy = user;
        newReply.ModifiedBy = user;
        var reply = await _unitOfWork.ReplyRepository.Create(newReply, _);
        await _unitOfWork.SaveChangesAsync(_);
        var res = _mapper.Map<ReplyResponse>(reply);
        return Ok(res);
    }

    // [HttpPut("{id}")]
    // public async Task<ActionResult<ThreadResponse?>> UpdateThread([FromRoute] Guid id,
    //     UpdateDocumentRequest request, CancellationToken _)
    // {
    //     try
    //     {
    //         var document = _mapper.Map<Domain.Models.Document.Document>(request);
    //         var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString());
    //         if (request.AttachmentIds is not null)
    //         {
    //             var attachments = await _unitOfWork.AttachmentRepository
    //                 .Get(x => request.AttachmentIds.Contains(x.AttachmentId)).ToListAsync(_);
    //             document.Attachments = attachments;
    //         }
    //         document.DocumentId = id;
    //         document.ModifiedBy = user;
    //         document.ModifiedAt = DateTimeOffset.UtcNow;
    //         var updatedDocument = await _unitOfWork.DocumentRepository.Update(document, _);
    //         await _unitOfWork.SaveChangesAsync(_);
    //         if (updatedDocument is null)
    //             return NotFound();
    //         var result = _mapper.Map<DocumentResponse>(updatedDocument);
    //         return Ok(result);
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(ex.Message);
    //     }
    // }

    // [HttpDelete("{id}")]
    // public async Task<ActionResult<DocumentResponse?>> DeleteThread([FromRoute] Guid id, CancellationToken _)
    // {
    //     try
    //     {
    //         var res = await _unitOfWork.DocumentRepository.Delete(id, cancellationToken: _);
    //         await _unitOfWork.SaveChangesAsync(_);
    //         if (res)
    //             return Ok(res);
    //         return BadRequest(res);
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(ex.Message);
    //     }
    // }
}