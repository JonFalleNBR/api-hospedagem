namespace API.Hospedagem.Models
{
    public class Cargo
    {

        public int Id { get; set;  }
        public string Nome { get; set; } = null!;


        public ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();


    }
}
