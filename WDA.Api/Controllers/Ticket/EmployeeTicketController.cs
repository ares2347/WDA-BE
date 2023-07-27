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
    [HttpGet("{ticketId:guid}")]
    public async Task<ActionResult<EmployeeTicketResponse>> GetEmployeeTickets([FromRoute] Guid ticketId,
        CancellationToken _)
    {
        var res = await _unitOfWork.TicketRepository.GetEmployeeTickets(
            x => !x.IsDeleted && x.TicketId == ticketId, 10, 0).FirstOrDefaultAsync(_);
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

    [Authorize]
    [HttpPost("Create")]
    public async Task<ActionResult<EmployeeTicketResponse>> CreateEmployeeTickets(CreateEmployeeTicketRequest request,
        CancellationToken _)
    {
        try
        {
            //create customer ticket
            var ticket = _mapper.Map<EmployeeTicket>(request);
            var requestor = await _userManager.FindByIdAsync(request.EmployeeId.ToString());
            if (requestor is null) return NotFound("Employee not found");
            ticket.Requestor = requestor;
            ticket.Status = TicketStatus.Opened;
            var res = _unitOfWork.TicketRepository.CreateEmployeeTicket(ticket);
            await _unitOfWork.SaveChangesAsync(_);

            //send email
            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Customer not found");

            var subjectReplacements = new Dictionary<string, string>
            {
                { "TicketId", res?.TicketId.ToString() ?? string.Empty }
            };
            var bodyReplacements = new Dictionary<string, string>
            {
                { "CreatedAt", $"{ticket.CreatedAt:D} " },
                { "TicketId", ticket.TicketId.ToString() },
                { "RequestorFullName", ticket.Requestor.FullName ?? ticket.Requestor.Email ?? string.Empty },
            };
            await _emailService.SendEmailNotification(EmailTemplateType.TicketOpened, res!.Requestor.Email,
                subjectReplacements, bodyReplacements, _: _);
            //end of send email

            var result = _mapper.Map<EmployeeTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Unexpected error occured.{e.Message}");
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
                    x => !x.IsDeleted && x.TicketId == request.TicketId && x.Status == TicketStatus.Opened, null,
                    null)
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
            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Customer not found");

            var customerSubjectReplacements = new Dictionary<string, string>
            {
                { "TicketId", res?.TicketId.ToString() ?? string.Empty }
            };
            var customerBodyReplacements = new Dictionary<string, string>
            {
                { "CreatedAt", $"{ticket.CreatedAt:D} " },
                { "TicketId", ticket.TicketId.ToString() },
                { "RequestorFullName", ticket.Requestor.FullName ?? string.Empty },
                { "ResolverFullName", ticket.Resolver?.FullName ?? string.Empty },
            };
            await _emailService.SendEmailNotification(EmailTemplateType.TicketPending, res!.Requestor.Email,
                customerSubjectReplacements, customerBodyReplacements, _: _);

            //end of send email customer
            //
            //send email employee
            if (string.IsNullOrEmpty(res?.Resolver?.Email) ||
                !Helper.ValidateEmailString(res?.Resolver.Email ?? string.Empty))
                return NotFound("Customer not found");

            var employeeSubjectReplacements = new Dictionary<string, string>
            {
                { "TicketId", res?.TicketId.ToString() ?? string.Empty }
            };
            var employeeBodyReplacements = new Dictionary<string, string>
            {
            };
            await _emailService.SendEmailNotification(EmailTemplateType.TicketAssigned, res!.Resolver.Email,
                customerSubjectReplacements, customerBodyReplacements, _: _);

            //end of send email employee
            var result = _mapper.Map<EmployeeTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Unexpected error occured.{e.Message}");
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
            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Customer email not found");

            var subjectReplacements = new Dictionary<string, string>
            {
                { "TicketId", res?.TicketId.ToString() ?? string.Empty }
            };
            var bodyReplacements = new Dictionary<string, string>
            {
                { "CreatedAt", $"{ticket.CreatedAt:D} " },
                { "TicketId", ticket.TicketId.ToString() },
                { "RequestorFullName", ticket.Requestor.FullName ?? string.Empty },
                { "ResolverFullName", ticket.Resolver?.FullName ?? string.Empty },
            };
            await _emailService.SendEmailNotification(EmailTemplateType.TicketProcessing, res!.Requestor.Email,
                subjectReplacements, bodyReplacements, _: _);
            //end of send email customer

            var result = _mapper.Map<EmployeeTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Unexpected error occured.{e.Message}");
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
            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Customer email not found");

            var subjectReplacements = new Dictionary<string, string>
            {
                { "TicketId", res?.TicketId.ToString() ?? string.Empty }
            };
            var bodyReplacements = new Dictionary<string, string>
            {
                { "CreatedAt", $"{ticket.CreatedAt:D} " },
                { "TicketId", ticket.TicketId.ToString() },
                { "RequestorFullName", ticket.Requestor.FullName ?? string.Empty },
                { "ReviewTicketUrl", $"{AppSettings.Instance.ClientConfiguration.CloseTicketBaseUrl}?ticketId={ticket.TicketId}" },
            };
            await _emailService.SendEmailNotification(EmailTemplateType.TicketDone, res!.Requestor.Email,
                subjectReplacements, bodyReplacements, _: _);
            //end of send email customer

            var result = _mapper.Map<EmployeeTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Unexpected error occured.{e.Message}");
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
            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Customer not found");

            var customerSubjectReplacements = new Dictionary<string, string>
            {
                { "TicketId", res?.TicketId.ToString() ?? string.Empty }
            };
            var customerBodyReplacements = new Dictionary<string, string>
            {
            };
            await _emailService.SendEmailNotification(EmailTemplateType.TicketReopenedRequestor,
                res!.Requestor.Email, customerSubjectReplacements, customerBodyReplacements, _: _);

            //end of send email customer
            //
            //send email employee
            if (string.IsNullOrEmpty(res?.Resolver?.Email) ||
                !Helper.ValidateEmailString(res?.Resolver.Email ?? string.Empty))
                return NotFound("Employee email not found");

            var employeeSubjectReplacements = new Dictionary<string, string>
            {
                { "TicketId", res?.TicketId.ToString() ?? string.Empty }
            };
            var employeeBodyReplacements = new Dictionary<string, string>
            {
            };
            await _emailService.SendEmailNotification(EmailTemplateType.TicketReopenedResolver, res!.Resolver.Email,
                customerSubjectReplacements, customerBodyReplacements, _: _);


            //end of send email employee

            var result = _mapper.Map<EmployeeTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Unexpected error occured.{e.Message}");
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
            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Customer email not found");

            var subjectReplacements = new Dictionary<string, string>
            {
                { "TicketId", res?.TicketId.ToString() ?? string.Empty }
            };
            var bodyReplacements = new Dictionary<string, string>
            {
                { "CreatedAt", $"{ticket.CreatedAt:D} " },
                { "TicketId", ticket.TicketId.ToString() },
                { "CustomerFullName", ticket.Requestor.FullName ?? string.Empty },
                { "CreateTicketUrl", $"{AppSettings.Instance.ClientConfiguration.SiteBaseUrl}" },
                { "ViewTicketUrl", $"{AppSettings.Instance.ClientConfiguration.CloseTicketBaseUrl}?ticketId={ticket.TicketId}" },
            };
            await _emailService.SendEmailNotification(EmailTemplateType.TicketClosed, res!.Requestor.Email,
                subjectReplacements, bodyReplacements, _: _);
            //end of send email customer

            var result = _mapper.Map<EmployeeTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Unexpected error occured.{e.Message}");
        }
    }
}