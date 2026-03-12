using Application.Requests.Categoria;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers.Profiles
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            CreateMap<CreateCategoriaRequest, Categoria>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Transacoes, opt => opt.Ignore());

            CreateMap<UpdateCategoriaRequest, Categoria>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Transacoes, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));   
        }
    }
}
