using API.Hospedagem.Data;
using API.Hospedagem.DTOs;
using API.Hospedagem.Models;
using API.Hospedagem.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Hospedagem.Services.Implementations
{
    public class FuncionarioService : IFuncionarioService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FuncionarioService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;

            _mapper = mapper;
        }


        public async Task<IEnumerable<FuncionarioReadDto>> GetAllAsync()
        {
            var list =  await _context.Funcionarios
                                        .Include(f => f.Cargo)
                                        .ToListAsync();

            return _mapper.Map<IEnumerable<FuncionarioReadDto>>(list);

        }

        public async Task<FuncionarioReadDto?> GetByIdAsync(int id)
        {
            var f = await _context.Funcionarios
                           .Include(x => x.Cargo)
                           .FirstOrDefaultAsync(x => x.Id == id);
            if (f == null) return null;
            return _mapper.Map<FuncionarioReadDto>(f);
        }


        public async Task<FuncionarioReadDto?> CreateAsync(FuncionarioCreateDto dto)
        {


            // Validação defensiva para campos obrigatórios mínimos e tratamento para casos nulos ou inválidos
            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.Nome) || dto.Nome.Length < 3 ||
                string.IsNullOrWhiteSpace(dto.CPF) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Telefone) ||
                string.IsNullOrWhiteSpace(dto.Endereco) ||
                string.IsNullOrWhiteSpace(dto.CargoNome))
            {
                return null;
            }

            var nome = dto.CargoNome.Trim().ToLower();
            int cargoId;


            //nome.Contains("recep") || nome.Contains("gest") ? cargoId = 1 : cargoId = 2;


            if (nome.Contains("recep") || nome.Contains("gest") || nome.Contains("geren"))
                cargoId = 1;
            else
                cargoId = 2;

            // 2) montar a entidade
            var f = new Funcionario
            {
                Nome = dto.Nome,
                CPF = dto.CPF,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Endereco = dto.Endereco,
                CargoId = cargoId
            };

            _context.Funcionarios.Add(f);
            await _context.SaveChangesAsync();

            // 3) recarregar com o Cargo para mapear o nome
            await _context.Entry(f).Reference(x => x.Cargo).LoadAsync();

            return _mapper.Map<FuncionarioReadDto>(f);

        }


        public async Task<bool> DeleteAsync(int id)
        {
            var delete = await _context.Funcionarios.FindAsync(id);
            if (delete == null) return false;

            _context.Funcionarios.Remove(delete);
            await _context.SaveChangesAsync();
            return true;
        }

    

    

        public async Task<bool> UpdateAsync(int id, FuncionarioCreateDto dto)
        {

            if (dto == null ||
               string.IsNullOrWhiteSpace(dto.Nome) || dto.Nome.Length < 3 ||
               string.IsNullOrWhiteSpace(dto.CPF) ||
               string.IsNullOrWhiteSpace(dto.Email) ||
               string.IsNullOrWhiteSpace(dto.Telefone) ||
               string.IsNullOrWhiteSpace(dto.Endereco) ||
               string.IsNullOrWhiteSpace(dto.CargoNome))
            {
                return false; // falha na validação
            }

            var f = await _context.Funcionarios.FindAsync(id);
            if (f == null) return false;

            // mescla campos:
            f.Nome = dto.Nome;
            f.CPF = dto.CPF;
            f.Email = dto.Email;
            f.Telefone = dto.Telefone;
            f.Endereco = dto.Endereco;

            // recalcular CargoId igual no Create
            var nome = dto.CargoNome.Trim().ToLower();
            f.CargoId = (nome.Contains("recep") || nome.Contains("gest") || nome.Contains("geren")) ? 1 : 2; //  posso criar if ternario com ate 3 validacoes Or 



            await _context.SaveChangesAsync();
            return true;

        }
    }
}
