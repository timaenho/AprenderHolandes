using AprenderHolandes.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class AlumnoMateriaCursadaEvaluaciondaNota
    {
        [Required(ErrorMessage = Validaciones.Required)]
        public Guid Id { get; set; }

    
        [Required(ErrorMessage = Validaciones.Required)]
        public Guid AlumnoId { get; set; }
        public Alumno Alumno { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public Guid MateriaCursadaEvaluacionId { get; set; }
        public MateriaCursadaEvaluacion MateriaCursadaEvaluacion  {get;set;}

        [Required(ErrorMessage = Validaciones.Required)]
        public string Nota { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public Guid ProfesorId { get; set; }
        public Profesor Profesor { get; set; }

    }
}
