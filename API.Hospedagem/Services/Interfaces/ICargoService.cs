using API.Hospedagem.DTOs;

namespace API.Hospedagem.Services.Interfaces
{
    public interface ICargoService
    {


        Task<IEnumerable<CargoReadDto>> GetAllAsync();



    }



}
