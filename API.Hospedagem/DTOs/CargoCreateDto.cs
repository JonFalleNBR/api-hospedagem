using System.ComponentModel.DataAnnotations;

namespace API.Hospedagem.DTOs
{


   
    public class CargoCreateDto
    {

        [Required]
        [MaxLength(50)]
        public string Nome { get; set; } = null!;


    }
}
