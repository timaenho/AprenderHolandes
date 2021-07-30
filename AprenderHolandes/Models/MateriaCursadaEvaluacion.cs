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
        public Guid EvaluacionId { get; set; }
        public Evaluacion Evaluacion { get; set; }

        [Required(ErrorMessage = Validaciones.Required)] 
        public Guid MateriaCursadaId { get; set; }
        public MateriaCursada MateriaCursada { get; set; }

        public bool Activo { get; set; }

        public AlumnoMateriaCursadaEvaluaciondaNota AlumnoMateriaCursadaEvaluaciondaNota { get; set; }


    }
}
