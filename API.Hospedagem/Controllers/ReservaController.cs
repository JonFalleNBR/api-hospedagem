using API.Hospedagem.DTOs;
using API.Hospedagem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Hospedagem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {

        // Eu quero que ao deletar a reserva, ela seja movida para uma tabela de Historico com os dados da Tabela Reserva após o Put
        // A Deleção devera ocorrer automaticamente após x tempo caso o quarto esteja = 0 e o status da reserva = Disponivel


        private readonly IReservaService _service;



        public ReservaController(IReservaService service)
        {

            _service = service;

        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservaReadDto>>> GetAll() => Ok(await _service.GetAllAsync());



        [HttpGet("{id:int}", Name = "GetReservaById")]
        public async Task<ActionResult<ReservaReadDto>> GetById(int id)
        {

            var dto = await _service.GetByIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);


        }



        [HttpPost]
        public async Task<ActionResult<ReservaReadDto>> Create(ReservaCreateDto dto)
        {

            var criado = await _service.CreateAsync(dto);

            if (criado == null) return BadRequest("Quarto não disponível para reserva ou não encontrado");

            // TODO -> adicionar logica que se um hospede ja tiver alugado um quarto, ele nao pode alugar outro

            return CreatedAtRoute("GetReservaById", new { id = criado.id }, criado);


        }



        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ReservaCreateDto dto) => await _service.UpdateAsync(id, dto) ? NoContent() : NotFound();



        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {


            var resultado = await _service.DeleteAsync(id);
            return resultado ? NoContent() : NotFound();

        }
    }
}
