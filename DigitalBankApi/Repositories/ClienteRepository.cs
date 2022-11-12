using DigitalBankApi.Data;
using DigitalBankApi.Models;
using DigitalBankApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalBankApi.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly Context _context;
        public ClienteRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> GetAll()
        {
            var getAllCliente = await _context.Cliente.ToListAsync();
            return getAllCliente;
        }

        public async Task<Cliente> GetById(int id)
        {
            var getByIdCliente = await _context.Cliente.FirstOrDefaultAsync(c => c.IdCliente == id);
            return getByIdCliente;
        }

        public async Task Add(Cliente cliente)
        {
            await _context.Cliente.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }
        
        public async Task Update(Cliente cliente)
        {
            _context.Cliente.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var getByIdCliente = await GetById(id);
            _context.Cliente.Remove(getByIdCliente);
            await _context.SaveChangesAsync();
        }

        //Métodos de checagem no banco.
        public async Task<bool> IdExists(int id)
        {
            var exists = await _context.Cliente.AnyAsync(c => c.IdCliente == id);
            return exists;
        }

        public async Task<bool> CpfExists(string cpf)
        {
            var cpfExists = await _context.Cliente.AnyAsync(c => c.Cpf == cpf);
            return cpfExists;
        }
    }
}
