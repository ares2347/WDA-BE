using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WDA.Api.Dto.CustomerTicket;
using WDA.Domain;
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
public class CustomerTicketController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly UserContext _userContext;
    private readonly UserManager<Domain.Models.User.User> _userManager;
    private readonly IAuthorizationService _authorizationService;

    public CustomerTicketController(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService,
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

    [HttpGet]
    public ActionResult<IQueryable<CustomerTicketResponse>> GetCustomerTickets(TicketStatus? status,
        CancellationToken _, int page = 0,
        int size = 10)
    {
        var res = _unitOfWork.TicketRepository.GetCustomerTickets(x => !x.IsDeleted, size, page);
        if (status is not null)
        {
            res = res.Where(x => x.Status == status);
        }

        return Ok(res);
    }

    [Authorize]
    [HttpGet("AssignedTickets")]
    public ActionResult<IQueryable<CustomerTicketResponse>> GetAssignedTickets(CancellationToken _, int page = 0,
        int size = 10)
    {
        var res = _unitOfWork.TicketRepository.GetCustomerTickets(
            x => !x.IsDeleted && x.ResolverId == _userContext.UserId, size, page);
        return Ok(res);
    }

    [Authorize]
    [HttpGet("TicketsByCustomer/{customerId:guid}")]
    public ActionResult<IQueryable<CustomerTicketResponse>> GetMyTickets([FromRoute] Guid customerId, CancellationToken _, int page = 0,
        int size = 10)
    {
        var res = _unitOfWork.TicketRepository.GetCustomerTickets(
            x => !x.IsDeleted && x.RequestorId == customerId, size, page);
        return Ok(res);
    }

    [HttpPost("Create")]
    public async Task<ActionResult<CustomerTicketResponse>> CreateCustomerTickets(CreateCustomerTicketRequest request,
        CancellationToken _)
    {
        try
        {
            _authorizationService.ValidateToken(request.ValidationToken);
            //create customer ticket
            var terst = await _unitOfWork.TicketRepository.GetCustomerTickets(x =>
                    x.Requestor.CustomerId == request.CustomerId && x.Status != TicketStatus.Closed, null, null)
                .ToListAsync(_);
            var isNotSubmittable = await _unitOfWork.TicketRepository.GetCustomerTickets(x =>
                    x.Requestor.CustomerId == request.CustomerId && x.Status != TicketStatus.Closed, null, null)
                .AnyAsync(_);
            if (isNotSubmittable) return BadRequest("Customer already has an unclosed ticket!");
            var ticket = _mapper.Map<CustomerTicket>(request);
            var customer = await _unitOfWork.CustomerRepository.GetById(request.CustomerId, _);
            if (customer is null) return NotFound("Customer not found");
            ticket.Requestor = customer;
            ticket.Status = TicketStatus.Opened;
            var res = _unitOfWork.TicketRepository.CreateCustomerTicket(ticket);
            await _unitOfWork.SaveChangesAsync(_);

            //send email
            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Customer not found");
            
            var subjectReplacements = new Dictionary<string, string>
            {
                {"TicketId", res?.TicketId.ToString() ?? string.Empty}
            };
            var bodyReplacements = new Dictionary<string, string>
            {
                { "CreatedAt", $"{ticket.CreatedAt:D} " },
                { "TicketId", ticket.TicketId.ToString() },
                { "RequestorFullName", ticket.Requestor.Name },
            };
           await _emailService.SendEmailNotification(EmailTemplateType.TicketOpened,res!.Requestor.Email, subjectReplacements, bodyReplacements, _: _);
            //end of send email

            var result = _mapper.Map<CustomerTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Unexpected error occured.{e.Message}.{AppSettings.Instance.Smtp.Email}.{AppSettings.Instance.Smtp.From}");
        }
    }

    [Authorize]
    [HttpPut("Assign")]
    public async Task<ActionResult<CustomerTicketResponse>> AssignCustomerTicket(AssignCustomerTicketRequest request,
        CancellationToken _)
    {
        try
        {
            var ticket = await _unitOfWork.TicketRepository
                .GetCustomerTickets(
                    x => !x.IsDeleted && x.TicketId == request.TicketId && x.Status == TicketStatus.Opened, null, null)
                .FirstOrDefaultAsync(_);
            if (ticket is null) return NotFound("Ticket not found.");
            if (!_userContext.Roles.Contains(RoleName.SaleManager) && request.EmployeeId is not null &&
                _userContext.UserId != request.EmployeeId)
                return Unauthorized("Unauthorized to perform action.");
            var resolver =
                await _userManager.FindByIdAsync(request.EmployeeId?.ToString() ?? _userContext.UserId.ToString());
            ticket.Resolver = resolver;
            ticket.Status = TicketStatus.Pending;
            var res = _unitOfWork.TicketRepository.UpdateCustomerTicket(ticket);
            await _unitOfWork.SaveChangesAsync(_);

            //send email customer
            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Customer not found");
            
            var customerSubjectReplacements = new Dictionary<string, string>
            {
                {"TicketId", res?.TicketId.ToString() ?? string.Empty}
            };
            var customerBodyReplacements = new Dictionary<string, string>
            {
                { "CreatedAt", $"{ticket.CreatedAt:D} " },
                { "TicketId", ticket.TicketId.ToString() },
                { "RequestorFullName", ticket.Requestor.Name },
                { "ResolverFullName", ticket.Resolver?.FullName ?? string.Empty },
            };
            await _emailService.SendEmailNotification(EmailTemplateType.TicketPending,res!.Requestor.Email, customerSubjectReplacements, customerBodyReplacements, _: _);
            
            //end of send email customer
            //
            //send email employee
            if (string.IsNullOrEmpty(res?.Resolver?.Email) ||
                !Helper.ValidateEmailString(res?.Resolver.Email ?? string.Empty))
                return NotFound("Customer not found");
            
            var employeeSubjectReplacements = new Dictionary<string, string>
            {
                {"TicketId", res?.TicketId.ToString() ?? string.Empty}
            };
            var employeeBodyReplacements = new Dictionary<string, string>
            {
            };
            await _emailService.SendEmailNotification(EmailTemplateType.TicketAssigned,res!.Resolver.Email, customerSubjectReplacements, customerBodyReplacements, _: _);
            //end of send email employee
            var result = _mapper.Map<CustomerTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Unexpected error occured.{e.Message}.{AppSettings.Instance.Smtp.Email}.{AppSettings.Instance.Smtp.From}");
        }
    }

    [Authorize]
    [HttpPut("Processing")]
    public async Task<ActionResult<CustomerTicketResponse>> ProcessingCustomerTicket(
        ProcessingCustomerTicketRequest request,
        CancellationToken _)
    {
        try
        {
            var ticket = await _unitOfWork.TicketRepository
                .GetCustomerTickets(
                    x => !x.IsDeleted && x.TicketId == request.TicketId && x.ResolverId == _userContext.UserId &&
                         x.Status == TicketStatus.Pending, null, null)
                .FirstOrDefaultAsync(_);
            if (ticket is null) return NotFound("Ticket not found.");
            ticket.Status = TicketStatus.Processing;
            var res = _unitOfWork.TicketRepository.UpdateCustomerTicket(ticket);
            await _unitOfWork.SaveChangesAsync(_);

            //send email customer
            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Customer email not found");
            
            var subjectReplacements = new Dictionary<string, string>
            {
                {"TicketId", res?.TicketId.ToString() ?? string.Empty}
            };
            var bodyReplacements = new Dictionary<string, string>
            {
                { "CreatedAt", $"{ticket.CreatedAt:D} " },
                { "TicketId", ticket.TicketId.ToString() },
                { "RequestorFullName", ticket.Requestor.Name },
                { "ResolverFullName", ticket.Resolver?.FullName ?? string.Empty },
            };
            await _emailService.SendEmailNotification(EmailTemplateType.TicketProcessing,res!.Requestor.Email, subjectReplacements, bodyReplacements, _: _);
            //end of send email customer

            var result = _mapper.Map<CustomerTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Unexpected error occured.{e.Message}.{AppSettings.Instance.Smtp.Email}.{AppSettings.Instance.Smtp.From}");
        }
    }

    [Authorize]
    [HttpPut("Complete")]
    public async Task<ActionResult<CustomerTicketResponse>> CompleteCustomerTicket(
        CompleteCustomerTicketRequest request,
        CancellationToken _)
    {
        try
        {
            var ticket = await _unitOfWork.TicketRepository
                .GetCustomerTickets(
                    x => !x.IsDeleted && x.TicketId == request.TicketId && x.ResolverId == _userContext.UserId &&
                         x.Status == TicketStatus.Processing, null, null)
                .FirstOrDefaultAsync(_);
            if (ticket is null) return NotFound("Ticket not found.");
            ticket.Status = TicketStatus.Done;
            var res = _unitOfWork.TicketRepository.UpdateCustomerTicket(ticket);
            await _unitOfWork.SaveChangesAsync(_);

            //send email customer
            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Customer email not found");
            var validationToken = _authorizationService.IssueTemporaryToken(7, _);
            var subjectReplacements = new Dictionary<string, string>
            {
                {"TicketId", res?.TicketId.ToString() ?? string.Empty}
            };
            var bodyReplacements = new Dictionary<string, string>
            {
                { "CreatedAt", $"{ticket.CreatedAt:D} " },
                { "TicketId", ticket.TicketId.ToString() },
                { "RequestorFullName", ticket.Requestor.Name },
                { "ReviewTicketUrl", $"{AppSettings.Instance.ClientConfiguration.CloseTicketBaseUrl}?ticketId={ticket.TicketId}&validationToken={validationToken.Token}" },
            };
            await _emailService.SendEmailNotification(EmailTemplateType.TicketDone,res!.Requestor.Email, subjectReplacements, bodyReplacements, _: _);
            //end of send email customer

            var result = _mapper.Map<CustomerTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Unexpected error occured.{e.Message}.{AppSettings.Instance.Smtp.Email}.{AppSettings.Instance.Smtp.From}");
        }
    }

    [HttpPut("Reopen")]
    public async Task<ActionResult<CustomerTicketResponse>> ReopenCustomerTickets(ReopenCustomerTicketRequest request,
        CancellationToken _)
    {
        try
        {
            var ticket = await _unitOfWork.TicketRepository.GetCustomerTickets(x =>
                    x.TicketId == request.TicketId &&
                    (x.Status == TicketStatus.Closed || x.Status == TicketStatus.Done), null, null)
                .FirstOrDefaultAsync(_);
            if (ticket is null) return NotFound("Ticket not found.");

            if (request.ValidationToken is null)
            {
                if (_userContext.UserId != ticket.ResolverId) return Unauthorized("Unauthorized to perform action.");
            }

            if (request.ValidationToken is not null)
            {
                _authorizationService.ValidateToken(request.ValidationToken);
            }
            //

            ticket.Status = TicketStatus.Pending;
            ticket.ReopenReasons.Add(request.Content);
            var res = _unitOfWork.TicketRepository.UpdateCustomerTicket(ticket);
            await _unitOfWork.SaveChangesAsync(_);

            //send email customer
            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Customer not found");
            
            var customerSubjectReplacements = new Dictionary<string, string>
            {
                {"TicketId", res?.TicketId.ToString() ?? string.Empty}
            };
            var customerBodyReplacements = new Dictionary<string, string>
            {
            };
            await _emailService.SendEmailNotification(EmailTemplateType.TicketReopenedRequestor,res!.Requestor.Email, customerSubjectReplacements, customerBodyReplacements, _: _);
            //end of send email customer
            //
            //send email employee
            if (string.IsNullOrEmpty(res?.Resolver?.Email) ||
                !Helper.ValidateEmailString(res?.Resolver.Email ?? string.Empty))
                return NotFound("Employee email not found");
            
            var employeeSubjectReplacements = new Dictionary<string, string>
            {
                {"TicketId", res?.TicketId.ToString() ?? string.Empty}
            };
            var employeeBodyReplacements = new Dictionary<string, string>
            {
            };
            await _emailService.SendEmailNotification(EmailTemplateType.TicketReopenedResolver,res!.Resolver.Email, customerSubjectReplacements, customerBodyReplacements, _: _);
            
            //end of send email employee

            var result = _mapper.Map<CustomerTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Unexpected error occured.{e.Message}.{AppSettings.Instance.Smtp.Email}.{AppSettings.Instance.Smtp.From}");
        }
    }

    [HttpPut("Close")]
    public async Task<ActionResult<CustomerTicketResponse>> CloseCustomerTicket(
        CloseCustomerTicketRequest request,
        CancellationToken _)
    {
        try
        {
            var ticket = await _unitOfWork.TicketRepository
                .GetCustomerTickets(
                    x => !x.IsDeleted && x.TicketId == request.TicketId &&
                         x.Status == TicketStatus.Done, null, null)
                .FirstOrDefaultAsync(_);
            if (ticket is null) return NotFound("Ticket not found.");
            if (request.ValidationToken is null)
            {
                if (_userContext.UserId != ticket.ResolverId) return Unauthorized("Unauthorized to perform action.");
            }

            if (request.ValidationToken is not null)
            {
                _authorizationService.ValidateToken(request.ValidationToken);
            }

            //
            ticket.Status = TicketStatus.Closed;
            var res = _unitOfWork.TicketRepository.UpdateCustomerTicket(ticket);
            await _unitOfWork.SaveChangesAsync(_);

            //send email customer
            if (string.IsNullOrEmpty(res?.Requestor.Email) ||
                !Helper.ValidateEmailString(res?.Requestor.Email ?? string.Empty))
                return NotFound("Customer email not found");
            var validationToken = _authorizationService.IssueTemporaryToken(7, _);
            var subjectReplacements = new Dictionary<string, string>
            {
                {"TicketId", res?.TicketId.ToString() ?? string.Empty}
            };
            var bodyReplacements = new Dictionary<string, string>
            {
                { "CreatedAt", $"{ticket.CreatedAt:D} " },
                { "TicketId", ticket.TicketId.ToString() },
                { "CustomerFullName", ticket.Requestor.Name },
                { "CreateTicketUrl", $"{AppSettings.Instance.ClientConfiguration.CreateTicketBaseUrl}?customerId={ticket.Requestor.CustomerId}&validationToken={validationToken.Token}" },
                { "ViewTicketUrl", $"{AppSettings.Instance.ClientConfiguration.CloseTicketBaseUrl}?ticketId={ticket.TicketId}&validationToken={validationToken.Token}"},
            };
            await _emailService.SendEmailNotification(EmailTemplateType.TicketClosed,res!.Requestor.Email, subjectReplacements, bodyReplacements, _: _);
            //end of send email customer

            var result = _mapper.Map<CustomerTicketResponse>(res);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Unexpected error occured.{e.Message}.{AppSettings.Instance.Smtp.Email}.{AppSettings.Instance.Smtp.From}");
        }
    }
}