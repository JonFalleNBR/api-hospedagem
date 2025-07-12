using System.ComponentModel.DataAnnotations;

namespace API.Hospedagem.DTOs
{
    public class HospedeCreateDto
    {

        // Dados de entrada para criar hóspede 


        [Required, MaxLength(100)]
        public string Nome { get; set; }

        [Required, MaxLength(14)]
        public string CPF { get; set; }

        [EmailAddress, MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Telefone { get; set; }
    }
}

