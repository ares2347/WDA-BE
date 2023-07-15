using Microsoft.AspNetCore.Identity;
using WDA.Domain.Models.Attachment;
using WDA.Domain.Models.Customer;
using WDA.Domain.Models.Document;
using WDA.Domain.Models.Feedback;
using WDA.Domain.Models.Thread;
using WDA.Domain.Models.Transaction;
using WDA.Domain.Models.User;
using WDA.Shared;
using Thread = WDA.Domain.Models.Thread.Thread;

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
    private IBaseRepository<Document>? _documentRepository;
    private IBaseRepository<Attachment>? _attachmentRepository;
    private IBaseRepository<Thread>? _threadRepository;
    private IBaseRepository<Reply>? _replyRepository;
    private IBaseRepository<Feedback>? _feedbackRepository;
    private AdminRepository _adminRepository;

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

    public IBaseRepository<Document> DocumentRepository
    {
        get
        {
            if (_documentRepository is null)
            {
                _documentRepository = new DocumentRepository(_dbContext);
            }
            return _documentRepository;
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

    public IBaseRepository<Thread> ThreadRepository
    {
        get
        {
            if (_threadRepository is null)
            {
                _threadRepository = new ThreadRepository(_dbContext);
            }
            return _threadRepository;
        }
    }    
    public IBaseRepository<Reply> ReplyRepository
    {
        get
        {
            if (_replyRepository is null)
            {
                _replyRepository = new ReplyRepository(_dbContext);
            }
            return _replyRepository;
        }
    }
    public IBaseRepository<Feedback> FeedbackRepository
    {
        get
        {
            if (_feedbackRepository is null)
            {
                _feedbackRepository = new FeedbackRepository(_dbContext);
            }
            return _feedbackRepository;
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

    public async Task<bool> SaveChangesAsync(CancellationToken _ = default)
    {
        return await _dbContext.SaveChangesAsync(_) > 0;
    }
}
