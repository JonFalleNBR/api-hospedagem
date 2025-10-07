using API.Hospedagem.Data;
using API.Hospedagem.DTOs;
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


        //[Setup] -> NÃO EXISTE NO XUNIT - USAR CONSTRUTOR
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

            // Arrange
            var dto = new API.Hospedagem.DTOs.FuncionarioCreateDto
            {
                Nome = "Marcus",
                CPF = "44444444",
                Email = "MARKUS@gmail.com",
                Telefone = "122222",
                Endereco = "aaa",
                CargoId = 1,
                CargoNome = "Gerente"




            };

            await ctx.SaveChangesAsync();


            // Act
            var res = await service.CreateAsync(dto);

            // Assert


            res.Should().NotBeNull();
            res!.Nome.Should().Be("Marcus");
            res.CargoNome.Should().Be("Gerente");




            ctx.Funcionarios.Should().HaveCount(1);
            // or  ctx.Funcionarios.Count().Should().Be(1); 



        }



        [Fact]
        public async Task Create_com_dados_invalidos_retorna_null()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);


            // Arrange: DTO com nome vazio e CPF nulo (dados inválidos)
            var dto = new API.Hospedagem.DTOs.FuncionarioCreateDto
            {
                Nome = null, // Nome inválido
                CPF = null, // CPF inválido
                Email = "email@teste.com",
                Telefone = "12345",
                Endereco = "Rua Teste",
                CargoId = 1 // Cargo válido, mas outros campos inválidos
            };

            //await ctx.SaveChangesAsync(); --> Nao precisa salvar o contexto aqui, pois o teste é sobre a validação dos dados de entrada antes de qualquer operação de banco de dados. - se usa saveChanges() apenas quando quer persistir algo no banco de dados. -> new Funcionario ok - new FuncionarioCreateDto nao



            // Act

            var res = await service.CreateAsync(dto);


            // Assert
            res.Should().BeNull();
          
        }


        [Fact]
        public async Task Create_com_cargo_diferente_de_gest_rep_retorna_2()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);


            // Arrange: DTO com CargoId que não existe no banco de dados
            var dto = new API.Hospedagem.DTOs.FuncionarioCreateDto
            {
                Nome = "Marcus",
                CPF = "44444444",
                Email = "MARKUS@gmail.com",
                Telefone = "122222",
                Endereco = "aaa",
                CargoId = 0,
                CargoNome = "lixeiro"

            };


            // act 
            var res = await service.CreateAsync(dto);
            
            
            // Assert

            res.Should().NotBeNull();
            res!.CargoId.Should().Be(2);

        }



        [Fact]
        public async Task Update_Caso_de_Sucesso()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);



            // Arrange: Adiciona um funcionário existente ao contexto
            var funcionrioExistete = new Models.Funcionario
            {

                Nome = "Jill Valentine",
                CPF = "12345678900",
                Email = "jill@hotel.com",
                Telefone = "91234-5678",
                Endereco = "Rua A, 1443",
                CargoId = 1 // "Gerente"



            }; 

            ctx.Funcionarios.Add(funcionrioExistete);

            await ctx.SaveChangesAsync();

            var id = funcionrioExistete.Id;

            //ACT 
            var dtoUpate = new API.Hospedagem.DTOs.FuncionarioCreateDto
            {

                Nome = "Jill V.",
                CPF = "99999999999",
                Email = "jillv@hotel.com",
                Telefone = "99999-9999",
                Endereco = "Rua B, 2000",
                CargoId = 1,
                CargoNome = "Recepção"

            };


            var res = await service.UpdateAsync(id, dtoUpate);

            //Assert
            res.Should().BeTrue(); // Verifica se o retorno é true, indicando sucesso na atualização


            // Verifica se os dados foram atualizados no banco
            var atualizado = await ctx.Funcionarios.FindAsync(id);
            atualizado.Should().NotBeNull();
            atualizado!.Nome.Should().Be("Jill V.");
            atualizado.CPF.Should().Be("99999999999");
            atualizado.Email.Should().Be("jillv@hotel.com");
            atualizado.Telefone.Should().Be("99999-9999");
            atualizado.Endereco.Should().Be("Rua B, 2000");
            atualizado.CargoId.Should().Be(1);


        }





        [Fact]
        public async Task Update_com_id_inexistente_retorna_null()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);

            var FuncionarioInexistente = new FuncionarioCreateDto
            {

                Nome = "X",
                CPF = "123",
                Email = "x@x.com",
                Telefone = "9",
                Endereco = "Y",
                CargoNome = "Recepção"


            }; 

            var ok = await service.UpdateAsync(999, FuncionarioInexistente); // 999 nao existe 

            // Assert
            ok!.Should().BeFalse(); // Verifica se o retorno é false, indicando que o funcionário não foi encontrado



        }

       

        [Fact]
        public async Task Update_com_dados_invalidos_retorna_null()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);


            // Arrange: Adiciona um funcionário existente ao contexto

            var funcionrioExistete = new Models.Funcionario
            {

                Nome = "Jill Valentine",
                CPF = "12345678900",
                Email = "jill@hotel.com",
                Telefone = "91234-5678",
                Endereco = "Rua A, 1443",
                CargoId = 1 // "Gerente"



            };


            ctx.Funcionarios.Add(funcionrioExistete); 

           await ctx.SaveChangesAsync();

            // Act -> tenta atualizar com dados inválidos (nome vazio e CPF nulo)

            var dtoUpate = new API.Hospedagem.DTOs.FuncionarioCreateDto
            {

                Nome = "J", // < 3 (inválido)
                CPF = "",   // inválido
                Email = " ",// inválido
                Telefone = " ", // inválido
                Endereco = "",  // inválido
                CargoId = 1,
                CargoNome = ""  // inválido

            };

            var ok = await service.UpdateAsync(funcionrioExistete.Id, dtoUpate);

            // Assert
            ok.Should().BeFalse(); // Verifica se o retorno é false, indicando falha na atualização devido a dados inválidos


            // E garante que NÃO alterou no "banco"
            var persisted = await ctx.Funcionarios.FindAsync(funcionrioExistete.Id);
            persisted!.Nome.Should().Be("Jill Valentine");
            persisted.CPF.Should().Be("12345678900");
            persisted.Email.Should().Be("jill@hotel.com");
            persisted.Telefone.Should().Be("91234-5678");
            persisted.Endereco.Should().Be("Rua A, 1443");
            persisted.CargoId.Should().Be(1);



        }

        [Fact]
        public async Task Delete_Caso_de_Sucesso()
        {
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);


            var funcionrioExistete = new Models.Funcionario
            {

                Nome = "Jill Valentine",
                CPF = "12345678900",
                Email = "jill@hotel.com",
                Telefone = "91234-5678",
                Endereco = "Rua A, 1443",
                CargoId = 1 // "Gerente"



            };


            ctx.Funcionarios.Add(funcionrioExistete);

            await ctx.SaveChangesAsync();

            //var id = funcionrioExistete.Id;


            // act 
            var res = await service.DeleteAsync(funcionrioExistete.Id);

            // Assert
            res.Should().BeTrue();
            ctx.Funcionarios.Count().Should().Be(0); // Garante que o banco está vazio após a deleção

        }

        [Fact]
        public async Task Delete_com_id_inexistente_retorna_false()
        {
            // Arrange
            var ctx = NewCtx();
            var mapper = NewMapper();
            var service = new FuncionarioService(ctx, mapper);


            // Act
            var ok = await service.DeleteAsync(999); // 999 nao existe


            // Assert
            ok.Should().BeFalse(); // Verifica se o retorno é false, indicando que o funcionário não foi encontrado
            ctx.Funcionarios.Count().Should().Be(0); // Garante que o banco continua vazio



        }
        // Ideia de melhoria para o futuro: usar o padrão de projeto "Factory" para criar os contextos e mapeadores, alem de delete cascade que contemple os FKs tambem
    }
}