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
        //public List<Transacao> ListaTransacoes { get; set; }
    }
}
