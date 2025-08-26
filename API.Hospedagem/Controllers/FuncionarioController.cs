using API.Hospedagem.DTOs;
using API.Hospedagem.Services.Implementations;
using API.Hospedagem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Hospedagem.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class FuncionarioController : ControllerBase
    {

        private readonly IFuncionarioService _srv;

        public FuncionarioController(IFuncionarioService srv)
        {
            _srv = srv;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuncionarioReadDto>>> GetAll()
            => Ok(await _srv.GetAllAsync());

        [HttpGet("{id:int}", Name = "GetFuncionarioById")]
        public async Task<ActionResult<FuncionarioReadDto>> GetById(int id)
        {
            var dto = await _srv.GetByIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<FuncionarioReadDto>> Create(FuncionarioCreateDto dto)
        {
            var criado = await _srv.CreateAsync(dto);
            return CreatedAtRoute("GetFuncionarioById",
                                  new { id = criado!.Id },
                                  criado);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, FuncionarioCreateDto dto)
            => await _srv.UpdateAsync(id, dto)
                 ? NoContent()
                 : NotFound();


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => await _srv.DeleteAsync(id)
                 ? NoContent()
                 : NotFound();




    }
}
