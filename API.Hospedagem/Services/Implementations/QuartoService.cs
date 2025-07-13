using API.Hospedagem.Data;
using API.Hospedagem.DTOs;
using API.Hospedagem.Models;
using API.Hospedagem.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Hospedagem.Services.Implementations
{
    public class QuartoService : IQuartoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public QuartoService(ApplicationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;

        }

   
        public async Task<IEnumerable<QuartoReadDto>> GetAllAsync()
        {

            var entities = await _context.Quartos.ToListAsync();
            return _mapper.Map<IEnumerable<QuartoReadDto>>(entities);

        }

        public async Task<QuartoReadDto?> GetByIdAsync(int id)
        {
           var entity = await _context.Quartos.FindAsync(id);
            return entity == null ? null : _mapper.Map<QuartoReadDto>(entity);
        }

        public async Task<QuartoReadDto?> CreateAsync(QuartoCreateDto dto)
        {
             var entity =  _mapper.Map<Quarto>(dto);
            _context.Quartos.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<QuartoReadDto>(entity);

        }

        public async Task<bool> UpdateAsync(int id, QuartoCreateDto dto)
        {
            
            var entities = await _context.Quartos.FindAsync(id);

            if (entities == null) return false;
            _mapper.Map(dto, entities);
            await _context.SaveChangesAsync();
            return true;



        }


        public async Task<bool> DeleteAsync(int id)
        {
            var entities = await _context.Quartos.FindAsync(id);

            if (entities == null) return false;

            _context.Quartos.Remove(entities);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
