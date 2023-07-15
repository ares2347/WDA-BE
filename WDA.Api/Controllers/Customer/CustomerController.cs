using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WDA.Api.Dto.Customer.Request;
using WDA.Api.Dto.Customer.Response;
using WDA.Domain.Models.User;
using WDA.Domain.Repositories;
using WDA.Shared;

namespace WDA.Api.Controllers.Customer;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomerController : ControllerBase
{
    private readonly UserContext _userContext;
    private readonly UserManager<Domain.Models.User.User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CustomerController(UserContext userContext, UserManager<Domain.Models.User.User> userManager, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userContext = userContext;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IQueryable<CustomerResponse>> GetCustomers(string? name, int page, int size, CancellationToken _)
    {
        var customers = _unitOfWork.CustomerRepository.Get(x=> x.Name.Contains(name ?? string.Empty), size, page).Select(x => _mapper.Map<CustomerResponse>(x));
        return Ok(customers);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse?>> CreateCustomer(CreateCustomerRequest request, CancellationToken _)
    {
        try
        {
            if (!Helper.ValidateEmailString(request.Email))
                throw new HttpException("Invalid email", System.Net.HttpStatusCode.BadRequest);
            var newCustomer = _mapper.Map<Domain.Models.Customer.Customer>(request);
            var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString());
            newCustomer.CreatedBy = user;
            newCustomer.ModifiedBy = user;
            var customer = await _unitOfWork.CustomerRepository.Create(newCustomer, _);
            await _unitOfWork.SaveChangesAsync(_);
            var result = _mapper.Map<CustomerResponse>(customer);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IQueryable<CustomerResponse>>> GetCustomerById([FromRoute] Guid id, CancellationToken _)
    {
        var customer = await _unitOfWork.CustomerRepository.GetById(id, _);
        if (customer is null) return NotFound();
        var res = _mapper.Map<CustomerResponse>(customer);
        return Ok(res);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerResponse?>> UpdateCustomer([FromRoute] Guid id, UpdateCustomerRequest request, CancellationToken _)
    {
        try
        {
            if (!Helper.ValidateEmailString(request.Email))
                throw new HttpException("Invalid email", System.Net.HttpStatusCode.BadRequest);

            var customer = _mapper.Map<Domain.Models.Customer.Customer>(request);
            var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString());
            customer.CustomerId = id;
            customer.ModifiedBy = user;
            customer.ModifiedAt = DateTimeOffset.UtcNow;
            var updatedCustomer = await _unitOfWork.CustomerRepository.Update(customer, _);
            await _unitOfWork.SaveChangesAsync(_);
            if (updatedCustomer is null)
                return NotFound();
            var result = _mapper.Map<CustomerResponse>(updatedCustomer);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<CustomerResponse?>> DeleteCustomer([FromRoute] Guid id, CancellationToken _)
    {
        try
        {
            var res = await _unitOfWork.CustomerRepository.Delete(id, cancellationToken: _);
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
