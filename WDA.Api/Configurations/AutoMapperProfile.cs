using AutoMapper;
using WDA.Api.Dto.Attachment;
using WDA.Api.Dto.Customer.Request;
using WDA.Api.Dto.Customer.Response;
using WDA.Api.Dto.Document.Request;
using WDA.Api.Dto.Document.Response;
using WDA.Api.Dto.Forum;
using WDA.Api.Dto.Transaction.Request;
using WDA.Api.Dto.Transaction.Response;
using WDA.Api.Dto.User.Response;
using WDA.Domain.Models.Attachment;
using WDA.Domain.Models.Customer;
using WDA.Domain.Models.Document;
using WDA.Domain.Models.Remark;
using WDA.Domain.Models.Thread;
using WDA.Domain.Models.Transaction;
using WDA.Domain.Models.User;
using Thread = WDA.Domain.Models.Thread.Thread;

namespace WDA.Api.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        //UserDTO mapping
        CreateMap<User, UserInfoResponse>();

        //CustomerDTO mapping
        CreateMap<CreateCustomerRequest, Customer>();
        CreateMap<UpdateCustomerRequest, Customer>();
        CreateMap<Customer, CustomerResponse>()
            .ForMember(x => x.CreatedById, opt => opt.MapFrom(src => src.CreatedBy.Id))
            .ForMember(x => x.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.FullName))
            .ForMember(x => x.ModifiedById, opt => opt.MapFrom(src => src.ModifiedBy.Id))
            .ForMember(x => x.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedBy.FullName));

        //TransactionDTO mapping
        CreateMap<SubTransactionDto, SubTransaction>();
        CreateMap<SubTransaction, SubTransactionDto>();
        CreateMap<RemarksDto, Remark>();
        CreateMap<Remark, RemarksDto>();
        CreateMap<CreateTransactionRequest, Transaction>()
            .ForMember(x => x.SubTransactions, opt => opt.MapFrom(src => src.SubTransactions))
            .ForMember(x => x.Remarks, opt => opt.MapFrom(src => src.Remarks));
        CreateMap<Transaction, TransactionResponse>()
            .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToShortDateString()))
            .ForMember(x => x.CreatedById, opt => opt.MapFrom(src => src.CreatedBy.Id))
            .ForMember(x => x.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.FullName))
            .ForMember(x => x.ModifiedById, opt => opt.MapFrom(src => src.ModifiedBy.Id))
            .ForMember(x => x.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedBy.FullName));

        //DocumentDTO mapping
        CreateMap<Document, DocumentResponse>()
            .ForMember(x => x.CreatedById, opt => opt.MapFrom(src => src.CreatedBy.Id))
            .ForMember(x => x.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.FullName))
            .ForMember(x => x.ModifiedById, opt => opt.MapFrom(src => src.ModifiedBy.Id))
            .ForMember(x => x.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedBy.FullName));
        CreateMap<CreateDocumentRequest, Document>()
            .ForMember(x => x.Attachments, opt => opt.Ignore());
        CreateMap<UpdateDocumentRequest, Document>()
            .ForMember(x => x.Attachments, opt => opt.Ignore());

        CreateMap<Attachment, AttachmentResponse>()
            .ForMember(x => x.CreatedById, opt => opt.MapFrom(src => src.CreatedBy.Id))
            .ForMember(x => x.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.FullName))
            .ForMember(x => x.ModifiedById, opt => opt.MapFrom(src => src.ModifiedBy.Id))
            .ForMember(x => x.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedBy.FullName));
        CreateMap<Reply, ReplyResponse>()
            .ForMember(x => x.CreatedById, opt => opt.MapFrom(src => src.CreatedBy.Id))
            .ForMember(x => x.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.FullName))
            .ForMember(x => x.ModifiedById, opt => opt.MapFrom(src => src.ModifiedBy.Id))
            .ForMember(x => x.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedBy.FullName));
        CreateMap<Thread, ThreadResponse>()
            .ForMember(x => x.CreatedById, opt => opt.MapFrom(src => src.CreatedBy.Id))
            .ForMember(x => x.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.FullName))
            .ForMember(x => x.ModifiedById, opt => opt.MapFrom(src => src.ModifiedBy.Id))
            .ForMember(x => x.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedBy.FullName));
        CreateMap<CreateReplyRequest, Reply>();
        CreateMap<CreateThreadRequest, Thread>();
    }
}