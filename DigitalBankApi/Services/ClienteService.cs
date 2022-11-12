using DigitalBankApi.Models;
using DigitalBankApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalBankApi.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
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

        public async Task<bool> Update(Cliente cliente)
        {
            var cpfExists = await _clienteRepository.CpfExists(cliente.Cpf);
            var idExists = await _clienteRepository.IdExists(cliente.IdCliente);
            //Verificacao : Caso falha - Idade menor que 18; Id inexistente; Cpf já em uso (existente).
            if (cliente.Idade < 18 || !(idExists) || cpfExists)
                return false;
            await _clienteRepository.Update(cliente);
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            //Verifica se o id existe.
            if(await _clienteRepository.IdExists(id))
            {
                await _clienteRepository.Delete(id);
                return true;
            }
            return false;
        }
    }
}
