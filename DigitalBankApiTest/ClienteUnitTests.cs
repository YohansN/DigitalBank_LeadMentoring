using DigitalBankApi.Controllers;
using DigitalBankApi.Services.Interfaces;
using System;
using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DigitalBankApi.Models;
using DigitalBankApi.Dtos;
using DigitalBankApi.Repositories.Interfaces;

namespace DigitalBankApiTest
{
    public class ClienteUnitTests
    {
        private readonly ClienteController _clienteController;
        private readonly Mock<IClienteService> _clienteServiceMock;
        public ClienteUnitTests()
        {
            _clienteServiceMock = new Mock<IClienteService>();
            _clienteController = new ClienteController(_clienteServiceMock.Object);
        }

        //ClienteGetById---------------------------------------------------------------------------------------------------------------------------
        [Theory]
        [InlineData(10)]
        public async Task ClienteGetById_SholdReturnOk_WhenIdIsValid(int id)
        {
            //Arrange
            Cliente clienteMockResult = new Cliente { IdCliente = id };
            _clienteServiceMock.Setup(clienteService => clienteService.GetById(id)).Returns(Task.FromResult(clienteMockResult));

            //Act
            var response = await _clienteController.GetById(id);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(200, statusCode);
        }

        [Theory]
        [InlineData(-2)]
        [InlineData(0)]
        public async Task ClienteGetById_SholdReturnBadRequest_WhenIdIsInvalid(int id)
        {
            //Arrange

            //Act
            var response = await _clienteController.GetById(id);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(400, statusCode);
        }

        //ClienteAdd---------------------------------------------------------------------------------------------------------------------------
        [Theory]
        [InlineData("Yohans","00000000000",20)]
        public async Task ClienteAdd_SholdReturnCreated_WhenNomeAndCpfAndIdadeAreValid(string nome, string cpf, int idade)
        {
            
            //Arrange
            AddClienteDto clienteMockResult = new AddClienteDto { Nome = nome, Cpf = cpf, Idade = idade };
            _clienteServiceMock.Setup(clienteService => clienteService.Add(clienteMockResult)).Returns(Task.FromResult(true));
            
            //Act
            var response = await _clienteController.Add(clienteMockResult);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(201, statusCode);
        }

        [Theory]
        [InlineData("", "00000000000", 20)] //Nome nulo
        [InlineData("Yohans", "", 20)] //Cpf nulo
        [InlineData("Yohans", "00000000000", 2)] //Idade menor que a requerida
        [InlineData("", "", 2)]
        public async Task ClienteAdd_SholdReturnBadRequest_WhenNomeOrCpfOrIdadeAreInvalid(string nome, string cpf, int idade)
        {
            //Arrange
            AddClienteDto cliente = new AddClienteDto { Nome = nome, Cpf = cpf, Idade = idade };

            //Act
            var response = await _clienteController.Add(cliente);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(400, statusCode);
        }

        //ClienteUpdate---------------------------------------------------------------------------------------------------------------------------
        [Theory]
        [InlineData(1, "Yohans", 20)]
        public async Task ClienteUpdate_SholdReturnNoContent_WhenIdAndNomeAndIdadeAreValid(int id, string nome, int idade)
        {
            //Arrange
            UpdateClienteDto clienteMockResult = new UpdateClienteDto { Nome = nome, Idade = idade };
            _clienteServiceMock.Setup(clienteService => clienteService.Update(id, clienteMockResult)).Returns(Task.FromResult(true));
            

            //Act
            var response = await _clienteController.Update(id, clienteMockResult);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(204, statusCode);
        }

        [Theory]
        [InlineData(-1, "Yohans", 20)]
        [InlineData(1, "", 20)]
        [InlineData(1, "Yohans", 4)]
        [InlineData(-1, "", 4)]
        public async Task ClienteUpdate_SholdReturnBadRequest_WhenIdOrNomeOrIdadeAreInvalid(int id, string nome, int idade)
        {
            //Arrange
            UpdateClienteDto clienteDto = new UpdateClienteDto { Nome = nome, Idade = idade };

            //Act
            var response = await _clienteController.Update(id, clienteDto);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(400, statusCode);
        }

        //ClienteDelete---------------------------------------------------------------------------------------------------------------------------
        [Theory]
        [InlineData(10)]
        public async Task ClienteDelete_SholdReturnOk_WhenIdIsValid(int id)
        {
            //Arrange
            _clienteServiceMock.Setup(clienteService => clienteService.Delete(id)).Returns(Task.FromResult(true));

            //Act
            var response = await _clienteController.Delete(id);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(200, statusCode);
        }

        [Theory]
        [InlineData(-1)]
        public async Task ClienteDelete_SholdReturnBadRequest_WhenIdIsInvalid(int id)
        {
            //Arrange

            //Act
            var response = await _clienteController.Delete(id);
            var statusCode = (response as ObjectResult).StatusCode;

            //Assert
            Assert.Equal(400, statusCode);
        }

    }
}
