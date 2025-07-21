using System.ComponentModel.DataAnnotations;

namespace API.Hospedagem.DTOs
{
    public class ReservaCreateDto
    {
        // Dados de entrada para criar reserva 

        [Required]
        public int HospedeId { get; set; }

        [Required]
        public int QuartoId { get; set; }


        [Required]
        public DateTime dataCheckin { get; set; }


        public DateTime? dataCheckout { get; set; }

        public string? statusReserva { get; set; }



    }
}
