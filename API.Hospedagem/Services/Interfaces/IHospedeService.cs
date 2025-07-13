using API.Hospedagem.DTOs;

namespace API.Hospedagem.Services.Interfaces
{
    public interface IHospedeService
    {
        Task<IEnumerable<HospedeReadDto>> GetAllAsync();

        Task<HospedeReadDto?> GetByIdAsync(int id);

        Task<HospedeReadDto?> CreateAsync(HospedeCreateDto dto);

        Task<bool> UpdateAsync(int id, HospedeCreateDto dto); 

        Task<bool> DeleteAsync(int id);




    }
}
