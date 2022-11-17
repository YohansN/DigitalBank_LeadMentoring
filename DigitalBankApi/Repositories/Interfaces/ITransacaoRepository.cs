using DigitalBankApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalBankApi.Repositories.Interfaces
{
    public interface ITransacaoRepository
    {
        Task Add(Transacao transacao);
        Task<List<Transacao>> GetAllByNumeroConta();
    }
}
