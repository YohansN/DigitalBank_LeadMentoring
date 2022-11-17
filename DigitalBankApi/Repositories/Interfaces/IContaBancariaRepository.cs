using DigitalBankApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalBankApi.Repositories.Interfaces
{
    public interface IContaBancariaRepository
    {
        Task<List<ContaBancaria>> GetAll();
        Task<ContaBancaria> GetByNumeroConta(int numeroConta);
        Task<ContaBancaria> GetByClienteId(int id);
        Task Add(ContaBancaria contaBancaria);
        Task Delete(ContaBancaria contaBancaria);
        Task Deposito(ContaBancaria contaBancaria);
        Task Debito(ContaBancaria contaBancaria);
        Task Transferencia(ContaBancaria contaOrigem, ContaBancaria contaDestino);
        Task<bool> IdExists(int id);
        Task<bool> NumeroContaExists(int numeroConta);
    }
}
