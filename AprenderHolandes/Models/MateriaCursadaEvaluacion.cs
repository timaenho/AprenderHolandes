using AprenderHolandes.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class MateriaCursadaEvaluacion
    {

        public Guid Id { get; set; }

        [Required(ErrorMessage=Validaciones.Required)]
        public Evaluacion Evaluacion { get; set; }

        [Required(ErrorMessage = Validaciones.Required)] 
        public MateriaCursada MateriaCursada { get; set; }

        public bool Activo { get; set; }

        public AlumnoMateriaCursadaNota AlumnoMateriaCursadaNota { get; set; }


    }
}
