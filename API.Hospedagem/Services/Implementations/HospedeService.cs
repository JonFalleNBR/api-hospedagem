using API.Hospedagem.Data;
using API.Hospedagem.DTOs;
using API.Hospedagem.Models;
using API.Hospedagem.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Hospedagem.Services.Implementations
{
    public class HospedeService : IHospedeService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;



        public HospedeService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<HospedeReadDto?> CreateAsync(HospedeCreateDto dto)
        {
            var entity = _mapper.Map<Hospede>(dto);
            _context.Hospedes.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<HospedeReadDto>(entity);

        }

        public async Task<IEnumerable<HospedeReadDto>> GetAllAsync()
        {
            var entities = await _context.Hospedes.ToListAsync();
            return _mapper.Map<IEnumerable<HospedeReadDto>>(entities);
        }

        public async Task<HospedeReadDto?> GetByIdAsync(int id)
        {
            var entity = await _context.Hospedes.FindAsync(id); 
            return entity == null ? null : _mapper.Map<HospedeReadDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, HospedeCreateDto dto)
        {
            var entity = await _context.Hospedes.FindAsync(id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Hospedes.FindAsync(id);
            if (entity == null) return false;

            _context.Hospedes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
