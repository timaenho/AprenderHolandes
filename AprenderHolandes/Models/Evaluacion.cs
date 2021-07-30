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
        public Guid id { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public string Titulo { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public Materia Materia { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        Profesor profesor { get; set; }

        public ICollection<MateriaCursadaEvaluacion> {get; set;}
     


        
       





    }
}
