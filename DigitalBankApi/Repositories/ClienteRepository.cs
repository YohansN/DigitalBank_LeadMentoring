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

        public async Task<List<Cliente>> GetAll() => await _context.Cliente.ToListAsync();
        
        public async Task<Cliente> GetById(int id) => await _context.Cliente.FirstOrDefaultAsync(c => c.IdCliente == id);
        
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
            _context.Cliente.Remove(await GetById(id));
            await _context.SaveChangesAsync();
        }

        //Métodos de checagem no banco.
        public async Task<bool> IdExists(int id) => await _context.Cliente.AnyAsync(c => c.IdCliente == id);
        
        public async Task<bool> CpfExists(string cpf) => await _context.Cliente.AsNoTracking().AnyAsync(c => c.Cpf == cpf);

        public async Task<Cliente> GetByCpf(string cpf) => await _context.Cliente.AsNoTracking().FirstOrDefaultAsync(c => c.Cpf == cpf);
    }
}
