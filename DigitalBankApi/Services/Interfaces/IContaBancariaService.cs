using DigitalBankApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalBankApi.Services.Interfaces
{
    public interface IContaBancariaService
    {
        Task<List<ContaBancaria>> GetAll();
        Task<ContaBancaria> GetByCpf(string cpf);
        Task<bool> Add(ContaBancaria contaBancaria);
        Task<bool> Delete(int numeroConta);
        Task<bool> Deposito(ContaBancaria contaBancaria);
        Task<bool> Debito(ContaBancaria contaBancaria);
        //Task<bool> Transferencia(ContaBancaria contaBancaria);
        //Task<bool> TransacaoById(ContaBancaria contaBancaria);
    }
}
