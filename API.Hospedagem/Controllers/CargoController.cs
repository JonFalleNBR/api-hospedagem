using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using API.Hospedagem.Services.Interfaces;

namespace API.Hospedagem.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class CargoController : ControllerBase
    {

        private readonly ICargoService _cargoRepository;




        public CargoController(ICargoService service) => _cargoRepository = service;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var cargos = await _cargoRepository.GetAllAsync();
            return Ok(cargos);
        }

    }
}
