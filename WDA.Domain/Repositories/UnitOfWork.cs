using Microsoft.AspNetCore.Identity;
using WDA.Domain.Models.Attachment;
using WDA.Domain.Models.Customer;
using WDA.Domain.Models.Transaction;
using WDA.Domain.Models.User;

namespace WDA.Domain.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(AppDbContext dbContext, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private IBaseRepository<Customer>? _customerRepository;
    private IBaseRepository<Transaction>? _transactionRepository;
    private IBaseRepository<Attachment>? _attachmentRepository;
    private AdminRepository _adminRepository;
    private TicketRepository _ticketRepository;

    public IBaseRepository<Customer> CustomerRepository
    {
        get
        {
            if (_customerRepository is null)
            {
                _customerRepository = new CustomerRepository(_dbContext);
            }
            return _customerRepository;
        }
    }

    public IBaseRepository<Transaction> TransactionRepository
    {
        get
        {
            if (_transactionRepository is null)
            {
                _transactionRepository = new TransactionRepository(_dbContext);
            }
            return _transactionRepository;
        }
    }
    
    public IBaseRepository<Attachment> AttachmentRepository
    {
        get
        {
            if (_attachmentRepository is null)
            {
                _attachmentRepository = new AttachmentRepository(_dbContext);
            }
            return _attachmentRepository;
        }
    }
    
    public AdminRepository AdminRepository
    {
        get
        {
            if (_adminRepository is null)
            {
                _adminRepository = new AdminRepository(_dbContext, _userManager);
            }
            return _adminRepository;
        }
    }

    public TicketRepository TicketRepository
    {
        get
        {
            if (_ticketRepository is null)
            {
                _ticketRepository = new TicketRepository(_dbContext, _userManager);
            }
            return _ticketRepository;
        }
    }
    public async Task<bool> SaveChangesAsync(CancellationToken _ = default)
    {
        return await _dbContext.SaveChangesAsync(_) > 0;
    }
}
