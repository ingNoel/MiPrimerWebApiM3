using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiPrimerWebApiM3.Contexts;
using MiPrimerWebApiM3.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MiPrimerWebApiM3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Autor>> Get() 
        {
            return context.Autores
                .Include(x => x.Libros)
                .ToList();
        } 
        
        [HttpGet("{id}", Name = "ObtenerAutor")]
        public ActionResult<Autor> Get(int id) 
        {
            var autor = context.Autores
                .Include(x => x.Libros)
                .FirstOrDefault(x => x.Id == id);

            if (autor == null) return NotFound();

            return autor;
        }
        /*
         6)_ Crear un método Post. Recibimos un autor de nuestro cliente para insertarlo en la base de datos, que mandará en el cuerpo de la petición HTTP
        6.1)_Una de las convenciones del HTTPPost es que si un recurso es creado debemos retornar una cabecera location en donde se coloque la información de ese recurso
        */
        [HttpPost]
        public ActionResult Post([FromBody] Autor autor)
        {
            context.Autores.Add(autor);
            context.SaveChanges();
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id }, autor);
        }
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Autor value)
        {
            /*Se valida el Id para asegurarnos de que no se quiera cambiar el valor del id de un recurso*/
            if (id != value.Id) 
                return BadRequest();

            context.Entry(value).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();//We dont need to return the Entity because client already have it
        }

        [HttpDelete("{id}")]
        public ActionResult<Autor> Delete(int id)
        {
            var autor = context.Autores.FirstOrDefault(x => x.Id == id);
            //if there is not valid entity, return NotFound as an answer
            if(autor == null)
                return NotFound();

            context.Autores.Remove(autor);
            context.SaveChanges();
            return autor;
        }

    }
}
