using DigitalBankApi.Dtos;
using DigitalBankApi.Models;
using DigitalBankApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System.Threading.Tasks;

namespace DigitalBankApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// Cadastra um Cliente no banco de dados.
        /// </summary>
        /// <remarks>O Id deve ser positivo diferente de zero. O Nome não deve ser vazio ou nulo.</remarks>
        /// <param name="clienteDto"></param>
        /// <returns>Não tem retorno.</returns>
        [HttpPost("cadastro_cliente")]
        public async Task<IActionResult> Add(AddClienteDto clienteDto)
        {
            if (string.IsNullOrEmpty(clienteDto.Nome))
                return BadRequest("O Nome é invalido.");
            else if (string.IsNullOrEmpty(clienteDto.Cpf))
                return BadRequest("O Cpf é invalido.");

            var addCliente = await _clienteService.Add(clienteDto);
            if (addCliente)
                return Created("O cliente foi cadastrado.", clienteDto);
            return BadRequest("Falha ao cadastrar cliente:\n - O cliente não pode ser menor de idade. \n - Esse id já existe. \n - Esse CPF já existe.");
        }

        /// <summary>
        /// Atualiza os dados em Cliente.
        /// </summary>
        /// <remarks>O Id deve ser positivo diferente de zero. O Nome não deve ser vazio ou nulo.</remarks>
        /// <param name="id"></param>
        /// <param name="clienteDto"></param>
        /// <returns>Não tem retorno.</returns>
        [HttpPut("atualiza_perfil_cliente/{id}")]
        public async Task<IActionResult> Update(int id, UpdateClienteDto clienteDto)
        {
            if (id <= 0)
                return BadRequest("O Id é invalido. Apenas Id's positivos e maiores que zero são validos.");
            else if (string.IsNullOrEmpty(clienteDto.Nome))
                return BadRequest("O Nome é invalido.");

            var updateCliente = await _clienteService.Update(id, clienteDto);
            if (updateCliente)
                return Ok("Dados atualizados com sucesso.");
            return BadRequest("Falha ao atualizar cliente.\n-Não existe cliente com esse id.\nOu\nIdade não permitida.");
        }

        /// <summary>
        /// Busca por todos os Clientes cadastrados no banco de dados.
        /// </summary>
        /// <returns>Retorna List(Cliente) caso exista um ou mais Cliente. Caso contrario retorna Null</returns>
        [HttpGet("/busca_clientes")]
        public async Task<IActionResult> GetAll()
        {
            var listCliente = await _clienteService.GetAll();
            if (listCliente.Count == 0)
                return NotFound("A lista está vazia.");
            return Ok(listCliente);
        }

        /// <summary>
        /// Busca por um Cliente cadastrado no banco de dados.
        /// </summary>
        /// <remarks>O Id deve ser positivo diferente de zero.</remarks>
        /// <param name="id"></param>
        /// <returns>Caso exista um Cliente com esse Id: Retorna um objeto Cliente. Caso contrário retorna NotFound.</returns>
        [HttpGet("busca_cliente_por_id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("O Id é invalido. Apenas Id's positivos e maiores que zero são validos.");

            var cliente = await _clienteService.GetById(id);
            if (cliente != null)
                return Ok(cliente);
            return NotFound("Não existe um cliente cadastrado com esse id.");
        }

        /// <summary>
        /// Apaga um Cliente do banco de dados de acordo com seu Id.
        /// </summary>
        /// <remarks>O Id deve ser positivo diferente de zero.</remarks>
        /// <param name="id"></param>
        /// <returns>Não tem retorno.</returns>
        [HttpDelete("apaga_conta_cliente_por_id/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("O Id é invalido. Apenas Id's positivos e maiores que zero são validos.");
            var deleteCliente = await _clienteService.Delete(id);
            if (deleteCliente)
                return Ok("Cliente apagado com sucesso.");
            return BadRequest("Falha ao deletar cliente:\n - Cliente inexistente.");
        }
    }
}
