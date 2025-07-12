using API.Hospedagem.DTOs;
using API.Hospedagem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Hospedagem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class HospedeController : ControllerBase
    {

        private readonly IHospedeService _service;

        public HospedeController(IHospedeService service)
        {

             _service = service;

        }
        // implementar os métodos do controller com as // chamadas para o serviço IHospedeService

        // GET api/hospede
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HospedeReadDto>>> GetAll() {
            var list = await _service.GetAllAsync();
            return Ok(list); // Retorna 200 OK com a lista de hóspedes

        }

        // GET api/hospede/{id}
        [HttpGet("{id:int}", Name = "GetHospedeById")]
        public async Task<ActionResult<HospedeReadDto>> GetById(int id) { 
            var dto = await _service.GetByIdAsync(id);
            if (dto is null) { 
                return NotFound(); // Retorna 404 Not Found se o hóspede não for encontrado
            }
            Thread.Sleep(5000); // Simula uma espera de 5 segundos para fins de demonstração
            return Ok(dto); // Retorna 200 OK com os dados do hóspede
        }

        // POST api/hospede
        [HttpPost]
        public async Task<ActionResult<HospedeReadDto>> Create(HospedeCreateDto dto) {
            var criado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado); // retorna um 201 Created com o local do recurso criado
            // return CreatedAtAction("GetHospedeById", new { id = criado.Id }, criado); 
        }


        // PUT api/hospede/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, HospedeCreateDto dto)
        {
            var atualizado = await _service.UpdateAsync(id, dto);
            return atualizado ? NoContent() : NotFound(); // if ternario que define o Retorno de um 204 No Content se atualizado com sucesso, ou 404 Not Found se não encontrado


        }

        // DELETE api/hospede/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) { 
            var deletado = await _service.DeleteAsync(id);
            return deletado ? NoContent() : NotFound(); 

        }

    }
}