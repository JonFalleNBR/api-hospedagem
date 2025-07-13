using API.Hospedagem.DTOs;

namespace API.Hospedagem.Services.Interfaces
{
    public interface IQuartoService
    {

        Task<IEnumerable<QuartoReadDto>> GetAllAsync();

        Task<QuartoReadDto?> GetByIdAsync(int id);

        Task<QuartoReadDto?> CreateAsync(QuartoCreateDto dto);

        Task<bool> UpdateAsync(int id, QuartoCreateDto dto);

        Task<bool> DeleteAsync(int id);

    }
}
