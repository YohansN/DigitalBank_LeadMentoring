using DigitalBankApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalBankApi.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> GetAll();
        Task<Cliente> GetById(int id);
        Task Add(Cliente cliente);
        Task Update(Cliente cliente);
        Task Delete(int id);
        Task<bool> IdExists(int id);
        Task<bool> CpfExists(string cpf);
    }
}
