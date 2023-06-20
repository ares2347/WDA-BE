using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WDA.Api.Dto.Customer.Request;
using WDA.Api.Dto.Customer.Response;
using WDA.Api.Dto.Transaction.Request;
using WDA.Api.Dto.Transaction.Response;
using WDA.Domain.Repositories;
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

    public TransactionController(UserContext userContext, UserManager<Domain.Models.User.User> userManager, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userContext = userContext;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IQueryable<TransactionResponse>> GetTransactions(CancellationToken _)
    {
        var customers = _unitOfWork.TransactionRepository.Get().Select(x => _mapper.Map<TransactionResponse>(x));
        return Ok(customers);
    }

    [HttpPost]
    public async Task<ActionResult<TransactionResponse?>> CreateTransaction(CreateTransactionRequest request, CancellationToken _)
    {
        try
        {
            var transaction = _mapper.Map<Domain.Models.Transaction.Transaction>(request);
            var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString());
            var customer = await _unitOfWork.CustomerRepository.GetById(request.CustomerId, _);
            transaction.Total = transaction.SubTransactions.Sum(sub => sub.SubTotal);
            transaction.CreatedBy = user;
            transaction.ModifiedBy = user;
            transaction.Customer = customer;
            var transactionRes = await _unitOfWork.TransactionRepository.Create(transaction, _);
            await _unitOfWork.SaveChangesAsync(_);
            var result = _mapper.Map<TransactionResponse>(transactionRes);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IQueryable<TransactionResponse>>> GetTransactionById([FromRoute] Guid id, CancellationToken _)
    {
        var transaction = await _unitOfWork.TransactionRepository.GetById(id, _);
        if (transaction is null) return NotFound();
        var res = _mapper.Map<TransactionResponse>(transaction);
        return Ok(res);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<TransactionResponse?>> RollbackTransaction([FromRoute] Guid id, CancellationToken _)
    {
        try
        {
            var res = await _unitOfWork.TransactionRepository.Delete(id, cancellationToken: _);
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
