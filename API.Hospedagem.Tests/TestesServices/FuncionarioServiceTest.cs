using API.Hospedagem.Data;
using API.Hospedagem.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Hospedagem.Services.Implementations;
using FluentAssertions;

namespace API.Hospedagem.Tests.TestesServices
{
    public class FuncionarioServiceTest 
    {


        // Cria um novo contexto para simular o banco de dados em memória e adiciona alguns cargos para testes
        private static ApplicationDbContext NewCtx() {


            // declaracao clara de opcoes para o contexto 
            var opts = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;



            var ctx = new ApplicationDbContext(opts);


            if (!ctx.Cargos.Any()) {

                ctx.Cargos.AddRange(
                       new Models.Cargo { Id = 1, Nome = "Gerente" },
                          new Models.Cargo { Id = 2, Nome = "Recepção" }



                    ); 

                ctx.SaveChanges();

            }
            return ctx;
        }


        // Cria um novo mapeador  para simular o AutoMapper da Service
        private static IMapper NewMapper() {



            var cgf = new MapperConfiguration(cfg =>
                             cfg.AddProfile<API.Hospedagem.Profiles.MappingProfile>()); 

            return cgf.CreateMapper();

        }

        [Fact]
        public async Task Getall_retorna_Lista_completa_corretamente() {

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



    }
}
