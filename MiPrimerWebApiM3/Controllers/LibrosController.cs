using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiPrimerWebApiM3.Contexts;
using MiPrimerWebApiM3.Entities;
using MiPrimerWebApiM3.Models;
using System;
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
        private readonly IMapper mapper;
        private readonly DbSet<Libro> contextTable;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.contextTable = context.Libros;
        }
        #endregion

        #region GET
        [HttpGet]
        [ResponseCache(Duration = 60)]
        //[Authorize]
        //public ActionResult<string> Get()
        public ActionResult<IEnumerable<LibroDTO>> Get()
        {
            var entity = contextTable.Include(x => x.Autor).ToList();
            return mapper.Map<List<LibroDTO>>(entity);
        }

        [HttpGet("{id}", Name = "ObtenerLibro")]
        public ActionResult<LibroDTO> Get(int id)
        {
            var entity = contextTable
                .Include(x => x.Autor)
                .FirstOrDefault(x => x.Id == id);

            if (entity == null) return NotFound();

            return mapper.Map<LibroDTO>(entity);
        }

        #endregion

        #region POST
        [HttpPost]
        public ActionResult Post([FromBody] Libro entity)
        {
            contextTable.Add(entity);
            context.SaveChanges();
            return new CreatedAtRouteResult("ObtenerLibro", new { id = entity.Id }, entity);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Libro value)
        {
            /*Se valida el Id para asegurarnos de que no se quiera cambiar el valor del id de un recurso*/
            if (id != value.Id) return BadRequest();

            context.Entry(value).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();//We dont need to return the Entity because client already have it
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public ActionResult<LibroDTO> Delete(int id)
        {
            var entity = contextTable.FirstOrDefault(x => x.Id == id);
            //if there is not valid entity, return NotFound as an answer
            if (entity == null) return NotFound();

            contextTable.Remove(entity);
            context.SaveChanges();
            return mapper.Map<LibroDTO>(entity);
        }
        #endregion
    }
}
