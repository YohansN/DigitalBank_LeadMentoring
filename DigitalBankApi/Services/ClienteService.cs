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

        public async Task<bool> Add(AddClienteDto clienteDto)
        {
            var cpfExists = await _clienteRepository.CpfExists(clienteDto.Cpf);
            if (clienteDto.Idade < 18 || cpfExists)
                return false;
            Cliente cliente = new Cliente(clienteDto.Nome, clienteDto.Cpf, clienteDto.Idade);
            await _clienteRepository.Add(cliente);
            return true;
        }

        public async Task<bool> Update(int idCliente, UpdateClienteDto clienteDto)
        {
            var idExists = await _clienteRepository.IdExists(idCliente);
            if (idExists)
            {
                if (clienteDto.Idade < 18)
                    return false;
                var cliente = await _clienteRepository.GetById(idCliente);
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
