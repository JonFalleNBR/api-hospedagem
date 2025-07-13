using System;
using System.ComponentModel.DataAnnotations;

namespace API.Hospedagem.DTOs
{
    public class QuartoCreateDto
    {
        //Dados de entrada para criar quarto



        [Required, MaxLength(6)]
        public string Numero { get; set; } // Número do quarto

        [Required, MaxLength(16)]
        public string Tipo { get; set; } // Tipo do quarto (ex: simples, duplo, suíte)

        [Required]
        public double Preco { get; set; } // Preço do quarto




    }
}
