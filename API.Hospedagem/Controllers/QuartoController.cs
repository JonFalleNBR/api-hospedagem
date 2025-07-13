using API.Hospedagem.DTOs;
using API.Hospedagem.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Hospedagem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class QuartoController : ControllerBase
    {

        private readonly IQuartoService _service;



        public QuartoController(IQuartoService service)
        {
            _service = service;
       

        }

        // GET api/quarto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuartoReadDto>>> GetAll() {

            var list = await _service.GetAllAsync();
            return Ok(list);


        }


        // GET api/quarto/{id}
        [HttpGet("{id:int}", Name = "GetQuartoById")]
        public async Task<ActionResult<QuartoReadDto>> GetById(int id) { 
        
            var quarto = await _service.GetByIdAsync(id);
            return quarto == null ? NotFound() : Ok(quarto);

        }


        // POST api/quarto
        [HttpPost]
        public async Task<ActionResult<QuartoReadDto>> Create(QuartoCreateDto dto) { 
            var criado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado); 

        }



        // PUT api/quarto/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id ,QuartoCreateDto dto) {

            var atualizado = await _service.UpdateAsync(id, dto);
            return atualizado ? NoContent() : NotFound(); 
        
        }



        // DELETE api/quarto/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {

            var deletado = await _service.DeleteAsync(id);
            return deletado ? NoContent() : NotFound();

        }


    }
}
