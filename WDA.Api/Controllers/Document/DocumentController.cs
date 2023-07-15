using System.Net;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WDA.Api.Dto.Attachment;
using WDA.Api.Dto.Document.Request;
using WDA.Api.Dto.Document.Response;
using WDA.Domain.Models.Attachment;
using WDA.Domain.Repositories;
using WDA.Service.Attachment;
using WDA.Shared;

namespace WDA.Api.Controllers.Document;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DocumentController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserContext _userContext;
    private readonly UserManager<Domain.Models.User.User> _userManager;
    private readonly IAttachmentService _attachmentService;

    public DocumentController(IUnitOfWork unitOfWork, IMapper mapper, UserContext userContext,
        UserManager<Domain.Models.User.User> userManager, IAttachmentService attachmentService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContext = userContext;
        _userManager = userManager;
        _attachmentService = attachmentService;
    }

    [HttpGet]
    public ActionResult<IQueryable<DocumentResponse>> GetDocuments(CancellationToken _)
    {
        var documents = _unitOfWork.DocumentRepository.Get()
            .Include(x => x.Attachments)
            .Include(x => x.CreatedBy)
            .Include(x=> x.ModifiedBy)
            .Select(x => _mapper.Map<DocumentResponse>(x));
        return Ok(documents);
    }

    [HttpPost]
    public async Task<ActionResult<DocumentResponse?>> CreateDocument(CreateDocumentRequest request,
        CancellationToken _)
    {
        try
        {
            var newDocument = _mapper.Map<Domain.Models.Document.Document>(request);
            if (request.AttachmentIds is not null)
            {
                var attachments = await _unitOfWork.AttachmentRepository
                    .Get(x => request.AttachmentIds.Contains(x.AttachmentId)).ToListAsync(_);
                newDocument.Attachments = attachments;
            }
            
            var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString());
            newDocument.CreatedBy = user;
            newDocument.ModifiedBy = user;
            var customer = await _unitOfWork.DocumentRepository.Create(newDocument, _);
            await _unitOfWork.SaveChangesAsync(_);
            var result = _mapper.Map<DocumentResponse>(customer);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IQueryable<DocumentResponse>>> GetDocumentById([FromRoute] Guid id,
        CancellationToken _)
    {
        var document = await _unitOfWork.DocumentRepository.GetById(id, _);
        if (document is null) return NotFound();
        var res = _mapper.Map<DocumentResponse>(document);
        return Ok(res);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DocumentResponse?>> UpdateDocument([FromRoute] Guid id,
        UpdateDocumentRequest request, CancellationToken _)
    {
        try
        {
            var document = _mapper.Map<Domain.Models.Document.Document>(request);
            var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString());
            if (request.AttachmentIds is not null)
            {
                var attachments = await _unitOfWork.AttachmentRepository
                    .Get(x => request.AttachmentIds.Contains(x.AttachmentId)).ToListAsync(_);
                document.Attachments = attachments;
            }
            document.DocumentId = id;
            document.ModifiedBy = user;
            document.ModifiedAt = DateTimeOffset.UtcNow;
            var updatedDocument = await _unitOfWork.DocumentRepository.Update(document, _);
            await _unitOfWork.SaveChangesAsync(_);
            if (updatedDocument is null)
                return NotFound();
            var result = _mapper.Map<DocumentResponse>(updatedDocument);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DocumentResponse?>> DeleteDocument([FromRoute] Guid id, CancellationToken _)
    {
        try
        {
            var res = await _unitOfWork.DocumentRepository.Delete(id, cancellationToken: _);
            await _unitOfWork.SaveChangesAsync(_);
            if (res)
                return Ok(res);
            return BadRequest(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("File")]
    [RequestSizeLimit(5 * 1014 * 1024)]
    public async Task<AttachmentResponse?> UploadAttachmentAsync(IFormFile file, CancellationToken _)
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
        return _mapper.Map<AttachmentResponse>(res);
    }

    [AllowAnonymous]
    [HttpGet("File/{attachmentId:guid}")]
    public async Task<FileStreamResult> PreviewAttachment([FromRoute] Guid attachmentId, CancellationToken _)
    {
        var attachment = await _unitOfWork.AttachmentRepository.GetById(attachmentId, _);
        HttpException.ThrowIfNull(attachment);  
        var contentType = attachment!.ContentType;
        var  content = await _attachmentService.BrowseFile(attachment.AttachmentId.ToString());
    
        return File(content, contentType, attachment.Name);
    }
}