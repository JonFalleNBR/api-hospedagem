using System;
using System.Collections.Generic;

namespace API.Hospedagem.Models
{
    public partial class Reserva
    {
        public int Id { get; set; }
        public int HospedeId { get; set; }
        public int QuartoId { get; set; }
        public DateTime DataCheckin { get; set; }
        public DateTime? DataCheckout { get; set; }
        public decimal? ValorTotal { get; set; }
        public string StatusReserva { get; set; } = null!;

        public virtual Hospede Hospede { get; set; } = null!;
        public virtual Quarto Quarto { get; set; } = null!;
    }
}
