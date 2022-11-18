using DigitalBankApi.Data;
using DigitalBankApi.Models;
using DigitalBankApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalBankApi.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly Context _context;
        public TransacaoRepository(Context context)
        {
            _context = context;
        }

        //Add - Adiciona uma nova transacao quando é chamada.
        public async Task Add(Transacao transacao)
        {
            _context.Transacao.Add(transacao);
            await _context.SaveChangesAsync();
        }

        //GetAllByNumeroConta - Retorna todas as transacoes de uma conta.
        public async Task<List<Transacao>> GetExtratoByNumeroConta(int numeroContaBancaria)
        {
            var listaTransacoes = await _context.Transacao.Where(t => t.NumeroConta == numeroContaBancaria).ToListAsync();
            //var listaTransacoesByNumeroConta = (listaTransacoes.Where(t => t.NumeroConta == numeroContaBancaria)).ToList();
            //return listaTransacoesByNumeroConta;
            return listaTransacoes;
        }
    }
}
