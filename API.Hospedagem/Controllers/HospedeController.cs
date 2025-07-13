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
                return NotFound();
            }
            Thread.Sleep(5000); 
            return Ok(dto); 
        }

        // POST api/hospede
        [HttpPost]
        public async Task<ActionResult<HospedeReadDto>> Create(HospedeCreateDto dto) {
            var criado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado); 
            // return CreatedAtAction("GetHospedeById", new { id = criado.Id }, criado); 
        }


        // PUT api/hospede/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, HospedeCreateDto dto)
        {
            var atualizado = await _service.UpdateAsync(id, dto);
            return atualizado ? NoContent() : NotFound(); 


        }

        // DELETE api/hospede/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) { 
            var deletado = await _service.DeleteAsync(id);
            return deletado ? NoContent() : NotFound(); 

        }

    }
}