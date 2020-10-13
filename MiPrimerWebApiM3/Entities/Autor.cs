using Microsoft.EntityFrameworkCore;
using MiPrimerWebApiM3.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerWebApiM3.Entities
{
    public class Autor : IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo Nombre es requerido")]
        [PrimeraLetraMayuscula]
        [StringLength(10,ErrorMessage ="El campo debe tener {1} caracteres o menos")]
        public string Nombre { get; set; }
        //[Range(18,120)]
        //public int Edad { get; set; }
        //[CreditCard]
        //public string CreditCard { get; set; }
        //[Url]
        //public string URL { get; set; }
        public List<Libro> Libros { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            #region Ejemplo de implementacion de IValidatableObject. Validación por Modelo
            if (!string.IsNullOrWhiteSpace(Nombre))
            {
                var length = Nombre.Length;
                if (length <= 2)
                    yield return new ValidationResult("Ingrese el nombre completo", new string[] { nameof(Nombre)});
            } 
            #endregion
        }
    }
}
