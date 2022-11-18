using System.ComponentModel.DataAnnotations;

namespace DigitalBankApi.Models
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public int Idade { get; set; }

        public Cliente (string nome, string cpf, int idade)
        {
            Nome = nome;
            Cpf = cpf;
            Idade = idade;
        }
    }
}
