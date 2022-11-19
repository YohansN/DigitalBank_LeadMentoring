using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DigitalBankApi.Models
{
    public class ContaBancaria
    {
        [Key]
        public int NumeroConta { get; set; }
        public int IdCliente { get; set; }
        public decimal Saldo { get; set; }

        public ContaBancaria (int idCliente, decimal saldo)
        {
            IdCliente = idCliente;
            Saldo = saldo;
        }
    }
}
