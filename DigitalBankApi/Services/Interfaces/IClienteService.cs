using DigitalBankApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalBankApi.Services.Interfaces
{
    public interface IClienteService
    {
        Task<List<Cliente>> GetAll();
        Task<Cliente> GetById(int id);
        Task<bool> Add(Cliente cliente);
        Task<bool> Update(Cliente cliente);
        Task<bool> Delete(int id);
    }
}
