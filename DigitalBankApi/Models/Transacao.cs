using DigitalBankApi.Dtos;
using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalBankApi.Models
{
    public enum TipoTransacao { Debito, Deposito, Transferencia_Enviada, Transferencia_Recebida }
    public class Transacao
    {
        [Key]
        public int IdTransacao { get; set; }
        public int NumeroConta { get; set; }
        public TipoTransacao _TipoTransacao { get; set; }
        public decimal ValorTransacao { get; set; }
        public DateTime DataTransacao { get; set; }

        public Transacao(int numeroConta, TipoTransacao tipoTransacao, decimal valorTransacao, DateTime dataTransacao)
        {
            NumeroConta = numeroConta;
            _TipoTransacao = tipoTransacao;
            ValorTransacao = valorTransacao;
            DataTransacao = dataTransacao;
        }

    }
}
