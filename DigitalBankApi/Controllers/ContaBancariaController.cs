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
        /// Adiciona uma ContaBancaria ao banco de dados.
        /// </summary>
        /// <remarks>
        /// O Id deve ser positivo diferente de zero. 
        /// O Nome não deve ser vazio ou nulo. 
        /// O saldo deve ser positivo diferente de zero.
        /// </remarks>
        /// <param name="contaBancariaDto"></param>
        /// <returns>Em caso de sucesso retorna um objeto ContaBancaria, caso contrario retorna null.</returns>
        [HttpPost("cadastro_conta_bancaria")]
        public async Task<IActionResult> Add(AddContaBancariaDto contaBancariaDto)
        {
            if (contaBancariaDto.IdCliente <= 0)
                return BadRequest("Cadastro impossibilitado.\nO Id é inválido.\nApenas Id's positivos e maiores que zero são validos.");
            else if (contaBancariaDto.Saldo <= 0)
                return BadRequest("Cadastro impossibilitado.\nSaldo inválido.\nO saldo inicial da conta deve ser positivo diferente de zero.");

            var contaBancariaIsCreated = await _contaBancariaService.Add(contaBancariaDto);
            if (contaBancariaIsCreated)
                return Created("A conta foi cadastrada com sucesso!", contaBancariaDto);
            return BadRequest("Falha ao cadastrar conta.");
        }

        /// <summary>
        /// Busca por todos as ContasBancarias cadastradas no banco de dados.
        /// </summary>
        /// <returns>Retorna List(ContaBancaria) caso exista uma ou mais ContaBancaria. Caso contrario retorna Null</returns>
        [HttpGet("busca_contas_bancarias")]
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
        /// Deposita (adiciona) um valor do saldo da ContaBancaria.
        /// </summary>
        /// <remarks>
        /// O NumeroConta deve ser positivo diferente de zero. 
        /// O valor a ser depositado deve ser positivo diferente de zero.
        /// </remarks>
        /// <param name="numeroConta"></param>
        /// <param name="depositoDto"></param>
        /// <returns>Não tem retorno.</returns>
        [HttpPut("deposito/{numeroConta}")]
        public async Task<IActionResult> Deposito(int numeroConta, DepositoDebitoDto depositoDto)
        {
            
            if (numeroConta <= 0)
                return BadRequest("Deposito inpossibilitado.\nO Numero da Conta Bancaria é inválido.\nApenas numeros positivos e maiores que zero são validos.");
            else if (depositoDto.Saldo <= 0)
                return BadRequest("Deposito inpossibilitado.\nO valor a ser depositado não pode ser negativo ou zero.");

            var depositoWasMade = await _contaBancariaService.Deposito(numeroConta, depositoDto);
            if (depositoWasMade)
                return Ok("Deposito realizado com sucesso.");
            return BadRequest("Houve um erro ao depositar.\n-Não existe conta bancária com esse número.");
        }

        /// <summary>
        /// Debita (retira) um valor do saldo da ContaBancaria.
        /// </summary>
        /// <remarks>
        /// O NumeroConta deve ser positivo diferente de zero. 
        /// O valor a ser debitado deve ser positivo diferente de zero.
        /// </remarks>
        /// <param name="numeroConta"></param>
        /// <param name="debitoDto"></param>
        /// <returns>Não tem retorno.</returns>
        [HttpPut("debito/{numeroConta}")]
        public async Task<IActionResult> Debito(int numeroConta, DepositoDebitoDto debitoDto)
        {
            if (numeroConta <= 0)
                return BadRequest("Debito inpossibilitado.\nO Numero da Conta Bancaria é inválido.\nApenas numeros positivos e maiores que zero são validos.");
            else if (debitoDto.Saldo <= 0)
                return BadRequest("Debito inpossibilitado.\nO valor a ser debitado não pode ser negativo ou zero.");

            var debitoWasMade = await _contaBancariaService.Debito(numeroConta, debitoDto);
            if (debitoWasMade)
                return Ok();
            return BadRequest("Houve um erro ao depositar.\n-Não existe conta bancária com esse número.\nOu\n-Valor a debitado é maior que o saldo da conta.");
        }

        /// <summary>
        /// Transfere um valor de uma conta (origem) para outra conta (destino).
        /// </summary>
        /// <remarks>
        /// Os NumeroConta devem ser positivo diferente de zero.
        /// O valor a ser transferido deve ser positivo diferente de zero.
        /// O valor transferido não pode ser maior que o saldo da conta de origem.
        /// </remarks>
        /// <param name="numeroContaOrigem"></param>
        /// <param name="numeroContaDestino"></param>
        /// <param name="transferenciaDto"></param>
        /// <returns>Não tem retorno.</returns>
        [HttpPut("transferencia")]
        public async Task<IActionResult> Transferencia([FromQuery]int numeroContaOrigem, [FromQuery]int numeroContaDestino, TransferenciaDto transferenciaDto)
        {
            if (numeroContaOrigem <= 0 || numeroContaDestino <= 0)
                return BadRequest("O número das contas bancarias não pode ser negativo ou zero.");
            else if (transferenciaDto.Saldo <= 0)
                return BadRequest("O valor a ser transferido não pode ser negativo ou zero.");
            var transacaoWasMade = await _contaBancariaService.Transferencia(numeroContaOrigem, numeroContaDestino, transferenciaDto);
            if (transacaoWasMade)
                return Ok();
            return BadRequest("Houve um erro na transação.\n-Não existe conta bancária com esse número (Conta de origem e/ou Conta de destino).\nOu\n-Valor a debitado é maior que o saldo da conta de origem.");
        }

        /// <summary>
        /// Busca todas as transações realizadas por uma conta bancaria (extrato) a partir do seu numero da conta.
        /// </summary>
        /// <remarks>Os NumeroConta devem ser positivo diferente de zero.</remarks>
        /// <param name="numeroConta"></param>
        /// <returns></returns>
        [HttpGet("busca_extrato_bancario_por_numero_da_conta/{numeroConta}")]
        public async Task<IActionResult> GetExtratoByNumeroConta(int numeroConta)
        {
            if (numeroConta <= 0)
                return BadRequest("O Numero da Conta Bancaria é inválido.\nApenas numeros positivos e maiores que zero são validos.");
            var listTransacoes = await _contaBancariaService.GetExtratoByNumeroConta(numeroConta);
            if (listTransacoes == null)
                return BadRequest("Esta conta bancaria não existe.");
            else if (listTransacoes.Count == 0)
                return NotFound("Esta conta bancaria ainda não realizou nenhuma transação.");
            return Ok(listTransacoes);
        }

        /// <summary>
        /// Apaga uma ContaBancaria de acordo com seu Numero da Conta.
        /// </summary>
        /// <remarks>O numero da conta deve ser positivo diferente de zero.</remarks>
        /// <param name="numeroConta"></param>
        /// <returns>Não tem retorno.</returns>
        [HttpDelete("apaga_conta_bancaria_por_numero_da_conta/{numeroConta}")]
        public async Task<IActionResult> Delete(int numeroConta)
        {
            if (numeroConta <= 0)
                return BadRequest("Apagamento de conta impossibilitado.\nO id é inválido.\nApenas Id's positivos e maiores que zero são validos.");

            var contaBancariaIsDeleted = await _contaBancariaService.Delete(numeroConta);
            if (contaBancariaIsDeleted)
                return Ok();
            return NotFound("Não existe Conta Bancária com esse número.");
        }
    }
}
