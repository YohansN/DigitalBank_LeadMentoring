using DigitalBankApi.Dtos;
using DigitalBankApi.Models;
using DigitalBankApi.Repositories.Interfaces;
using DigitalBankApi.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalBankApi.Services
{
    public class ContaBancariaService : IContaBancariaService
    {
        private readonly IContaBancariaRepository _contaBancariaRepository;
        private readonly IClienteRepository _clienteRepository;
        public ContaBancariaService(IContaBancariaRepository contaBancariaRepository, IClienteRepository clienteRepository)
        {
            _contaBancariaRepository = contaBancariaRepository;
            _clienteRepository = clienteRepository;
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
                if(cliente.IdCliente == contaBancaria.IdCliente)
                {
                    return contaBancaria;
                }
                return null;
            }
            return null;
        }

        public async Task<bool> Add(ContaBancaria contaBancaria)
        {
            /*
            - Numero conta não pode existir previamente.
            - IdCliente tem que existir. (Um cliente so pode ter uma conta bancaria e vice versa)
            - Saldo nao pode ser negativo.
            - Uma cliente so pode estar vinculado a UMA conta bancaria.
             */
            var contaBancariaExists = await _contaBancariaRepository.GetByNumeroConta(contaBancaria.NumeroConta);
            //var clienteExists = await _clienteRepository.GetById(contaBancaria.IdCliente);
            var clienteExists = await _clienteRepository.IdExists(contaBancaria.IdCliente);
            if (contaBancariaExists == null && clienteExists == true && contaBancaria.Saldo > 0)
            {
                //Verifica se aquela conta já está atrelada a algum cliente:
                var contaBancariaAlreadExists = await _contaBancariaRepository.IdExists(contaBancaria.IdCliente);
                if (contaBancariaAlreadExists)
                    return false;
                await _contaBancariaRepository.Add(contaBancaria);
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(int numeroConta)
        {
            if(await _contaBancariaRepository.NumeroContaExists(numeroConta))
            {
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
            if(contaOrigemExists && contaDestinoExists)
            {
                var contaOrigem = await _contaBancariaRepository.GetByNumeroConta(numeroContaOrigem);
                var contaDestino = await _contaBancariaRepository.GetByNumeroConta(numeroContaDestino);
                contaOrigem.Saldo -= transferenciaDto.Saldo;
                contaDestino.Saldo += transferenciaDto.Saldo;
                await _contaBancariaRepository.Transferencia(contaOrigem, contaDestino);
                return true;
            }
            return false;
        }
    }
}
