using DigitalBankApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalBankApi.Repositories.Interfaces
{
    public interface IContaBancariaRepository
    {
        Task<List<ContaBancaria>> GetAll();
        Task<Cliente> GetByCpf(string cpf);
        Task Add(ContaBancaria contaBancaria);
        Task Deposito(ContaBancaria contaBancaria);
        Task Debito(ContaBancaria contaBancaria);
        Task Transferencia(ContaBancaria contaBancaria);
        Task<bool> IdExists(int id);
    }
}
