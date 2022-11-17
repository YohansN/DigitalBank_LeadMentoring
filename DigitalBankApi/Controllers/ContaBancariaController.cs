using DigitalBankApi.Dtos;
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

        /// <summary>
        /// Busca por todos as ContasBancarias cadastradas no banco de dados.
        /// </summary>
        /// <returns>Retorna List(ContaBancaria) caso exista uma ou mais ContaBancaria. Caso contrario retorna Null</returns>
        [HttpGet("busca_conta_bancaria")]
        public async Task<IActionResult> GetAll()
        {
            var listContaBancaria = await _contaBancariaService.GetAll();
            if (listContaBancaria.Count == 0)
                return NotFound("A lista está vazia.");
            return Ok(listContaBancaria);
        }

        /// <summary>
        /// Busca por uma ContaBancaria cadastrada no banco de dados.
        /// </summary>
        /// <remarks>O Cpf não deve ser vazio ou nulo.</remarks>
        /// <param name="cpf"></param>
        /// <returns>Retorna um objeto ContaBancaria.</returns>
        [HttpGet("busca_conta_bancaria_por_cpf/{cpf}")]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return BadRequest("O cpf não pode ser nulo ou vazio");

            var contaBancaria = await _contaBancariaService.GetByCpf(cpf);
            if (contaBancaria != null)
                return Ok(contaBancaria);
            return NotFound("A conta bancaria vinculada a esse Cpf não existe.");
        }

        /// <summary>
        /// Adiciona uma ContaBancaria ao banco de dados.
        /// </summary>
        /// <remarks>O Id deve ser positivo diferente de zero. O Nome não deve ser vazio ou nulo. O saldo deve ser positivo diferente de zero.</remarks>
        /// <param name="contaBancaria"></param>
        /// <returns>Em caso de sucesso retorna um objeto ContaBancaria, caso contrario retorna null.</returns>
        [HttpPost("cadastro_conta_bancaria")]
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

        /// <summary>
        /// Apaga uma ContaBancaria de acordo com seu Numero da Conta.
        /// </summary>
        /// <remarks>O numero da conta deve ser positivo diferente de zero.</remarks>
        /// <param name="numeroConta"></param>
        /// <returns>Não tem retorno.</returns>
        [HttpDelete("apaga_conta_bancaria_por_cpf{NumeroConta}")]
        public async Task<IActionResult> Delete(int numeroConta)
        {
            if (numeroConta <= 0)
                return BadRequest("Apagamento de conta impossibilitado.\nO id é inválido.\nApenas Id's positivos e maiores que zero são validos.");

            var contaBancariaIsDeleted = await _contaBancariaService.Delete(numeroConta);
            if (contaBancariaIsDeleted)
                return Ok();
            return NotFound("Não existe Conta Bancária com esse número.");
        }

        /// <summary>
        /// Deposita (adiciona) um valor do saldo da ContaBancaria.
        /// </summary>
        /// <remarks>O NumeroConta deve ser positivo diferente de zero. O valor a ser depositado deve ser positivo diferente de zero.</remarks>
        /// <param name="numeroConta"></param>
        /// <param name="depositoDto"></param>
        /// <returns>Não tem retorno.</returns>
        [HttpPut("deposito")]
        public async Task<IActionResult> Deposito([FromQuery] int numeroConta, DepositoDebitoDto depositoDto)
        {
            
            if (numeroConta <= 0)
                return BadRequest("Deposito inpossibilitado.\nO Numero da Conta Bancaria é inválido.\nApenas numeros positivos e maiores que zero são validos.");
            else if (depositoDto.Saldo <= 0)
                return BadRequest("Deposito inpossibilitado.\nO valor a ser depositado não pode ser negativo ou zero.");

            var depositoWasMade = await _contaBancariaService.Deposito(numeroConta, depositoDto);
            if (depositoWasMade)
                return Ok();
            return BadRequest("Houve um erro ao depositar.");
        }

        /// <summary>
        /// Debita (retira) um valor do saldo da ContaBancaria.
        /// </summary>
        /// <remarks>O NumeroConta deve ser positivo diferente de zero. O valor a ser debitado deve ser positivo diferente de zero.</remarks>
        /// <param name="numeroConta"></param>
        /// <param name="debitoDto"></param>
        /// <returns>Não tem retorno.</returns>
        [HttpPut("debito")]
        public async Task<IActionResult> Debito([FromQuery]int numeroConta, DepositoDebitoDto debitoDto)
        {
            if (numeroConta <= 0)
                return BadRequest("Debito inpossibilitado.\nO Numero da Conta Bancaria é inválido.\nApenas numeros positivos e maiores que zero são validos.");
            else if (debitoDto.Saldo <= 0)
                return BadRequest("Debito inpossibilitado.\nO valor a ser debitado não pode ser negativo ou zero.");

            var debitoWasMade = await _contaBancariaService.Debito(numeroConta, debitoDto);
            if (debitoWasMade)
                return Ok();
            return BadRequest("Houve um erro ao depositar.");
        }
    }
}
