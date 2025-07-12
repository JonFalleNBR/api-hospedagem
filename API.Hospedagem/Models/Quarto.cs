using System;
using System.Collections.Generic;

namespace API.Hospedagem.Models
{
    public partial class Quarto
    {
        public Quarto()
        {
            Reservas = new HashSet<Reserva>();
        }

        public int Id { get; set; }
        public int Numero { get; set; }
        public string Tipo { get; set; } = null!;
        public decimal PrecoPorNoite { get; set; }
        public int Status { get; set; }

        public virtual ICollection<Reserva> Reservas { get; set; }
    }
}
