using DigitalBankApi.Dtos;
using DigitalBankApi.Models;
using DigitalBankApi.Repositories.Interfaces;
using DigitalBankApi.Services.Interfaces;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace DigitalBankApi.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IContaBancariaRepository _contaBancariaRepository;
        public ClienteService(IClienteRepository clienteRepository, IContaBancariaRepository contaBancariaRepository)
        {
            _clienteRepository = clienteRepository;
            _contaBancariaRepository = contaBancariaRepository;
        }
        /* REGRAS
         * No Add a idade tem que ser igual ou maior de 18 anos.
         * Validar CPF - Não podem existir pessoas com mesmo CPF
         * Validar CPF - Validar se esta no formato aceito (posteriormente) */

        public async Task<List<Cliente>> GetAll()
        {
            return await _clienteRepository.GetAll();
        }

        public async Task<Cliente> GetById(int id)
        {
            if(await _clienteRepository.IdExists(id))
                return await _clienteRepository.GetById(id);
            return null;
        }

        public async Task<bool> Add(Cliente cliente)
        {
            var cpfExists = await _clienteRepository.CpfExists(cliente.Cpf);
            var idExists = await _clienteRepository.IdExists(cliente.IdCliente);
            //Verificacao: Caso falha - Idade menor que 18; Id já em uso (existente); Cpf já em uso (existente);
            if (cliente.Idade < 18 || idExists || cpfExists)
                return false;
            await _clienteRepository.Add(cliente);
            return true;
        }

        public async Task<bool> Update(int idCliente, UpdateClienteDto clienteDto)
        {
            var idExists = await _clienteRepository.IdExists(idCliente);
            if (idExists)
            {
                var cliente = await _clienteRepository.GetById(idCliente);

                if (string.IsNullOrEmpty(clienteDto.Nome) || clienteDto.Idade < 18)
                    return false;
                cliente.Nome = clienteDto.Nome;
                cliente.Idade = clienteDto.Idade;
                await _clienteRepository.Update(cliente);
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var clienteExists = await _clienteRepository.IdExists(id);
            var contaBancariaOfThisClienteExists = await _contaBancariaRepository.IdExists(id);
            if (clienteExists && contaBancariaOfThisClienteExists)
            {
                var contaBancariaOfThisCliente = await _contaBancariaRepository.GetByClienteId(id);
                await _contaBancariaRepository.Delete(contaBancariaOfThisCliente);

                await _clienteRepository.Delete(id);
                return true;
            }
            else if (clienteExists)
            {
                await _clienteRepository.Delete(id);
                return true;
            }
            return false;
        }
    }
}
