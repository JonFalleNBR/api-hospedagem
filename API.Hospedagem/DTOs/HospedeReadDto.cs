namespace API.Hospedagem.DTOs
{
    public class HospedeReadDto
    {
        // Dados de entrada para ler as informações de um hóspede
        public int Id { get; set; }           // ← ESSA LINHA PRECISA ESTAR AÍ
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public DateTime DataCadastro { get; set; }

    }
}
