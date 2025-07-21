using API.Hospedagem.Data;
using API.Hospedagem.DTOs;
using API.Hospedagem.Models;
using API.Hospedagem.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Hospedagem.Services.Implementations
{
    public class ReservaService : IReservaService
    {

        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;

        public ReservaService(ApplicationDbContext context, IMapper mapper) { 
            _context = context;
            _mapper = mapper;

        }


        public async Task<IEnumerable<ReservaReadDto>> GetAllAsync()
        {

            var lista = await  _context.Reservas.ToListAsync();
            return _mapper.Map<IEnumerable<ReservaReadDto>>(lista);

        }


        public async Task<ReservaReadDto?> GetByIdAsync(int id)
        {

            var objeto = await _context.Reservas.FindAsync(id);
            return objeto == null ? null : _mapper.Map<ReservaReadDto>(objeto);

        }


        public async Task<ReservaReadDto?> CreateAsync(ReservaCreateDto dto)
        {
            var quarto = await _context.Quartos.FindAsync(dto.QuartoId);
            if(quarto == null )
                    return Exception.ReferenceEquals(null, quarto) ? null : throw new Exception("Quarto não encontrado");

            if (quarto.Status != 0) { 
                var mensagem = "Quarto não disponível para reserva";
                return null;
                
            }

            var reserva = _mapper.Map<Reserva>(dto);
            reserva.StatusReserva =  dto.statusReserva ?? "Disponível";
            _context.Reservas.Add(reserva);

            quarto.Status = 1;

            await _context.SaveChangesAsync();
            return _mapper.Map<ReservaReadDto>(reserva);

        }



        public async Task<bool> UpdateAsync(int id, ReservaCreateDto dto)
        {
                var reserva = await _context.Reservas
                                            .Include(r => r.Quarto)
                                            .FirstOrDefaultAsync(r => r.Id == id);


            if (reserva == null) return false;

            _mapper.Map(dto, reserva);

            if (dto.dataCheckout.HasValue) { 
                var nights = (dto.dataCheckout.Value - dto.dataCheckin).Days;
                reserva.ValorTotal = nights * reserva.Quarto.PrecoPorNoite;
                reserva.Quarto.Status = 0;

            }

            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteAsync(int id)
        {

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return false;

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return true;

        }

     

      
     
    }
}
