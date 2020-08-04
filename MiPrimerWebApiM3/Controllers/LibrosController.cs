using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiPrimerWebApiM3.Contexts;
using MiPrimerWebApiM3.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MiPrimerWebApiM3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        #region Constructor
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region GET
        [HttpGet]
        public ActionResult<IEnumerable<Libro>> Get()
        {
            return context.Libros.Include(x => x.Autor).ToList();
        }

        [HttpGet("{id}", Name = "ObtenerLibro")]
        public ActionResult<Libro> Get(int id)
        {
            var entity = context.Libros
                .Include(x => x.Autor)
                .FirstOrDefault(x => x.Id == id);

            if (entity == null) return NotFound();

            return entity;
        }

        #endregion

        #region POST
        [HttpPost]
        public ActionResult Post([FromBody] Libro entity)
        {
            context.Libros.Add(entity);
            context.SaveChanges();
            return new CreatedAtRouteResult("ObtenerLibro", new { id = entity.Id }, entity);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Libro value)
        {
            /*Se valida el Id para asegurarnos de que no se quiera cambiar el valor del id de un recurso*/
            if (id != value.Id)
            {
                return BadRequest();
            }

            context.Entry(value).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();//We dont need to return the Entity because client already have it
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public ActionResult<Libro> Delete(int id)
        {
            var entity = context.Libros.FirstOrDefault(x => x.Id == id);
            //if there is not valid entity, return NotFound as an answer
            if (entity == null)
                return NotFound();

            context.Libros.Remove(entity);
            context.SaveChanges();
            return entity;
        }
        #endregion
    }
}
