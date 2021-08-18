using AprenderHolandes.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class Evaluacion
    {
        [Required(ErrorMessage = Validaciones.Required)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        [MaxLength(50, ErrorMessage = Validaciones.MaxLength)]
        public string Titulo { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public Guid MateriaId { get; set; }
        public Materia Materia { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public Guid ProfesorId { get; set; }
        Profesor Profesor { get; set; }

        public ICollection<MateriaCursadaEvaluacion> MateriaCursadaEvaluaciones{get; set;}
     


        
       





    }
}
