using DigitalBankApi.Dtos;
using DigitalBankApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalBankApi.Services.Interfaces
{
    public interface IContaBancariaService
    {
        Task<List<ContaBancaria>> GetAll();
        Task<ContaBancaria> GetByCpf(string cpf);
        Task<List<Transacao>> GetExtratoByNumeroConta(int numeroContaBancaria);
        Task<bool> Add(AddContaBancariaDto contaBancariaDto);
        Task<bool> Delete(int numeroConta);
        Task<bool> Deposito(int numeroConta, DepositoDebitoDto depositoDto);
        Task<bool> Debito(int numeroConta, DepositoDebitoDto debitoDto);
        Task<bool> Transferencia(int numeroContaOrigem, int numeroContaDestino, TransferenciaDto transferenciaDto);
    }
}
