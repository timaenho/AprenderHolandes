using AprenderHolandes.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class AlumnoMateriaCursadaNota
    {
        [Required(ErrorMessage = Validaciones.Required)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public Alumno Alumno { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public AlumnoMateriaCursada AlumnoMateriaCursada { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public MateriaCursadaEvaluacion MateriaCursadaEvaluacion  {get;set;}

        [Required(ErrorMessage = Validaciones.Required)]
        public MateriaCursada MateriaCursada { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public string Nota { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public Profesor Profesor { get; set; }

    }
}
