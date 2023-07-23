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

    public TransactionController(UserContext userContext, UserManager<Domain.Models.User.User> userManager,
        IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
    {
        _userContext = userContext;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailService = emailService;
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
            var emailTemplate = await _emailService.GetEmailTemplate(EmailTemplateType.TransactionCompleted, _);
            var subject = emailTemplate!.Subject.Replace("[[TransactionId]]",
                transactionRes?.TransactionId.ToString() ?? string.Empty);
            var replacements = new Dictionary<string, string>
            {
            };
            var bodyBuilder = new StringBuilder(emailTemplate.Body);
            foreach (var pair in replacements)
            {
                bodyBuilder.Replace($"[[{pair.Key}]]", pair.Value);
            }

            await _emailService.Send(subject, bodyBuilder.ToString(), customer.Email, null, null, _);
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