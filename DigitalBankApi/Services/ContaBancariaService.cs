using DigitalBankApi.Dtos;
using DigitalBankApi.Models;
using DigitalBankApi.Repositories.Interfaces;
using DigitalBankApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DigitalBankApi.Models.Transacao;

namespace DigitalBankApi.Services
{
    public class ContaBancariaService : IContaBancariaService
    {
        private readonly IContaBancariaRepository _contaBancariaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ITransacaoRepository _transacaoRepository;
        public ContaBancariaService(IContaBancariaRepository contaBancariaRepository, IClienteRepository clienteRepository, ITransacaoRepository transacaoRepository)
        {
            _contaBancariaRepository = contaBancariaRepository;
            _clienteRepository = clienteRepository;
            _transacaoRepository = transacaoRepository;
        }
        public async Task<List<ContaBancaria>> GetAll()
        {
            return await _contaBancariaRepository.GetAll();
        }

        public async Task<ContaBancaria> GetByCpf(string cpf)
        {
            if(await _clienteRepository.CpfExists(cpf))
            {
                var cliente = await _clienteRepository.GetByCpf(cpf);
                var contaBancaria = await _contaBancariaRepository.GetByClienteId(cliente.IdCliente);
                if(contaBancaria != null)
                {
                    if (cliente.IdCliente == contaBancaria.IdCliente)
                    {
                        return contaBancaria;
                    }
                    return null;
                }
                return null;
            }
            return null;
        }

        public async Task<bool> Add(AddContaBancariaDto contaBancariaDto)
        { 
            var clienteExists = await _clienteRepository.IdExists(contaBancariaDto.IdCliente);
            if (clienteExists == true && contaBancariaDto.Saldo > 0)
            {
                //Verifica se aquela conta já está atrelada a algum cliente:
                var contaBancariaAlreadExists = await _contaBancariaRepository.IdExists(contaBancariaDto.IdCliente);
                if (contaBancariaAlreadExists)
                    return false;
                ContaBancaria contaBancaria = new ContaBancaria(contaBancariaDto.IdCliente, contaBancariaDto.Saldo);
                await _contaBancariaRepository.Add(contaBancaria);
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(int numeroConta)
        {
            if(await _contaBancariaRepository.NumeroContaExists(numeroConta))
            {
                await _transacaoRepository.DeleteTransacoes(numeroConta);
                var contaBancariaToDelete = await _contaBancariaRepository.GetByNumeroConta(numeroConta);
                await _contaBancariaRepository.Delete(contaBancariaToDelete);
                return true;
            }
            return false;
        }

        public async Task<bool> Deposito(int numeroConta, DepositoDebitoDto depositoDto)
        {
            /*
            - O numero da conta tem que existir, o dinheiro vai pra ela.
            - Verificar se o IdCliente passado existe tambem.
            - Caso exista, o IdCliente não pode ser modificado, ou seja tem que ser igual ao que esta no banco de dados.
            - O numero da conta tem que estar atrelado ao IdCliente.
            - O saldo não pode ser menor que o saldo previamente existente.
            */
            if (await _contaBancariaRepository.NumeroContaExists(numeroConta))
            {
                var contaBancaria = await _contaBancariaRepository.GetByNumeroConta(numeroConta);
                contaBancaria.Saldo += depositoDto.Saldo;
                await _contaBancariaRepository.Deposito(contaBancaria);

                Transacao transacao = new Transacao(
                    contaBancaria.NumeroConta,
                    TipoTransacao.Deposito,
                    depositoDto.Saldo,
                    DateTime.Now);
                await _transacaoRepository.Add(transacao);
                return true;
            }
            return false;
        }

        public async Task<bool> Debito(int numeroConta, DepositoDebitoDto debitoDto)
        {
            /*
            - O numero da conta tem que existir, o dinheiro vai pra ela.
            - Se a conta existe o Cliente também mas devemos verificar 
            - Verificar se o IdCliente passado existe tambem.
            - Caso exista, o IdCliente não pode ser modificado, ou seja tem que ser igual ao que esta no banco de dados.
            - O numero da conta tem que estar atrelado ao IdCliente.
            - O saldo não pode ser maior que o saldo previamente existente.
            */
            if (await _contaBancariaRepository.NumeroContaExists(numeroConta)){
                var contaBancaria = await _contaBancariaRepository.GetByNumeroConta(numeroConta);
                if (debitoDto.Saldo <= contaBancaria.Saldo)
                {
                    contaBancaria.Saldo -= debitoDto.Saldo;
                    await _contaBancariaRepository.Debito(contaBancaria);

                    Transacao transacao = new Transacao(
                    contaBancaria.NumeroConta,
                    TipoTransacao.Debito,
                    -debitoDto.Saldo,
                    DateTime.Now);
                    await _transacaoRepository.Add(transacao);
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<bool> Transferencia(int numeroContaOrigem, int numeroContaDestino, TransferenciaDto transferenciaDto)
        {
            var contaOrigemExists = await _contaBancariaRepository.NumeroContaExists(numeroContaOrigem);
            var contaDestinoExists = await _contaBancariaRepository.NumeroContaExists(numeroContaDestino);
            if (contaOrigemExists && contaDestinoExists)
            {
                var contaOrigem = await _contaBancariaRepository.GetByNumeroConta(numeroContaOrigem);
                var contaDestino = await _contaBancariaRepository.GetByNumeroConta(numeroContaDestino);
                if (contaOrigem.Saldo < transferenciaDto.Saldo)
                    return false;

                contaOrigem.Saldo -= transferenciaDto.Saldo;
                contaDestino.Saldo += transferenciaDto.Saldo;
                await _contaBancariaRepository.Transferencia(contaOrigem, contaDestino);

                Transacao transacaoOrigem = new Transacao(
                    contaOrigem.NumeroConta,
                    TipoTransacao.Transferencia_Enviada,
                    -transferenciaDto.Saldo,
                    DateTime.Now);
                await _transacaoRepository.Add(transacaoOrigem);

                Transacao transacaoDestino = new Transacao(
                    contaDestino.NumeroConta,
                    TipoTransacao.Transferencia_Recebida,
                    transferenciaDto.Saldo,
                    DateTime.Now);
                await _transacaoRepository.Add(transacaoDestino);
                return true;
            }
            return false;
        }

        public async Task<List<Transacao>> GetExtratoByNumeroConta(int numeroContaBancaria)
        {
            if(await _contaBancariaRepository.NumeroContaExists(numeroContaBancaria))
            {
                var listaTransacoesByNumeroConta = await _transacaoRepository.GetExtratoByNumeroConta(numeroContaBancaria);
                return listaTransacoesByNumeroConta;
            }
            return null;
        }
    }
}
