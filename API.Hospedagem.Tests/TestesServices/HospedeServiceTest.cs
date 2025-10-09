using API.Hospedagem.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Hospedagem.Tests.TestesServices
{
    public class HospedeServiceTest
    {


        private static ApplicationDbContext NewCtx()
        {


            // declaracao clara de opcoes para o contexto 
            var opts = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;



            var ctx = new ApplicationDbContext(opts);


            //if (!ctx.Cargos.Any())
            //{

            //    ctx.Cargos.AddRange(
            //           new Models.Cargo { Id = 1, Nome = "Gerente" },
            //              new Models.Cargo { Id = 2, Nome = "Recepção" }



            //        );

            //    ctx.SaveChanges();

            //}
            return ctx;
        }


        // Cria um novo mapeador  para simular o AutoMapper da Service
        private static IMapper NewMapper()
        {



            var cgf = new MapperConfiguration(cfg =>
                             cfg.AddProfile<API.Hospedagem.Profiles.MappingProfile>());

            return cgf.CreateMapper();

        }


    }
}
