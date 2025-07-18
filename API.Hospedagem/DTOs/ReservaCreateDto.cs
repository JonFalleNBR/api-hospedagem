using System.ComponentModel.DataAnnotations;

namespace API.Hospedagem.DTOs
{
    public class ReservaCreateDto
    {
        // Dados de entrada para criar reserva 

        [Required]
        public int id { get; set; }

        [Required]
        public int numero { get; set; }

        [Required]
        public DateTime dataCheckin { get; set; }





    }
}
