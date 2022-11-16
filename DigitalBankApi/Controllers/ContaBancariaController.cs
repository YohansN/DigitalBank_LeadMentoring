using DigitalBankApi.Models;
using DigitalBankApi.Services;
using DigitalBankApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DigitalBankApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaBancariaController : ControllerBase
    {
        private readonly IContaBancariaService _contaBancariaService;
        public ContaBancariaController(IContaBancariaService contaBancariaService)
        {
            _contaBancariaService = contaBancariaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var listContaBancaria = await _contaBancariaService.GetAll();
            if (listContaBancaria.Count == 0)
                return NotFound("A lista está vazia.");
            return Ok(listContaBancaria);
        }

        [HttpGet("{cpf}")]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return BadRequest("O cpf não pode ser nulo ou vazio");

            var contaBancaria = await _contaBancariaService.GetByCpf(cpf);
            if (contaBancaria != null)
                return Ok(contaBancaria);
            return NotFound("A conta bancaria vinculada a esse Cpf não existe.");
        }

        [HttpPost]
        public async Task<IActionResult> Add(ContaBancaria contaBancaria)
        {
            if (contaBancaria.IdCliente <= 0)
                return BadRequest("Cadastro impossibilitado.\nO Id é inválido.\nApenas Id's positivos e maiores que zero são validos.");
            else if (contaBancaria.NumeroConta <= 0)
                return BadRequest("Cadastro impossibilitado.\nO Numero da Conta Bancaria é inválido.\nApenas numeros positivos e maiores que zero são validos.");
            else if (contaBancaria.Saldo <= 0)
                return BadRequest("Cadastro impossibilitado.\nSaldo inválido.\nO saldo inicial da conta deve ser positivo diferente de zero.");

                var contaBancariaIsCreated = await _contaBancariaService.Add(contaBancaria);
            if (contaBancariaIsCreated)
                return Created("A conta foi cadastrada com sucesso!", contaBancaria);
            return BadRequest("Falha ao cadastrar conta.");
        }

        [HttpDelete("{NumeroConta}")]
        public async Task<IActionResult> Delete(int numeroConta)
        {
            if (numeroConta <= 0)
                return BadRequest("Apagamento de conta impossibilitado.\nO id é inválido.\nApenas Id's positivos e maiores que zero são validos.");

            var contaBancariaIsDeleted = await _contaBancariaService.Delete(numeroConta);
            if (contaBancariaIsDeleted)
                return Ok();
            return NotFound("Não existe Conta Bancária com esse número.");
        }

        [HttpPut("deposito")]
        public async Task<IActionResult> Deposito(ContaBancaria contaBancaria)
        {
            if (contaBancaria.IdCliente <= 0)
                return BadRequest("Deposito inpossibilitado.\nO Id é inválido.\nApenas Id's positivos e maiores que zero são validos.");
            else if (contaBancaria.NumeroConta <= 0)
                return BadRequest("Deposito inpossibilitado.\nO Numero da Conta Bancaria é inválido.\nApenas numeros positivos e maiores que zero são validos.");
            else if (contaBancaria.Saldo <= 0)
                return BadRequest("Deposito inpossibilitado.\nO valor a ser depositado não pode ser negativo ou zero.");
            
            var depositoWasMade = await _contaBancariaService.Deposito(contaBancaria);
            if (depositoWasMade)
                return Ok(depositoWasMade);
            return BadRequest("Houve um erro ao depositar.");
        }

        [HttpPut("debito")]
        public async Task<IActionResult> Debito(ContaBancaria contaBancaria)
        {
            if (contaBancaria.IdCliente <= 0)
                return BadRequest("Debito inpossibilitado.\nO Id é inválido.\nApenas Id's positivos e maiores que zero são validos.");
            else if (contaBancaria.NumeroConta <= 0)
                return BadRequest("Debito inpossibilitado.\nO Numero da Conta Bancaria é inválido.\nApenas numeros positivos e maiores que zero são validos.");
            else if (contaBancaria.Saldo <= 0)
                return BadRequest("Debito inpossibilitado.\nO valor a ser debitado não pode ser negativo ou zero.");

            var debitoWasMade = await _contaBancariaService.Debito(contaBancaria);
            if (debitoWasMade)
                return Ok(debitoWasMade);
            return BadRequest("Houve um erro ao depositar.");
        }
    }
}
