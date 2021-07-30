using AprenderHolandes.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class MateriaCursadaEvaluacion
    {

        public Guid Id { get; set; }

        [Required(ErrorMessage=Validaciones.Required)]
        [Key, ForeignKey("Evaluacion")]
        public Guid EvaluacionId { get; set; }
        public Evaluacion Evaluacion { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        [Key, ForeignKey("MateriaCursada")]
        public Guid MateriaCursadaId { get; set; }
        public MateriaCursada MateriaCursada { get; set; }

        public bool Activo { get; set; }

        public ICollection< AlumnoMateriaCursadaEvaluaciondaNota> AlumnoMateriaCursadaEvaluaciondaNotas { get; set; }


    }
}
