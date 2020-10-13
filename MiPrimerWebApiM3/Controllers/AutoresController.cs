using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiPrimerWebApiM3.Contexts;
using MiPrimerWebApiM3.Entities;
using MiPrimerWebApiM3.Services;
using System.Collections.Generic;
using System.Linq;

namespace MiPrimerWebApiM3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IClaseB claseB;
        private readonly ILogger<AutoresController> logger;
        private readonly DbSet<Autor> contextTable;

        public AutoresController(ApplicationDbContext context, ClaseB claseB, ILogger<AutoresController> logger)
        {
            this.context = context;
            this.claseB = claseB;
            this.logger = logger;
            contextTable = context.Autores;
        }

        [HttpGet] // get /api/autores
        [HttpGet("listado")] //get /api/autores/listado
        [HttpGet("/listado")] //get /listado
        public ActionResult<IEnumerable<Autor>> Get() 
        {
            logger.LogInformation("Obteniendo los autores");
            claseB.HacerAlgo();
            return contextTable
                .Include(x => x.Libros)
                .ToList();
        }
        [HttpGet("Primer")]
        public ActionResult<Autor> GetPrimerAutor()
        {
            return contextTable.FirstOrDefault();
        }
        //Si se desea un valor por defecto, colocar el signo = 
        //Si queremos hace un parámetro opcional, se coloca signo de interrogación “?”:
        // get /api/autores/1/armando/rodriguez
        //[HttpGet("{id}/{param2=Alejandro}/{param3?}", Name = "ObtenerAutor")]
        [HttpGet("{id}", Name = "ObtenerAutor")]
        public ActionResult<Autor> Get(int id) 
        {
            var entity = contextTable
                .Include(x => x.Libros)
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                logger.LogInformation($"El autor de Id {id} no ha sido encontrado");
                return NotFound();
            }

            return entity;
        }
        /*
         6)_ Crear un método Post. Recibimos un autor de nuestro cliente para insertarlo en la base de datos, que mandará en el cuerpo de la petición HTTP
        6.1)_Una de las convenciones del HTTPPost es que si un recurso es creado debemos retornar una cabecera location en donde se coloque la información de ese recurso
        */
        [HttpPost]
        public ActionResult Post([FromBody] Autor entity)
        {
            contextTable.Add(entity);
            context.SaveChanges();
            return new CreatedAtRouteResult("ObtenerAutor", new { id = entity.Id }, entity);
        }
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Autor entity)
        {
            /*Se valida el Id para asegurarnos de que no se quiera cambiar el valor del id de un recurso*/
            if (id != entity.Id) return BadRequest();

            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();//We dont need to return the Entity because client already have it
        }

        [HttpDelete("{id}")]
        public ActionResult<Autor> Delete(int id)
        {
            var entity = contextTable.FirstOrDefault(x => x.Id == id);
            //if there is not valid entity, return NotFound as an answer
            if(entity == null) return NotFound();

            contextTable.Remove(entity);
            context.SaveChanges();
            return entity;
        }

    }
}
