using AutoMapper;
using WDA.Api.Dto.Customer.Request;
using WDA.Api.Dto.Customer.Response;
using WDA.Api.Dto.Transaction.Request;
using WDA.Api.Dto.Transaction.Response;
using WDA.Api.Dto.User.Response;
using WDA.Domain.Models.Customer;
using WDA.Domain.Models.Remark;
using WDA.Domain.Models.Transaction;
using WDA.Domain.Models.User;

namespace WDA.Api.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserInfoResponse>();

        CreateMap<CreateCustomerRequest, Customer>();
        CreateMap<UpdateCustomerRequest, Customer>();
        CreateMap<Customer, CustomerResponse>()
            .ForMember(x => x.CreatedById, opt => opt.MapFrom(src => src.CreatedBy.Id))
            .ForMember(x => x.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.FullName))
            .ForMember(x => x.ModifiedById, opt => opt.MapFrom(src => src.ModifiedBy.Id))
            .ForMember(x => x.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedBy.FullName)); 
        
        CreateMap<SubTransactionDto, SubTransaction>();
        CreateMap<SubTransaction, SubTransactionDto>();        
        CreateMap<RemarksDto, Remark>();
        CreateMap<Remark, RemarksDto>();
        CreateMap<CreateTransactionRequest, Transaction>()
            .ForMember(x => x.SubTransactions, opt => opt.MapFrom(src => src.SubTransactions))
            .ForMember(x => x.Remarks, opt => opt.MapFrom(src => src.Remarks));
        CreateMap<Transaction, TransactionResponse>()
            .ForMember(x => x.CreatedById, opt => opt.MapFrom(src => src.CreatedBy.Id))
            .ForMember(x => x.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.FullName))
            .ForMember(x => x.ModifiedById, opt => opt.MapFrom(src => src.ModifiedBy.Id))
            .ForMember(x => x.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedBy.FullName));

    }
}
