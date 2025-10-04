using API.Hospedagem.Data;
using API.Hospedagem.Models;
using API.Hospedagem.Services.Implementations;
using API.Hospedagem.Services.Interfaces;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace API.Hospedagem.Tests.TestesServices
{
    public class FuncionarioServiceTest
    {


        // Cria um novo contexto para simular o banco de dados em memória e adiciona alguns cargos para testes
        private static ApplicationDbContext NewCtx()
        {


            // declaracao clara de opcoes para o contexto 
            var opts = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;



            var ctx = new ApplicationDbContext(opts);


            if (!ctx.Cargos.Any())
            {

                ctx.Cargos.AddRange(
                       new Models.Cargo { Id = 1, Nome = "Gerente" },
                          new Models.Cargo { Id = 2, Nome = "Recepção" }



                    );

                ctx.SaveChanges();

            }
            return ctx;
        }


        // Cria um novo mapeador  para simular o AutoMapper da Service
        private static IMapper NewMapper()
        {



            var cgf = new MapperConfiguration(cfg =>
                             cfg.AddProfile<API.Hospedagem.Profiles.MappingProfile>());

            return cgf.CreateMapper();

        }

        [Fact]
        public async Task Getall_retorna_Lista_completa_corretamente()
        {

            // Arrange 
            var ctx = NewCtx();
            var mapper = NewMapper();


            var service = new FuncionarioService(ctx, mapper);

            // Act


            ctx.Funcionarios.Add(new Models.Funcionario
            {
                Nome = "Jon",
                CPF = "1111",
                Email = "Jon@gmail.com",
                Telefone = "1",
                Endereco = "AAA",
                CargoId = 1
            });

            await ctx.SaveChangesAsync();




            var lista = await service.GetAllAsync();

            // Assert


            lista.Should().HaveCount(1);
            lista.First().CargoNome.Should().Be("Gerente");





        }


        [Fact]
        public async Task GetAll_com_lista_vazia_retorna_lista_vazia()
        {
            // Arrange 
            var ctx = NewCtx();
            var mapper = NewMapper();



            var service = new FuncionarioService(ctx, mapper);

            // Act
            var lista = await service.GetAllAsync();




            // Assert
            //lista.Should().HaveCount(0); 
            lista.Should().BeEmpty();



        }



        [Fact]
        public async Task GetById_retorna_Funcionario_corretamente()
        {

            var ctx = NewCtx();
            var mapper = NewMapper();

            var service = new FuncionarioService(ctx, mapper);



            var funcionario = new Models.Funcionario
            {
                Nome = "Jill Valentine",
                CPF = "12345678900",
                Email = "jill@hotel.com",
                Telefone = "91234-5678",
                Endereco = "Rua A, 1443",
                CargoId = 1 // "Gerente"
            };


            ctx.Funcionarios.Add(funcionario);


            await ctx.SaveChangesAsync();

            var idBuscado = funcionario.Id; // Pega o ID gerado após salvar no contexto

            // Act
            var res = await service.GetByIdAsync(idBuscado);



            // Assert

            res.Should().NotBeNull(); // primeiramente validar que o resultado nao e nulo - se for o teste falha  
            res!.Nome.Should().Be("Jill Valentine");
            res.CargoNome.Should().Be("Gerente");


            /* 
             * res! -> o operador ! (null-forgiving operator) é usado para informar ao compilador que a variável res não é nula naquele ponto específico do código.
             * Nesse caso nao precisa., pois ja foi validado que res nao e nulo na linha anterior com o Should().NotBeNull() 
             * 
             * Nesse caso ha uma validacao para se o nome do funcionario retornado é "Jill Valentine" e o nome do cargo é "Gerente" e garantir que o mapeamento e a recuperacao do banco de dados estao corretos.
             * O mesmo vale para o CargoNome, pois o mapeamento do CargoId para o nome do cargo é feito via AutoMapper e valida se estya funcionando corretamente no relacionamento entre Funcionario e Cargo.
             * 
             * 
             * Validar ambos é importante porque:
•	Você testa se o funcionário certo foi retornado.
•	Você testa se o relacionamento entre funcionário e cargo está correto e se o DTO está sendo populado corretamente.
             * 
             */

        }





        [Fact]
        public async Task GetById_com_id_inexistente_retorna_null()
        {


            var ctx = NewCtx();
            var mapper = NewMapper();

            var service = new FuncionarioService(ctx, mapper);

            // Act
            var res = await service.GetByIdAsync(999); // ID que nao existe

            // Assert
            res.Should().BeNull();



        }




        // TODO - criar testes para Create, Update e Delete 
        [Fact]

        public async Task Create_adiciona_Funcionario_corretamente()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);

            // Implementação do teste para o método CreateAsync

        }



        [Fact]
        public async Task Create_com_dados_invalidos_retorna_null()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);
            // Implementação do teste para o método CreateAsync com dados inválidos
        }



        [Fact]
        public async Task Create_com_cargo_inexistente_retorna_null()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);
            // Implementação do teste para o método CreateAsync com cargo inexistente
        }


        [Fact]
        public async Task Create_com_cargo_nulo_retorna_null()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);
            // Implementação do teste para o método CreateAsync com cargo nulo
        }

        [Fact]
        public async Task Create_com_cargo_vazio_retorna_null()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);
            // Implementação do teste para o método CreateAsync com cargo vazio
        }

        [Fact]
        public async Task Create_com_cargo_espacos_retorna_null()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);
            // Implementação do teste para o método CreateAsync com cargo contendo apenas espaços
        }


        [Fact]
        public async Task Create_com_cargo_numerico_retorna_null()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);
            // Implementação do teste para o método CreateAsync com cargo numérico
        }


        [Fact]
        public async Task Update_Caso_de_Sucesso()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);
            // Implementação do teste para o método UpdateAsync com cargo contendo caracteres especiais
        }

        [Fact]
        public async Task Update_com_id_inexistente_retorna_null()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);
            // Implementação do teste para o método UpdateAsync com ID inexistente
        }

        [Fact]
        public async Task Update_com_dados_invalidos_retorna_null()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);
            // Implementação do teste para o método UpdateAsync com dados inválidos
        }

        [Fact]
        public async Task Delete_Caso_de_Sucesso()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);
            // Implementação do teste para o método DeleteAsync
        }

        [Fact]
        public async Task Delete_com_id_inexistente_retorna_false()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);
            // Implementação do teste para o método DeleteAsync com ID inexistente
        }
    }
}