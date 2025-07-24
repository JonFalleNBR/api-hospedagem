using System.ComponentModel.DataAnnotations;

namespace API.Hospedagem.DTOs
{
    public class FuncionarioCreateDto
    {

        [Required, MaxLength(100)]
        public string Nome { get; set; } = null!;

        [Required, StringLength(14)]
        public string CPF { get; set; } = null!;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Telefone { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Endereco { get; set; } = null!;

        
        public int CargoId { get; internal set; }

        [Required]
        public string CargoNome { get; set; }
    }




}

