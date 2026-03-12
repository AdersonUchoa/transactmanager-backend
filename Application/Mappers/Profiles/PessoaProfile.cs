using Application.Requests.Pessoa;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers.Profiles
{
    public class PessoaProfile : Profile
    {
        public PessoaProfile() {
            CreateMap<CreatePessoaRequest, Pessoa>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Transacoes, opt => opt.Ignore());

            CreateMap<UpdatePessoaRequest, Pessoa>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Transacoes, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
