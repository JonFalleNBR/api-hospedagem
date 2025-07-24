using API.Hospedagem.DTOs;
using API.Hospedagem.Models;

namespace API.Hospedagem.Services.Interfaces
{
    public interface IFuncionarioService
    {

        Task<IEnumerable<FuncionarioReadDto>> GetAllAsync();


        Task<FuncionarioReadDto?> GetByIdAsync(int id);

        Task<FuncionarioReadDto?> CreateAsync(FuncionarioCreateDto dto);

        Task<bool> UpdateAsync(int id, FuncionarioCreateDto dto);

        Task<bool> DeleteAsync(int id);



    }
}
