using API.Hospedagem.Controllers;
using API.Hospedagem.DTOs;
using API.Hospedagem.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
//using NUnit.Framework;
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
        public async Task GetById_Quando_nao_existe_deve_retornar_NotFound()
        {


            // ARRANGE
            _serviceMock.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((FuncionarioReadDto?)null);


            // ACT
            var result = await _controller.GetById(99);

            // ASSERT 
            result.Result.Should().BeOfType<NotFoundResult>(); // Should é do FluentAssertions que deixa o teste mais legível



            _serviceMock.Verify(s => s.GetByIdAsync(99), Times.Once());  // Verifica se o método foi chamado exatamente uma vez E CONFORME o parametro do setup mockado

        }


        [Fact]
        public async Task GetById_Quando_existe_retorna_Ok()
        {


            // ARRANGE -> Mockando explicitamente um funcionário 
            var funcionario = new FuncionarioReadDto
            {



                Id = 4,
                Nome = "Jill Valentine",
                CPF = "12345678900",
                Email = "jill@hotel.com",
                Telefone = "91234-5678",
                Endereco = "Rua A, 1443",
                CargoId = 2,
                CargoNome = "Serviços Gerais"


            };


            // ARRANGE
            _serviceMock.Setup(s => s.GetByIdAsync(4)).ReturnsAsync(funcionario);



            // ACT

            var result = await _controller.GetById(4);

            // Passando o ID que foi mockado


            // ASSERT

            result.Result.Should().BeOfType<OkObjectResult>(); // Verifica se o resultado é um OkObjectResult
            var ok = result.Result as OkObjectResult; // Converte o resultado para OkObjectResult
            ok!.Value.Should().BeEquivalentTo(funcionario); // Verifica se o valor retornado é equivalente ao funcionário mockado



            _serviceMock.Verify(s => s.GetByIdAsync(4), Times.Once);
        }







        [Fact]
        public async Task GetAll_Deve_Retornar_Ok_Com_Lista_Funcionarios()
        {

            #region ARRANGE
            var esperados = new List<FuncionarioReadDto> {



                new(){
                     Id = 6, Nome = "Jill Valentine", CPF = "12345678900",
            Email = "jill@hotel.com", Telefone = "91234-5678",
            Endereco = "Rua A, 1443", CargoId = 2, CargoNome = "Serviços Gerais"
                   },

         
                //new()
                //{
                //     Id = 5, Nome = "João Costa", CPF = "12345678911",
                //     Email = "joao@hotel.com", Telefone = "91234-6666",
                //    Endereco = "Rua C, 1455", CargoId = 1, CargoNome = "Gestão"

                //}

        };



            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(esperados);

            #endregion



            #region ACT
            var result = await _controller.GetAll();
            #endregion


            #region ASSERT -> OK + Content
            var ok = result.Result.Should().BeOfType<OkObjectResult>().Which;
            var lista = ok.Value.Should().BeAssignableTo<IEnumerable<FuncionarioReadDto>>().Which;
            #endregion

            _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);




        }

        [Fact]
        public async Task GetAll_quando_vazio_retorna_Ok_com_lista_vazia()
        {

            // ARRANGE
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<FuncionarioReadDto>()); // Simula o retorno vazio do serviço


            // ACT
            var result = await _controller.GetAll(); // So verifica se o resultado é http eh o esperado



            // // ASSERT

            result.Result.Should().BeOfType<OkObjectResult>(); // Verifica se o resultado é um OkObjectResult
            var ok = result.Result as OkObjectResult; // Converte o resultado para OkObjectResult
            ok.Should().NotBeNull(); // Verifica se ok não é nulo



            var lista = ok!.Value as IEnumerable<FuncionarioReadDto>; // Verifica se o valor retornado é uma lista de FuncionarioReadDto
            lista.Should().BeNullOrEmpty();// Verifica se a lista é nula ou vazia
            lista!.Should().BeEmpty(); // Verifica se a lista tem 0 elementos


            _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);// Verifica se o método foi chamado exatamente uma vez






        }


        [Fact]
        public async Task Create_com_Sucesso_retorna_CreatedAtRoute_com_Payload()
        {


            var input = new FuncionarioCreateDto
            {


                Nome = "Ada Lovelace",
                CPF = "12345678900",
                Email = "ada@hotel.com",
                Telefone = "99999-9999",
                Endereco = "Rua X, 123",
                CargoId = 1


            }; // Dados de entrada para criação


            var criado = new FuncionarioReadDto
            {

                Id = 10,
                Nome = "Ada Lovelace",
                CPF = "12345678900",
                Email = "ada@hotel.com",
                Telefone = "99999-9999",
                Endereco = "Rua X, 123",
                CargoId = 1,
                CargoNome = "Recepção"

            }; // Dados de entrada para criação



            _serviceMock.Setup(s => s.CreateAsync(input)).ReturnsAsync(criado); // Mockando o serviço para retornar o funcionário criado




            var result = await _controller.Create(input); // Chamando o método Create do controller



            result.Result.Should().BeOfType<CreatedAtRouteResult>(); // Verifica se o resultado é um CreatedAtRouteResult
            var createdAt = result.Result as CreatedAtRouteResult; createdAt.Should().NotBeNull(); // Converte o resultado para CreatedAtRouteResult e verifica se não é nulo
            createdAt.RouteValues!["id"].Should().Be(criado.Id); // Verifica se o id retornado é o mesmo do funcionário criado

            // Assertando o payload retornado

            var payload = createdAt.Value as FuncionarioReadDto;
            payload.Should().NotBeNull();
            payload.Should().BeEquivalentTo(criado); // Verifica se o payload retornado é equivalente ao funcionário criado

            _serviceMock.Verify(s => s.CreateAsync(input), Times.Once); // Verifica se o método foi chamado exatamente uma vez com o input esperado


        }


        [Fact]
        public async Task Create_Com_Lancamento_de_Exception()
        {
            var input = new FuncionarioCreateDto
            {


                Nome = "Ada Lovelace",
                CPF = "12345678900",
                Email = "ada@hotel.com",
                Telefone = "99999-9999",
                Endereco = "Rua X, 123",
                CargoId = 1


            };

            _serviceMock.Setup(s => s.CreateAsync(input)).ReturnsAsync((FuncionarioReadDto?)null); // Mockando o serviço para retornar null, simulando que o funcionário não existe

            var act = async () => await _controller.Create(input);
            await act.Should().ThrowAsync<NullReferenceException>();

            _serviceMock.Verify(s => s.CreateAsync(input), Times.Once);// Verifica se o método foi chamado exatamente uma vez com o input esperado


        }


        [Fact]
        public async Task update_quando_sucesso_retorna_noContent()
        {


            var dto = new FuncionarioCreateDto
            {
                Nome = "Ada Lovelace",
                CPF = "12345678900",
                Email = "ada@hotel.com",
                Telefone = "99999-9999",
                Endereco = "Rua X, 123",
                CargoId = 1

            };


            _serviceMock.Setup(s => s.UpdateAsync(10, dto)).ReturnsAsync(true); // Mockando o serviço para retornar true, simulando sucesso na atualização



            var resp = await _controller.Update(10, dto); // Chamando o método Update do controller




            resp.Should().BeOfType<NoContentResult>(); // Verifica se o resultado é um NoContentResult

            _serviceMock.Verify(s => s.UpdateAsync(10, dto), Times.Once); // Verifica se o método foi chamado exatamente uma vez com o input esperado


        }

        [Fact]
        public async Task update_quando_nao_encontrado_retorna_NotFound()
        {

            var dto = new FuncionarioCreateDto
            {
                Nome = "Ada Lovelace",
                CPF = "12345678900",
                Email = "ada@hotel.com",
                Telefone = "99999-9999",
                Endereco = "Rua X, 123",
                CargoId = 1



            };

            _serviceMock.Setup(S => S.UpdateAsync(10, dto)).ReturnsAsync(false);



            var resp = await _controller.Update(10, dto);


            resp.Should().BeOfType<NotFoundResult>();

            _serviceMock.Verify(s => s.UpdateAsync(10, dto), Times.Once); // Verifica se o método foi chamado exatamente uma vez com o input esperado
        }


        [Fact]
        public async Task delete_quando_sucesso_retorna_NoContent()
        {



            _serviceMock.Setup(s => s.DeleteAsync(10)).ReturnsAsync(true); // Mockando o serviço para retornar true, simulando sucesso na deleção


            var resp = await _controller.Delete(10); // Chamando o método Delete do controller

            resp.Should().BeOfType<NoContentResult>(); // Verifica se o resultado é um NoContentResult

            _serviceMock.Verify(s => s.DeleteAsync(10), Times.Once); // Verifica se o método foi chamado exatamente uma vez com o input esperado


        }

        [Fact]
        public async Task delete_quando_nao_encontrado_retorna_NotFound()
        {


            _serviceMock.Setup(s => s.DeleteAsync(10)).ReturnsAsync(false); // Mockando o serviço para retornar false, simulando que o funcionário não foi encontrado


            var resp = await _controller.Delete(10);

            resp.Should().BeOfType<NotFoundResult>();

            _serviceMock.Verify(s => s.DeleteAsync(10), Times.Once); // Verifica se o método foi chamado exatamente uma vez com o input esperado

        }


    }
}
