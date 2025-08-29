using API.Hospedagem.DTOs;
using API.Hospedagem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Hospedagem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _service;

        public ReservaController(IReservaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservaReadDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}", Name = "GetReservaById")]
        public async Task<ActionResult<ReservaReadDto>> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        // CHECK-IN (pode ser seu POST padrão)
        [HttpPost("checkin")]
        public async Task<ActionResult<ReservaReadDto>> Checkin([FromBody] ReservaCreateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var criado = await _service.CheckinAsync(dto); // ou CreateAsync(dto)
            if (criado == null)
                return BadRequest("Quarto indisponível ou hóspede já possui reserva ativa.");

            return CreatedAtRoute("GetReservaById", new { id = criado.id }, criado);
        }

        // Checkout (encerra reserva, calcula total, libera quarto)
        [HttpPost("{id:int}/checkout")]
        public async Task<IActionResult> Checkout(int id)
        {
            var ok = await _service.CheckoutAsync(id);
            return ok ? Ok("Checkout realizado") : NotFound("Reserva não encontrada ou já finalizada");
        }

        // Mantém seu POST antigo se quiser (cria check-in do mesmo jeito)
        [HttpPost]
        public async Task<ActionResult<ReservaReadDto>> Create([FromBody] ReservaCreateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var criado = await _service.CreateAsync(dto);
            if (criado == null)
                return BadRequest("Quarto não disponível para reserva ou não encontrado");

            return CreatedAtRoute("GetReservaById", new { id = criado.id }, criado);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ReservaCreateDto dto)
            => await _service.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => await _service.DeleteAsync(id) ? NoContent() : NotFound();

        // (Opcional) listar apenas ativas
        [HttpGet("ativas")]
        public async Task<ActionResult<IEnumerable<ReservaReadDto>>> GetAtivas()
        {
            var todas = await _service.GetAllAsync();
            return Ok(todas.Where(r => r.DataCheckout == null && r.StatusReserva == "Ativa"));
        }
    }
}
