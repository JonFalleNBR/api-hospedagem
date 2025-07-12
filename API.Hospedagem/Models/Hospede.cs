using System;
using System.Collections.Generic;

namespace API.Hospedagem.Models
{
    public partial class Hospede
    {
        public Hospede()
        {
            Reservas = new HashSet<Reserva>();
        }

        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Cpf { get; set; } = null!;
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public DateTime DataCadastro { get; set; }

        public virtual ICollection<Reserva> Reservas { get; set; }
    }
}
