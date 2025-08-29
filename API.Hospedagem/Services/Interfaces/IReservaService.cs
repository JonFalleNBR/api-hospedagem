using API.Hospedagem.DTOs;

namespace API.Hospedagem.Services.Interfaces
{
    public interface IReservaService
    {

        Task<IEnumerable<ReservaReadDto>> GetAllAsync();

        Task<ReservaReadDto?> GetByIdAsync(int id);

        Task<ReservaReadDto?> CreateAsync(ReservaCreateDto dto);




        Task<bool> UpdateAsync(int id, ReservaCreateDto dto);


        Task<bool> DeleteAsync(int id);

        Task<bool> CheckoutAsync(int reservaId, DateTime? dataCheckout = null);

        Task<ReservaReadDto?> CheckinAsync(ReservaCreateDto dto);


    }
}
