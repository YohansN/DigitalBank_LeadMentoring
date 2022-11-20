using DigitalBankApi.Controllers;
using DigitalBankApi.Dtos;
using DigitalBankApi.Models;
using DigitalBankApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DigitalBankApiTest
{
    public class ContaBancariaUnitTests
    {
        private readonly ContaBancariaController _contaBancariaController;
        private readonly Mock<IContaBancariaService> _contaBancariaServiceMock;
        public ContaBancariaUnitTests()
        {
            _contaBancariaServiceMock = new Mock<IContaBancariaService>();
            _contaBancariaController = new ContaBancariaController(_contaBancariaServiceMock.Object);
        }

        //ContaBancariaGetByCpf---------------------------------------------------------------------------------------------------
        [Theory]
        [InlineData("00000000001")]
        public async Task ContaBancariaGetByCpf_SholdReturnOk_WhenCpfIsValid(string cpf)
        {
            //Arrange
            ContaBancaria contaBancariaMockResult = new ContaBancaria { };
            _contaBancariaServiceMock.Setup(clienteService => clienteService.GetByCpf(cpf)).Returns(Task.FromResult(contaBancariaMockResult));

            //Act
            var response = await _contaBancariaController.GetByCpf(cpf);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(200, statusCode);
        }

        [Theory]
        [InlineData("")]
        public async Task ContaBancariaGetByCpf_SholdReturnBadRequest_WhenCpfIsInvalid(string cpf)
        {
            //Arrange
            

            //Act
            var response = await _contaBancariaController.GetByCpf(cpf);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(400, statusCode);
        }
        
        //ContaBancariaAdd---------------------------------------------------------------------------------------------------
        [Theory]
        [InlineData(1, 10)]
        public async Task ContaBancariaAdd_SholdReturnCreated_WhenIdAndSaldoAreValid(int id, decimal saldo)
        {
            //Arrange
            AddContaBancariaDto contaBancariaMockResult = new AddContaBancariaDto { IdCliente = id, Saldo = saldo };
            _contaBancariaServiceMock.Setup(clienteService => clienteService.Add(contaBancariaMockResult)).Returns(Task.FromResult(true));

            //Act
            var response = await _contaBancariaController.Add(contaBancariaMockResult);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(201, statusCode);
        }

        [Theory]
        [InlineData(-1, 10)]
        [InlineData(1, -10)]
        [InlineData(-1, -10)]
        public async Task ContaBancariaAdd_SholdReturnBadRequest_WhenIdOrSaldoAreInvalid(int id, decimal saldo)
        {
            //Arrange
            AddContaBancariaDto contaBancariaMockResult = new AddContaBancariaDto { IdCliente = id, Saldo = saldo };

            //Act
            var response = await _contaBancariaController.Add(contaBancariaMockResult);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(400, statusCode);
        }

        //ContaBancariaDelete---------------------------------------------------------------------------------------------------
        [Theory]
        [InlineData(1)]
        public async Task ContaBancariaDelete_SholdReturnOk_WhenNumeroContaIsValid(int numeroConta)
        {
            //Arrange
            _contaBancariaServiceMock.Setup(clienteService => clienteService.Delete(numeroConta)).Returns(Task.FromResult(true));

            //Act
            var response = await _contaBancariaController.Delete(numeroConta);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(200, statusCode);
        }

        [Theory]
        [InlineData(1)]
        public async Task ContaBancariaDelete_SholdReturnNotFound_WhenNumeroContaDoNotExist(int numeroConta)
        {
            //Arrange
            _contaBancariaServiceMock.Setup(clienteService => clienteService.Delete(numeroConta)).Returns(Task.FromResult(false));

            //Act
            var response = await _contaBancariaController.Delete(numeroConta);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(404, statusCode);
        }
        [Theory]
        [InlineData(-1)]
        public async Task ContaBancariaDelete_SholdReturnBadRequest_WhenNumeroContaIsInvalid(int numeroConta)
        {
            //Arrange


            //Act
            var response = await _contaBancariaController.Delete(numeroConta);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(400, statusCode);
        }

        //ContaBancariaDeposito---------------------------------------------------------------------------------------------------
        [Theory]
        [InlineData(1, 10)]
        public async Task ContaBancariaDeposito_SholdReturnOk_WhenNumeroContaAndSaldoAreValid(int numeroConta, decimal saldo)
        {
            //Arrange
            DepositoDebitoDto contaBancariaMockResult = new DepositoDebitoDto { Saldo = saldo };
            _contaBancariaServiceMock.Setup(clienteService => clienteService.Deposito(numeroConta, contaBancariaMockResult)).Returns(Task.FromResult(true));

            //Act
            var response = await _contaBancariaController.Deposito(numeroConta, contaBancariaMockResult);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(200, statusCode);
        }

        [Theory]
        [InlineData(-1, 10)]
        [InlineData(1, -10)]
        [InlineData(-1, -10)]
        public async Task ContaBancariaDeposito_SholdReturnBadRequest_WhenNumeroContaOrSaldoAreInvalid(int numeroConta, decimal saldo)
        {
            //Arrange
            DepositoDebitoDto contaBancariaMockResult = new DepositoDebitoDto { Saldo = saldo };

            //Act
            var response = await _contaBancariaController.Deposito(numeroConta, contaBancariaMockResult);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(400, statusCode);
        }

        //ContaBancariaDebito---------------------------------------------------------------------------------------------------
        [Theory]
        [InlineData(1, 10)]
        public async Task ContaBancariaDebito_SholdReturnOk_WhenNumeroContaAndSaldoAreValid(int numeroConta, decimal saldo)
        {
            //Arrange
            DepositoDebitoDto contaBancariaMockResult = new DepositoDebitoDto { Saldo = saldo };
            _contaBancariaServiceMock.Setup(clienteService => clienteService.Debito(numeroConta, contaBancariaMockResult)).Returns(Task.FromResult(true));

            //Act
            var response = await _contaBancariaController.Debito(numeroConta, contaBancariaMockResult);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(200, statusCode);
        }

        [Theory]
        [InlineData(-1, 10)]
        [InlineData(1, -10)]
        [InlineData(-1, -10)]
        public async Task ContaBancariaDebito_SholdReturnBadRequest_WhenNumeroContaOrSaldoAreInvalid(int numeroConta, decimal saldo)
        {
            //Arrange
            DepositoDebitoDto contaBancariaMockResult = new DepositoDebitoDto { Saldo = saldo };

            //Act
            var response = await _contaBancariaController.Debito(numeroConta, contaBancariaMockResult);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(400, statusCode);
        }

        //ContaBancariaTransacao---------------------------------------------------------------------------------------------------
        [Theory]
        [InlineData(1, 2, 10)]
        public async Task ContaBancariaTransferencia_SholdReturnOk_WhenNumeroContasAndSaldoAreValid(int numeroContaOrigem, int numeroContaDestino, decimal saldo)
        {
            //Arrange
            TransferenciaDto contaBancariaMockResult = new TransferenciaDto { Saldo = saldo };
            _contaBancariaServiceMock.Setup(clienteService => clienteService.Transferencia(numeroContaOrigem, numeroContaDestino, contaBancariaMockResult)).Returns(Task.FromResult(true));

            //Act
            var response = await _contaBancariaController.Transferencia(numeroContaOrigem, numeroContaDestino, contaBancariaMockResult);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(200, statusCode);
        }

        [Theory]
        [InlineData(-1, 2, 10)]
        [InlineData(1, -2, 10)]
        [InlineData(1, 2, -10)]
        [InlineData(-1, -2, -10)]
        public async Task ContaBancariaTransferencia_SholdReturnBadRequest_WhenNumeroContasOrSaldoAreInvalid(int numeroContaOrigem, int numeroContaDestino, decimal saldo)
        {
            //Arrange
            TransferenciaDto contaBancariaMockResult = new TransferenciaDto { Saldo = saldo };
            

            //Act
            var response = await _contaBancariaController.Transferencia(numeroContaOrigem, numeroContaDestino, contaBancariaMockResult);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(400, statusCode);
        }
        
    }
}
