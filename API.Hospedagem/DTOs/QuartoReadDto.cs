using API.Hospedagem.Helpers; // Certifique-se de que o namespace Helper está correto


namespace API.Hospedagem.DTOs
{
    public class QuartoReadDto
    {
        // Dados de entrada para ler as informações de um quarto

        public int Id { get; set; }           // ← ESSA LINHA PRECISA ESTAR AÍ

        public string Numero { get; set; }

        public string Tipo { get; set; }

        public double Preco { get; set; }

        public EnumStatusQuarto Status { get; set; } // Aqui utiliza o enum Status definido na classe Helper
    }

}
