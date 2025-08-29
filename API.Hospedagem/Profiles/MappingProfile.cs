using API.Hospedagem.DTOs;
using API.Hospedagem.Models;
using AutoMapper;

namespace API.Hospedagem.Profiles
{
    public class MappingProfile : Profile
    {


        public MappingProfile()
        {
            CreateMap<Hospede, HospedeReadDto>();         
            CreateMap<HospedeCreateDto, Hospede>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DataCadastro, opt => opt.Ignore())
                .ForMember(dest => dest.Reservas, opt => opt.Ignore());

            // demais mappings...
            CreateMap<Quarto, QuartoReadDto>()
                   .ForMember(dest => dest.Preco,
                  opt => opt.MapFrom(src => src.PrecoPorNoite));

            CreateMap<QuartoCreateDto, Quarto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Reservas, opt => opt.Ignore())
                .ForMember(dest => dest.PrecoPorNoite,
                           opt => opt.MapFrom(src => src.Preco));




            CreateMap<Reserva, ReservaReadDto>();
            CreateMap<ReservaCreateDto, Reserva>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ValorTotal, opt => opt.Ignore())   
                .ForMember(dest => dest.Quarto, opt => opt.Ignore())    
                .ForMember(dest => dest.Hospede, opt => opt.Ignore());



            CreateMap<ReservaCreateDto, Reserva>()
                .ForMember(d => d.DataCheckin, opt => opt.Ignore())
                    .ForMember(d => d.DataCheckout, opt => opt.Ignore())
                 .ForMember(d => d.ValorTotal, opt => opt.Ignore());
            




            CreateMap<Cargo, CargoReadDto>();


            CreateMap<FuncionarioCreateDto, Funcionario>()
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.CargoId, opt => opt.MapFrom(src => src.CargoId));

            CreateMap<Funcionario, FuncionarioReadDto>()
             .ForMember(dest => dest.CargoNome, opt => opt.MapFrom(src => src.Cargo.Nome));



        }

    }
   }

