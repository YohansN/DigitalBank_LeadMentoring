using DigitalBankApi.Data;
using DigitalBankApi.Models;
using DigitalBankApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalBankApi.Repositories
{
    public class ContaBancariaRepository : IContaBancariaRepository
    {
        private readonly Context _context;
        public ContaBancariaRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<ContaBancaria>> GetAll() => await _context.ContaBancaria.ToListAsync();

        //GetByCpf -> Pego objeto CLIENTE pelo CPF e verificar se o cliente.IdCliente tem alguma conta em ContaBancaria.IdCliente.
        public async Task<Cliente> GetByCpf(string cpf) => await _context.Cliente.FirstOrDefaultAsync(c => c.Cpf == cpf);
        
        public async Task Add(ContaBancaria contaBancaria)
        {
            await _context.ContaBancaria.AddAsync(contaBancaria);
            await _context.SaveChangesAsync();
        }
        
        public async Task Deposito(ContaBancaria contaBancaria)
        {
            _context.ContaBancaria.Update(contaBancaria);
            await _context.SaveChangesAsync();
        }
        
        public async Task Debito(ContaBancaria contaBancaria)
        {
            _context.ContaBancaria.Update(contaBancaria);
            await _context.SaveChangesAsync();
        }
        
        public async Task Transferencia(ContaBancaria contaBancaria)
        {
            _context.ContaBancaria.Update(contaBancaria);
            await _context.SaveChangesAsync();
        }
        //GetAllTransacaoById

        //Métodos de checagem no banco.
        public async Task<bool> IdExists(int id) => await _context.ContaBancaria.AnyAsync(b => b.IdCliente == id);
    }
}
