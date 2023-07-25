using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WDA.Api.Dto.CustomerTicket;
using WDA.Api.Dto.EmployeeTicket;
using WDA.Domain.Enums;
using WDA.Domain.Models.Email;
using WDA.Domain.Models.Ticket;
using WDA.Domain.Repositories;
using WDA.Service.Email;
using WDA.Shared;
using IAuthorizationService = WDA.Service.User.IAuthorizationService;

namespace WDA.Api.Controllers.Ticket;

[Route("api/[controller]")]
[ApiController]
public class EmployeeTicketController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly UserContext _userContext;
    private readonly UserManager<Domain.Models.User.User> _userManager;
    private readonly IAuthorizationService _authorizationService;

    public EmployeeTicketController(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService,
        UserContext userContext, UserManager<Domain.Models.User.User> userManager,
        IAuthorizationService authorizationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailService = emailService;
        _userContext = userContext;
        _userManager = userManager;
        _authorizationService = authorizationService;
    }

    [Authorize]
    [HttpGet]
    public ActionResult<IQueryable<EmployeeTicketResponse>> GetEmployeeTickets(TicketStatus? status,
        CancellationToken _, int page = 0,
        int size = 10)
    {
        var res = _unitOfWork.TicketRepository.GetEmployeeTickets(x => !x.IsDeleted, size, page);
        if (status is not null)
        {
            res = res.Where(x => x.Status == status);
        }

        return Ok(res);
    }

    [Authorize]
    [HttpGet("TicketsByEmployee/{employeeId:guid}")]
    public ActionResult<IQueryable<EmployeeTicketResponse>> GetMyTickets([FromRoute] Guid employeeId,
        CancellationToken _, int page = 0,
        int size = 10)
    {
        var res = _unitOfWork.TicketRepository.GetEmployeeTickets(
            x => !x.IsDeleted && x.RequestorId == employeeId, size, page);
        return Ok(res);
    }

    [Authorize]
    [HttpGet("AssignedTickets")]
    public ActionResult<IQueryable<EmployeeTicketResponse>> GetAssignedTickets(CancellationToken _, int page = 0,
        int size = 10)
    {
        var res = _unitOfWork.TicketRepository.GetEmployeeTickets(
            x => !x.IsDeleted && x.ResolverId == _userContext.UserId, size, page);
        return Ok(res);
    }

    [HttpPost("Create")]
    public async Task<ActionResult<EmployeeTicketResponse>> CreateEmployeeTickets(CreateEmployeeTicketRequest request,
        CancellationToken _)
    {
        try
        {
            _authorizationService.ValidateToken(request.ValidationToken);
            //create customer ticket
            var ticket = _mapper.Map<EmployeeTicket>(request);
            var requestor = await _userManager.FindByIdAsync(request.EmployeeId.ToString());
            if (requestor is null) return NotFound("Employee not found");
            ticket.Requestor = requestor;
            ticket.Status = TicketStatus.Opened;
            var res = _unitOfWork.TicketRepository.CreateEmployeeTicket(ticket);
            await _unitOfWork.SaveChangesAsync(_);

            //send email
            var emailTemplate = await _emailService.GetEmailTemplate(EmailTemplateType.TicketOpened, _);
            var subject = emailTemplate!.Subject.Replace("[[TicketId]]",
                res?.TicketId.ToString() ?? string.Empty);
            var replacements = new Dictionary<string, string>
            {
                { "CreatedAt", $"{ticket.CreatedAt:D} " },
                { "TicketId", ticket.TicketId.ToString() },
                { "RequestorFullName", ticket.Requestor.FullName ?? ticket.Requestor.Email ?? string.Empty },
            };
            var bodyBuilder = new StringBuilder(emailTemplate.Body);
            foreach (var pair in replacements)
            {
                bodyBuilder.Replace($"[[{pair.Key}]]", pair.Value);
            }

            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Customer not found");

            await _emailService.Send(subject, bodyBuilder.ToString(), res!.Requestor.Email, null, null, _);
            //end of send email

            var result = _mapper.Map<EmployeeTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest("Unexpected error occured.");
        }
    }

    [Authorize(Roles = $"{RoleName.Hr},{RoleName.HrManager}")]
    [HttpPut("Assign")]
    public async Task<ActionResult<EmployeeTicketResponse>> AssignEmployeeTicket(AssignEmployeeTicketRequest request,
        CancellationToken _)
    {
        try
        {
            var ticket = await _unitOfWork.TicketRepository
                .GetEmployeeTickets(
                    x => !x.IsDeleted && x.TicketId == request.TicketId && x.Status == TicketStatus.Opened, null, null)
                .FirstOrDefaultAsync(_);
            if (ticket is null) return NotFound("Ticket not found.");
            if (!_userContext.Roles.Contains(RoleName.HrManager) && request.EmployeeId is not null &&
                _userContext.UserId != request.EmployeeId)
                return Unauthorized("Unauthorized to perform action.");
            var resolver =
                await _userManager.FindByIdAsync(request.EmployeeId?.ToString() ?? _userContext.UserId.ToString());
            ticket.Resolver = resolver;
            ticket.Status = TicketStatus.Pending;
            var res = _unitOfWork.TicketRepository.UpdateEmployeeTicket(ticket);
            await _unitOfWork.SaveChangesAsync(_);

            //send email customer
            var customerEmailTemplate = await _emailService.GetEmailTemplate(EmailTemplateType.TicketPending, _);
            var subject = customerEmailTemplate!.Subject.Replace("[[TicketId]]",
                res?.TicketId.ToString() ?? string.Empty);
            var customerReplacements = new Dictionary<string, string>
            {
                { "CreatedAt", $"{ticket.CreatedAt:D} " },
                { "TicketId", ticket.TicketId.ToString() },
                { "RequestorFullName", ticket.Requestor.FullName ?? string.Empty },
                { "ResolverFullName", ticket.Resolver?.FullName ?? string.Empty },
            };
            var bodyBuilder = new StringBuilder(customerEmailTemplate.Body);
            foreach (var pair in customerReplacements)
            {
                bodyBuilder.Replace($"[[{pair.Key}]]", pair.Value);
            }

            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Requestor not found");

            await _emailService.Send(subject, bodyBuilder.ToString(), res!.Requestor.Email, null, null, _);
            //end of send email customer
            //
            //send email employee
            var employeeEmailTemplate = await _emailService.GetEmailTemplate(EmailTemplateType.TicketAssigned, _);
            var employeeEmailSubject = employeeEmailTemplate!.Subject.Replace("[[TicketId]]",
                res?.TicketId.ToString() ?? string.Empty);
            var employeeReplacements = new Dictionary<string, string>
            {
            };
            var employeeEmailBodyBuilder = new StringBuilder(employeeEmailTemplate.Body);
            foreach (var pair in employeeReplacements)
            {
                bodyBuilder.Replace($"[[{pair.Key}]]", pair.Value);
            }

            if (string.IsNullOrEmpty(res?.Resolver?.Email) ||
                !Helper.ValidateEmailString(res?.Resolver?.Email ?? string.Empty))
                return NotFound("Resolver not found");

            await _emailService.Send(employeeEmailSubject, bodyBuilder.ToString(), res!.Resolver!.Email, null, null, _);
            //end of send email employee
            var result = _mapper.Map<EmployeeTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest("Unexpected error occured.");
        }
    }

    [Authorize(Roles = $"{RoleName.Hr},{RoleName.HrManager}")]
    [HttpPut("Processing/{ticketId:guid}")]
    public async Task<ActionResult<EmployeeTicketResponse>> ProcessingEmployeeTicket(
        [FromRoute] Guid ticketId,
        CancellationToken _)
    {
        try
        {
            var ticket = await _unitOfWork.TicketRepository
                .GetEmployeeTickets(
                    x => !x.IsDeleted && x.TicketId == ticketId && x.ResolverId == _userContext.UserId &&
                         x.Status == TicketStatus.Pending, null, null)
                .FirstOrDefaultAsync(_);
            if (ticket is null) return NotFound("Ticket not found.");
            ticket.Status = TicketStatus.Processing;
            var res = _unitOfWork.TicketRepository.UpdateEmployeeTicket(ticket);
            await _unitOfWork.SaveChangesAsync(_);

            //send email customer
            var customerEmailTemplate = await _emailService.GetEmailTemplate(EmailTemplateType.TicketProcessing, _);
            var subject = customerEmailTemplate!.Subject.Replace("[[TicketId]]",
                res?.TicketId.ToString() ?? string.Empty);
            var customerReplacements = new Dictionary<string, string>
            {
                { "CreatedAt", $"{ticket.CreatedAt:D} " },
                { "TicketId", ticket.TicketId.ToString() },
                { "RequestorFullName", ticket.Requestor.FullName ?? string.Empty },
                { "ResolverFullName", ticket.Resolver?.FullName ?? string.Empty },
            };
            var bodyBuilder = new StringBuilder(customerEmailTemplate.Body);
            foreach (var pair in customerReplacements)
            {
                bodyBuilder.Replace($"[[{pair.Key}]]", pair.Value);
            }

            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Requestor not found");

            await _emailService.Send(subject, bodyBuilder.ToString(), res!.Requestor.Email, null, null, _);
            //end of send email customer

            var result = _mapper.Map<EmployeeTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest("Unexpected error occured.");
        }
    }

    [Authorize(Roles = $"{RoleName.Hr},{RoleName.HrManager}")]
    [HttpPut("Complete/{ticketId:guid}")]
    public async Task<ActionResult<EmployeeTicketResponse>> CompleteEmployeeTicket(
        [FromRoute] Guid ticketId,
        CancellationToken _)
    {
        try
        {
            var ticket = await _unitOfWork.TicketRepository
                .GetEmployeeTickets(
                    x => !x.IsDeleted && x.TicketId == ticketId && x.ResolverId == _userContext.UserId &&
                         x.Status == TicketStatus.Processing, null, null)
                .FirstOrDefaultAsync(_);
            if (ticket is null) return NotFound("Ticket not found.");
            ticket.Status = TicketStatus.Done;
            var res = _unitOfWork.TicketRepository.UpdateEmployeeTicket(ticket);
            await _unitOfWork.SaveChangesAsync(_);

            //send email customer
            var customerEmailTemplate = await _emailService.GetEmailTemplate(EmailTemplateType.TicketDone, _);
            var subject = customerEmailTemplate!.Subject.Replace("[[TicketId]]",
                res?.TicketId.ToString() ?? string.Empty);
            var customerReplacements = new Dictionary<string, string>
            {
                { "CreatedAt", $"{ticket.CreatedAt:D} " },
                { "TicketId", ticket.TicketId.ToString() },
                { "RequestorFullName", ticket.Requestor.FullName ?? string.Empty },
                { "ReviewTicketUrl", "url" },
            };
            var bodyBuilder = new StringBuilder(customerEmailTemplate.Body);
            foreach (var pair in customerReplacements)
            {
                bodyBuilder.Replace($"[[{pair.Key}]]", pair.Value);
            }

            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Requestor not found");

            await _emailService.Send(subject, bodyBuilder.ToString(), res!.Requestor.Email, null, null, _);
            //end of send email customer

            var result = _mapper.Map<EmployeeTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest("Unexpected error occured.");
        }
    }

    [Authorize]
    [HttpPut("Reopen/{ticketId:guid}")]
    public async Task<ActionResult<EmployeeTicketResponse>> ReopenEmployeeTickets([FromRoute] Guid ticketId,
        ReopenEmployeeTicketRequest request,
        CancellationToken _)
    {
        try
        {
            var ticket = await _unitOfWork.TicketRepository.GetEmployeeTickets(x =>
                    x.TicketId == ticketId &&
                    (x.Status == TicketStatus.Closed || x.Status == TicketStatus.Done), null, null)
                .FirstOrDefaultAsync(_);
            if (ticket is null) return NotFound("Ticket not found.");

            ticket.Status = TicketStatus.Pending;
            ticket.ReopenReasons.Add(request.Content);
            var res = _unitOfWork.TicketRepository.UpdateEmployeeTicket(ticket);
            await _unitOfWork.SaveChangesAsync(_);

            //send email customer
            var customerEmailTemplate =
                await _emailService.GetEmailTemplate(EmailTemplateType.TicketReopenedRequestor, _);
            var subject = customerEmailTemplate!.Subject.Replace("[[TicketId]]",
                res?.TicketId.ToString() ?? string.Empty);
            var customerReplacements = new Dictionary<string, string>
            {
            };
            var bodyBuilder = new StringBuilder(customerEmailTemplate.Body);
            foreach (var pair in customerReplacements)
            {
                bodyBuilder.Replace($"[[{pair.Key}]]", pair.Value);
            }

            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Requestor not found");

            await _emailService.Send(subject, bodyBuilder.ToString(), res!.Requestor.Email, null, null, _);
            //end of send email customer
            //
            //send email employee
            var employeeEmailTemplate =
                await _emailService.GetEmailTemplate(EmailTemplateType.TicketReopenedResolver, _);
            var employeeEmailSubject = employeeEmailTemplate!.Subject.Replace("[[TicketId]]",
                res?.TicketId.ToString() ?? string.Empty);
            var employeeReplacements = new Dictionary<string, string>
            {
            };
            var employeeEmailBodyBuilder = new StringBuilder(employeeEmailTemplate.Body);
            foreach (var pair in employeeReplacements)
            {
                bodyBuilder.Replace($"[[{pair.Key}]]", pair.Value);
            }

            if (string.IsNullOrEmpty(res?.Resolver?.Email) ||
                !Helper.ValidateEmailString(res?.Resolver?.Email ?? string.Empty))
                return NotFound("Resolver not found");

            await _emailService.Send(employeeEmailSubject, bodyBuilder.ToString(), res!.Resolver!.Email, null, null, _);
            //end of send email employee

            var result = _mapper.Map<EmployeeTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest("Unexpected error occured.");
        }
    }

    [Authorize]
    [HttpPut("Close/{ticketId:guid}")]
    public async Task<ActionResult<EmployeeTicketResponse>> CloseEmployeeTicket(
        [FromRoute] Guid ticketId,
        CancellationToken _)
    {
        try
        {
            var ticket = await _unitOfWork.TicketRepository
                .GetEmployeeTickets(
                    x => !x.IsDeleted && x.TicketId == ticketId &&
                         x.Status == TicketStatus.Done, null, null)
                .FirstOrDefaultAsync(_);
            if (ticket is null) return NotFound("Ticket not found.");

            ticket.Status = TicketStatus.Closed;
            var res = _unitOfWork.TicketRepository.UpdateEmployeeTicket(ticket);
            await _unitOfWork.SaveChangesAsync(_);

            //send email customer
            var customerEmailTemplate = await _emailService.GetEmailTemplate(EmailTemplateType.TicketClosed, _);
            var subject = customerEmailTemplate!.Subject.Replace("[[TicketId]]",
                res?.TicketId.ToString() ?? string.Empty);
            var customerReplacements = new Dictionary<string, string>
            {
                { "CreatedAt", $"{ticket.CreatedAt:D} " },
                { "TicketId", ticket.TicketId.ToString() },
                { "CustomerFullName", ticket.Requestor.FullName ?? string.Empty },
                { "CreateTicketUrl", "url" },
                { "ViewTicketUrl", "url" },
            };
            var bodyBuilder = new StringBuilder(customerEmailTemplate.Body);
            foreach (var pair in customerReplacements)
            {
                bodyBuilder.Replace($"[[{pair.Key}]]", pair.Value);
            }

            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Requestor not found");

            await _emailService.Send(subject, bodyBuilder.ToString(), res!.Requestor.Email, null, null, _);
            //end of send email customer

            var result = _mapper.Map<EmployeeTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest("Unexpected error occured.");
        }
    }
}