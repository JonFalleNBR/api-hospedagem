namespace API.Hospedagem.Models
{
    public class Funcionario
    {

        public int Id { get; set; }

        public string Nome { get; set; } = null!;

        public string CPF { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Telefone { get; set; } = null!;

        public string Endereco { get; set; } = null!;   



        // Defiindo a FK do Cargo 
        public int CargoId { get; set; }
        public Cargo cargo { get; set; } = null!;



    }
}
