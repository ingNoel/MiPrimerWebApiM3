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
using System.Threading.Tasks;

namespace MiPrimerWebApiM3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        #region Properties
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly DbSet<Libro> contextTable; 
        #endregion

        #region Constructor
        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.contextTable = context.Libros;
        }
        #endregion

        #region GET
        [HttpGet]
        //[ResponseCache(Duration = 60)]
        //[Authorize]
        //public ActionResult<string> Get()
        public async Task<ActionResult<IEnumerable<LibroDTO>>> Get()
        {
            List<Libro> entity = await contextTable.Include(x => x.Autor).ToListAsync();
            return mapper.Map<List<LibroDTO>>(entity);
        }

        [HttpGet("{id}", Name = "ObtenerLibro")]
        public async Task<ActionResult<LibroDTO>> Get(int id)
        {
            var entity = await contextTable
                .Include(x => x.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) return NotFound();

            return mapper.Map<LibroDTO>(entity);
        }

        #endregion

        #region POST
        [HttpPost]
        public async  Task<ActionResult> Post([FromBody] LibroPostDTO entity)
        {
            Libro libro = mapper.Map<Libro>(entity);
            context.Add(libro);
            await context.SaveChangesAsync();
            LibroDTO libroDTO = mapper.Map<LibroDTO>(libro);
            return new CreatedAtRouteResult("ObtenerLibro", new { id = libroDTO.Id }, libroDTO);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Libro value)
        {
            /*Se valida el Id para asegurarnos de que no se quiera cambiar el valor del id de un recurso*/
            if (id != value.Id) return BadRequest();

            context.Entry(value).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();//We dont need to return the Entity because client already have it
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<LibroDTO>> Delete(int id)
        {
            var entity = await contextTable.FirstOrDefaultAsync(x => x.Id == id);
            //if there is not valid entity, return NotFound as an answer
            if (entity == null) return NotFound();

            contextTable.Remove(entity);
            context.SaveChanges();
            return mapper.Map<LibroDTO>(entity);
        }
        #endregion
    }
}
