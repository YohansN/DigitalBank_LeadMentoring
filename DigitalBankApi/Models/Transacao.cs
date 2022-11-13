using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalBankApi.Models
{
    public class Transacao
    {
        [Key]
        public int IdTransacao { get; set; }
        public int NumeroConta { get; set; }
        public Enum TipoTransacao { get; set; }
        public decimal ValorTransacao { get; set; }
        public DateTime DataTransacao { get; set; }
    }
}
