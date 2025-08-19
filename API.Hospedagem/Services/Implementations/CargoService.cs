using API.Hospedagem.Data;
using API.Hospedagem.DTOs;
using API.Hospedagem.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Hospedagem.Services.Implementations
{
    public class CargoService : ICargoService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CargoService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CargoReadDto>> GetAllAsync()
        {
            var lista = await _context.Cargos.OrderBy(c => c.Nome).ToListAsync();
            return _mapper.Map<IEnumerable<CargoReadDto>>(lista);

        }
    }
}
