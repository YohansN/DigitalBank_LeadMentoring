using DigitalBankApi.Models;
using DigitalBankApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var listCliente = await _clienteService.GetAll();
            if (listCliente.Count == 0)
                return NotFound("A lista está vazia.");
            return Ok(await _clienteService.GetAll());
        }

        //Fazer a verificacao se o Id de entrada é valido. (Nao negativo)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("O Id é invalido. Apenas Id's positivos e maiores que zero são validos.");

            var cliente = await _clienteService.GetById(id);
            if (cliente != null)
                return Ok(cliente);
            return NotFound("Não existe um cliente cadastrado com esse id.");
        }

        //Fazer a verificação se o Id e o Nome de entrada são validos.
        [HttpPost]
        public async Task<IActionResult> Add(Cliente cliente)
        {
            if (cliente.IdCliente <= 0)
                return BadRequest("O Id é invalido. Apenas Id's positivos e maiores que zero são validos.");
            else if(string.IsNullOrEmpty(cliente.Nome))
                return BadRequest("O Nome é invalido.");

            var addCliente = await _clienteService.Add(cliente);
            if(addCliente)
                return Created("O cliente foi cadastrado.", cliente);
            return BadRequest("Falha ao cadastrar cliente:\n - O cliente não pode ser menor de idade. \n - Esse id já existe. \n - Esse CPF já existe.");
        }

        //Fazer a verificação se o Id e o Nome de entrada são validos.
        [HttpPut]
        public async Task<IActionResult> Update(Cliente cliente)
        {
            if (cliente.IdCliente <= 0)
                return BadRequest("O Id é invalido. Apenas Id's positivos e maiores que zero são validos.");
            else if (string.IsNullOrEmpty(cliente.Nome))
                return BadRequest("O Nome é invalido.");

            var updateCliente = await _clienteService.Update(cliente);
            if (updateCliente)
                return NoContent();
            return BadRequest("Falha ao atualizar cliente.");
        }

        //Fazer a verificacao se o Id de entrada é valido. (Nao negativo)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("O Id é invalido. Apenas Id's positivos e maiores que zero são validos.");
            var deleteCliente = await _clienteService.Delete(id);
            if (deleteCliente)
                return Ok();
            return BadRequest("Falha ao deletear cliente:\n - Cliente inexistente.");
        }
    }
}
