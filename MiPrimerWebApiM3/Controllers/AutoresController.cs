using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiPrimerWebApiM3.Contexts;
using MiPrimerWebApiM3.Entities;
using MiPrimerWebApiM3.Helpers;
using MiPrimerWebApiM3.Models;
using MiPrimerWebApiM3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerWebApiM3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Microsoft.AspNetCore.Authorization.Authorize]
    public class AutoresController : ControllerBase
    {

        #region Properties
        private readonly ApplicationDbContext context;
        private readonly IClaseB claseB;
        private readonly ILogger<AutoresController> logger;
        private readonly DbSet<Autor> contextTable;
        private readonly IMapper mapper;
        #endregion

        #region Constructor
        public AutoresController(ApplicationDbContext context, ClaseB claseB, ILogger<AutoresController> logger, IMapper mapper)
        {
            this.context = context;
            this.claseB = claseB;
            this.logger = logger;
            this.mapper = mapper;
            contextTable = context.Autores;
        }
        #endregion

        #region GET
        [HttpGet] // get /api/autores
        [HttpGet("listado")] //get /api/autores/listado
        [HttpGet("/listado")] //get /listado
        //Es necesario incluir ServiceFilter por la inyección de dependencias
        [ServiceFilter(typeof(MiFiltroDeAccion))]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> Get()
        {
            //logger.LogInformation("Obteniendo los autores");
            //claseB.HacerAlgo();
            var entity = await contextTable
                .Include(x => x.Libros)
                .ToListAsync();

            var _return = mapper.Map<List<AutorDTO>>(entity);
            return _return;
        }
        [HttpGet("Primer")]
        public async Task<ActionResult<AutorDTO>> GetPrimerAutor()
        {
            var entity = await contextTable.FirstOrDefaultAsync();
            return mapper.Map<AutorDTO>(entity);
        }
        //Si se desea un valor por defecto, colocar el signo = 
        //Si queremos hace un parámetro opcional, se coloca signo de interrogación “?”:
        // get /api/autores/1/armando/rodriguez
        //[HttpGet("{id}/{param2=Alejandro}/{param3?}", Name = "ObtenerAutor")]
        [HttpGet("{id}", Name = "ObtenerAutor")]
        public async Task<ActionResult<AutorDTO>> Get(int id)
        {
            var entity = await contextTable
                .Include(x => x.Libros)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                logger.LogWarning($"El autor de Id {id} no ha sido encontrado");
                return NotFound();
            }

            var _return = mapper.Map<AutorDTO>(entity);
            return _return;
        }
        #endregion

        #region POST
        /*
        6)_ Crear un método Post. Recibimos un autor de nuestro cliente para insertarlo en la base de datos, que mandará en el cuerpo de la petición HTTP
       6.1)_Una de las convenciones del HTTPPost es que si un recurso es creado debemos retornar una cabecera location en donde se coloque la información de ese recurso
       */
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorPostDTO autorPostDTO)
        {
            var autor = mapper.Map<Autor>(autorPostDTO);
            context.Add(autor);
            await context.SaveChangesAsync();
            var autorDTO = mapper.Map<AutorDTO>(autor);
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autorDTO.Id }, autorDTO);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Autor entity)
        {
            /*Se valida el Id para asegurarnos de que no se quiera cambiar el valor del id de un recurso*/
            if (id != entity.Id) return BadRequest();

            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();//We dont need to return the Entity because client already have it
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<AutorDTO>> Delete(int id)
        {
            var entity = contextTable.FirstOrDefault(x => x.Id == id);
            //if there is not valid entity, return NotFound as an answer
            if (entity == null) return NotFound();

            contextTable.Remove(entity);
            await context.SaveChangesAsync();
            return mapper.Map<AutorDTO>(entity);
        } 
        #endregion

    }
}
