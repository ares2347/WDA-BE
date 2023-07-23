using AutoMapper;
using WDA.Api.Dto.Attachment;
using WDA.Api.Dto.Customer.Request;
using WDA.Api.Dto.Customer.Response;
using WDA.Api.Dto.Ticket;
using WDA.Api.Dto.Transaction.Response;
using WDA.Api.Dto.User.Response;
using WDA.Domain.Models.Attachment;
using WDA.Domain.Models.Customer;
using WDA.Domain.Models.Ticket;
using WDA.Domain.Models.Transaction;
using WDA.Domain.Models.User;

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
        CreateMap<Transaction, TransactionResponse>()
            .ForMember(x => x.CreatedById, opt => opt.MapFrom(src => src.CreatedBy.Id))
            .ForMember(x => x.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.FullName))
            .ForMember(x => x.ModifiedById, opt => opt.MapFrom(src => src.ModifiedBy.Id))
            .ForMember(x => x.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedBy.FullName));

        //DocumentDTO mapping

        CreateMap<Attachment, AttachmentResponse>()
            .ForMember(x => x.CreatedById, opt => opt.MapFrom(src => src.CreatedBy.Id))
            .ForMember(x => x.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.FullName))
            .ForMember(x => x.ModifiedById, opt => opt.MapFrom(src => src.ModifiedBy.Id))
            .ForMember(x => x.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedBy.FullName));
        
        //Customer ticket mapping 
        CreateMap<CreateCustomerTicketRequest, CustomerTicket>();
        CreateMap<CustomerTicket, CustomerTicketResponse>();
    }
}