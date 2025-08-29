using API.Hospedagem.Data;
using API.Hospedagem.DTOs;
using API.Hospedagem.Models;
using API.Hospedagem.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Hospedagem.Services.Implementations
{
    public class ReservaService : IReservaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReservaService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // =========================
        // CONSULTAS BÁSICAS
        // =========================
        public async Task<IEnumerable<ReservaReadDto>> GetAllAsync()
        {
            var lista = await _context.Reservas
                                      .Include(r => r.Quarto)
                                      .Include(r => r.Hospede)
                                      .AsNoTracking()
                                      .ToListAsync();
            return _mapper.Map<IEnumerable<ReservaReadDto>>(lista);
        }

        public async Task<ReservaReadDto?> GetByIdAsync(int id)
        {
            var objeto = await _context.Reservas
                                       .Include(r => r.Quarto)
                                       .Include(r => r.Hospede)
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(r => r.Id == id);
            return objeto == null ? null : _mapper.Map<ReservaReadDto>(objeto);
        }

        // =========================
        // CHECK-IN (Create)
        // =========================
        public async Task<ReservaReadDto?> CreateAsync(ReservaCreateDto dto)
            => await CheckinAsync(dto);

        public async Task<ReservaReadDto?> CheckinAsync(ReservaCreateDto dto)
        {
            // 1) quarto existe?
            var quarto = await _context.Quartos.FirstOrDefaultAsync(q => q.Id == dto.QuartoId);
            if (quarto is null) return null;

            // 2) quarto está livre? (0 = livre, 1 = ocupado)
            if (quarto.Status == 1) return null;

            // 3) quarto já tem reserva ativa?
            var quartoOcupado = await _context.Reservas
                .AnyAsync(r => r.QuartoId == dto.QuartoId &&
                               r.DataCheckout == null &&
                               r.StatusReserva == "Ativa");
            if (quartoOcupado) return null;

            // 4) hóspede já possui reserva ativa?
            var hospedeComReservaAtiva = await _context.Reservas
                .AnyAsync(r => r.HospedeId == dto.HospedeId &&
                               r.DataCheckout == null &&
                               r.StatusReserva == "Ativa");
            if (hospedeComReservaAtiva) return null;

            // 5) criar reserva ativa
            var reserva = _mapper.Map<Reserva>(dto);
            reserva.StatusReserva = string.IsNullOrWhiteSpace(dto.statusReserva) ? "Ativa" : dto.statusReserva;
            reserva.DataCheckin = DateTime.Now; 
            reserva.DataCheckout = null;
            reserva.ValorTotal = null;

            // 6) marcar quarto como ocupado
            quarto.Status = 1;

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return _mapper.Map<ReservaReadDto>(reserva);
        }

        // =========================
        // CHECK-OUT
        // =========================
        public async Task<bool> CheckoutAsync(int reservaId, DateTime? dataCheckout = null)
        {
            // transação pra garantir consistência
            using var tx = await _context.Database.BeginTransactionAsync();

            var reserva = await _context.Reservas
                                        .Include(r => r.Quarto)
                                        .FirstOrDefaultAsync(r => r.Id == reservaId);

            if (reserva is null) return false;

            // já finalizada?
            if (reserva.DataCheckout != null) return false;

            var checkout = (dataCheckout ?? DateTime.Now);

            // cálculo de diárias (mínimo 1 diária), usando Date para ignorar horas
            var nights = (int)Math.Ceiling((checkout.Date - reserva.DataCheckin.Date).TotalDays);
            if (nights <= 0) nights = 1;

            // valor total
            var precoPorNoite = reserva.Quarto.PrecoPorNoite;
            reserva.ValorTotal = nights * precoPorNoite;

            // finalizar reserva e liberar quarto
            reserva.DataCheckout = checkout;
            reserva.StatusReserva = "Finalizada";
            reserva.Quarto.Status = 0; // 0 = livre

            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }

        // =========================
        // UPDATE / DELETE (se quiser manter)
        // =========================
        public async Task<bool> UpdateAsync(int id, ReservaCreateDto dto)
        {
            var reserva = await _context.Reservas
                                        .Include(r => r.Quarto)
                                        .FirstOrDefaultAsync(r => r.Id == id);
            if (reserva is null) return false;

            // Atualiza campos básicos (seu AutoMapper deve ignorar os que não quer trocar)
            _mapper.Map(dto, reserva);

            // Se veio checkout no update, aplica as regras
            if (dto.dataCheckout.HasValue && reserva.DataCheckout == null)
            {
                var nights = (int)Math.Ceiling((dto.dataCheckout.Value.Date - reserva.DataCheckin.Date).TotalDays);
                if (nights <= 0) nights = 1;

                reserva.ValorTotal = nights * reserva.Quarto.PrecoPorNoite;
                reserva.DataCheckout = dto.dataCheckout.Value;
                reserva.StatusReserva = "Finalizada";
                reserva.Quarto.Status = 0; // libera
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva is null) return false;

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
