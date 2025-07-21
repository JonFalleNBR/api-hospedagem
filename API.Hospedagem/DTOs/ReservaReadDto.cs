using API.Hospedagem.Models;

namespace API.Hospedagem.DTOs
{
    public class ReservaReadDto
    {

        // Dados de entrada para ler as informações de uma reserva

        public int id { get; set; }

        public int HospedeId { get; set; }

        public int QuartoId { get; set; }

        public DateTime DataCheckin { get; set; }
        public DateTime? DataCheckout { get; set; }
        public decimal? ValorTotal { get; set; }
        public string? StatusReserva { get; set; }
    }
}
