using System.Net;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WDA.Api.Dto.Customer.Request;
using WDA.Api.Dto.Customer.Response;
using WDA.Api.Dto.Transaction.Request;
using WDA.Api.Dto.Transaction.Response;
using WDA.Domain.Models.Email;
using WDA.Domain.Repositories;
using WDA.Service.Email;
using WDA.Shared;
using IAuthorizationService = WDA.Service.User.IAuthorizationService;

namespace WDA.Api.Controllers.Transaction;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly UserContext _userContext;
    private readonly UserManager<Domain.Models.User.User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly IAuthorizationService _authorizationService;

    public TransactionController(UserContext userContext, UserManager<Domain.Models.User.User> userManager,
        IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, IAuthorizationService authorizationService)
    {
        _userContext = userContext;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailService = emailService;
        _authorizationService = authorizationService;
    }

    [HttpGet]
    public ActionResult<IQueryable<TransactionResponse>> GetTransactions(CancellationToken _)
    {
        var customers = _unitOfWork.TransactionRepository.Get(x => !x.IsDelete).Include(x => x.Customer)
            .Select(x => _mapper.Map<TransactionResponse>(x));
        return Ok(customers);
    }

    [HttpPost]
    public async Task<ActionResult<TransactionResponse?>> CreateTransaction(CreateTransactionRequest request,
        CancellationToken _)
    {
        try
        {
            var transaction = _mapper.Map<Domain.Models.Transaction.Transaction>(request);
            //get user
            var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString());
            //get customer
            var customer = await _unitOfWork.CustomerRepository.GetById(request.CustomerId, _);
            if (customer is null)
                throw new HttpException("Transaction Created Failed. Customer Not Found", HttpStatusCode.BadRequest);
            //map and create transaction
            transaction.CreatedBy = user;
            transaction.ModifiedBy = user;
            transaction.Customer = customer;
            var transactionRes = await _unitOfWork.TransactionRepository.Create(transaction, _);
            await _unitOfWork.SaveChangesAsync(_);

            //send email
            if (string.IsNullOrEmpty(customer.Email) ||
                !Helper.ValidateEmailString(customer.Email))
                return NotFound("Customer email not found");
            var validationToken = _authorizationService.IssueTemporaryToken(7, _);
            var subjectReplacements = new Dictionary<string, string>
            {
                { "TransactionId", transactionRes?.TransactionId.ToString() ?? string.Empty }
            };
            var bodyReplacements = new Dictionary<string, string>
            {
                { "CreatedAt", $"{transactionRes.CreatedAt!.Value:D} " },
                { "TransactionId", transactionRes.TransactionId.ToString() },
                { "CustomerFullName", transactionRes.Customer.Name },
                { "CreateTicketUrl", $"{AppSettings.Instance.ClientConfiguration.CreateTicketBaseUrl}?customerId={customer.CustomerId}&validationToken={validationToken.Token}" }
            };
            await _emailService.SendEmailNotification(EmailTemplateType.TransactionCompleted, customer.Email,
                subjectReplacements, bodyReplacements, _: _);

            //end of send email

            var result = _mapper.Map<TransactionResponse>(transactionRes);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IQueryable<TransactionResponse>>> GetTransactionById([FromRoute] Guid id,
        CancellationToken _)
    {
        var transaction = await _unitOfWork.TransactionRepository.GetById(id, _);
        if (transaction is null) return NotFound();
        var res = _mapper.Map<TransactionResponse>(transaction);
        return Ok(res);
    }

    [HttpGet("Customer/{customerId:guid}")]
    public ActionResult<IQueryable<TransactionResponse>> GetTransactionsByCustomerId([FromRoute] Guid customerId,
        CancellationToken _)
    {
        var transactions = _unitOfWork.TransactionRepository
            .Get(x => !x.IsDelete && x.Customer.CustomerId == customerId).Include(x => x.Customer)
            .Select(x => _mapper.Map<TransactionResponse>(x));
        return Ok(transactions);
    }
}