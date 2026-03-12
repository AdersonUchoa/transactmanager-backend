using Application.Requests.Transacao;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers.Profiles
{
    public class TransacaoProfile : Profile
    {
        public TransacaoProfile()
        {
            CreateMap<CreateTransacaoRequest, Transacao>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Pessoa, opt => opt.Ignore())
                .ForMember(dest => dest.Categoria, opt => opt.Ignore());

            CreateMap<UpdateTransacaoRequest, Transacao>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Pessoa, opt => opt.Ignore())
                .ForMember(dest => dest.Categoria, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
