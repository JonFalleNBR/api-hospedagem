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


    }
}
