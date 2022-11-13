using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalBankApi.Models
{
    public class Transacao
    {
        [Key]
        public int IdTransacao { get; set; }
        public int NumeroConta { get; set; }
        public enum TipoTransacao
        {
            Debito = 1,
            Deposito = 2,
            Transferencia_Enviada = 3,
            Transferencia_Recebida = 4
        }
        public decimal ValorTransacao { get; set; }
        public DateTime DataTransacao { get; set; }
    }
}
