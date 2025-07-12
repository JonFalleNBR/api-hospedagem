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

            // criação: ignora Id, DataCadastro e Reservas,
            // pela regra de negócio vamos setar DataCadastro no serviço
            CreateMap<HospedeCreateDto, Hospede>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DataCadastro, opt => opt.Ignore())
                .ForMember(dest => dest.Reservas, opt => opt.Ignore());

            // demais mappings...
            CreateMap<Quarto, QuartoReadDto>();
            CreateMap<QuartoCreateDto, Quarto>();

            CreateMap<Reserva, ReservaReadDto>();
            CreateMap<ReservaCreateDto, Reserva>();

        }

    }
    }

