using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class AlumnoMateriaCursadaNota
    {
        
        public Alumno alumno { get; set; }

        public AlumnoMateriaCursada AlumnoMateriaCursada { get; set; }

        public MateriaCursada materiaCursada { get; set; }

        public string Nota { get; set; }

        public Profesor profesor { get; set; }

        public Evaluacion evaluacion { get; set; }
    }
}
