using API.Hospedagem.Controllers;
using API.Hospedagem.DTOs;
using API.Hospedagem.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Hospedagem.Tests.TestesControllers
{
    public class FuncionarioControllerTest
    {


        private readonly Mock<IFuncionarioService> _serviceMock;
        private readonly FuncionarioController _controller;


        public FuncionarioControllerTest()
        {
            _serviceMock = new Mock<IFuncionarioService>();
            _controller = new FuncionarioController(_serviceMock.Object);
        }


        [Fact]
        public async Task GetById_Quando_nao_existe_deve_retornar_NotFound() {


            // ARRANGE
            _serviceMock.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((FuncionarioReadDto?)null);


            // ACT
            var result = await _controller.GetById(99);

            // ASSERT 
            result.Result.Should().BeOfType<NotFoundResult>(); // Should é do FluentAssertions que deixa o teste mais legível



            _serviceMock.Verify(s => s.GetByIdAsync(99), Times.Once());  // Verifica se o método foi chamado exatamente uma vez E CONFORME o parametro do setup mockado

        }


        [Fact]
        public async Task GetById_Quando_existe_retorna_Ok() {


            // ARRANGE -> Mockando explicitamente um funcionário 
            var funcionario = new FuncionarioReadDto
            {

           

                Id = 6,
                Nome = "Jill Valentine",
                CPF = "12345678900",
                Email = "jill@hotel.com",
                Telefone = "91234-5678",
                Endereco = "Rua A, 1443",
                CargoId = 2,
                CargoNome = "Serviços Gerais"


            };


            // ARRANGE
            _serviceMock.Setup(s => s.GetByIdAsync(6)).ReturnsAsync(funcionario);



            // ACT

            var result = await _controller.GetById(6);

            // Passando o ID que foi mockado


            // ASSERT

            result.Result.Should().BeOfType<OkObjectResult>(); // Verifica se o resultado é um OkObjectResult
            var ok = result.Result as OkObjectResult; // Converte o resultado para OkObjectResult
            ok!.Value.Should().BeEquivalentTo(funcionario); // Verifica se o valor retornado é equivalente ao funcionário mockado



            _serviceMock.Verify(s => s.GetByIdAsync(6), Times.Once);
        }
    }
}
