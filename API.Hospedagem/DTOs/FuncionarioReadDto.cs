namespace API.Hospedagem.DTOs
{
    public class FuncionarioReadDto
    {

        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string CPF { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Telefone { get; set; } = null!;
        public string Endereco { get; set; } = null!;



        // Você pode querer expor o nome do cargo em vez do ID:
        public int CargoId { get; set; }
        public string CargoNome { get; set; } = null!;



    }
}
