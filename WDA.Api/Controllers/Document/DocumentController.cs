using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WDA.Api.Dto.Document.Request;
using WDA.Api.Dto.Document.Response;
using WDA.Domain.Repositories;
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

    public DocumentController(IUnitOfWork unitOfWork, IMapper mapper, UserContext userContext, UserManager<Domain.Models.User.User> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContext = userContext;
        _userManager = userManager;
    }
    
    [HttpGet]
    public ActionResult<IQueryable<DocumentResponse>> GetDocuments(CancellationToken _)
    {
        var documents = _unitOfWork.DocumentRepository.Get().Select(x => _mapper.Map<DocumentResponse>(x));
        return Ok(documents);
    }
    [HttpPost]
    public async Task<ActionResult<DocumentResponse?>> CreateDocument(CreateDocumentRequest request, CancellationToken _)
    {
        try
        {
            var newDocument = _mapper.Map<Domain.Models.Document.Document>(request);
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
    public async Task<ActionResult<IQueryable<DocumentResponse>>> GetDocumentById([FromRoute] Guid id, CancellationToken _)
    {
        var document = await _unitOfWork.DocumentRepository.GetById(id, _);
        if (document is null) return NotFound();
        var res = _mapper.Map<DocumentResponse>(document);
        return Ok(res);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DocumentResponse?>> UpdateDocument([FromRoute] Guid id, UpdateDocumentRequest request, CancellationToken _)
    {
        try
        {
            var document = _mapper.Map<Domain.Models.Document.Document>(request);
            var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString());
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
}