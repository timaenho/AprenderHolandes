using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class MateriaCursadaEvaluacion
    {

        public Guid Id { get; set; }
        public Evaluacion Evaluacion { get; set; }

        public MateriaCursada MateriaCursada { get; set; }

        public AlumnoMateriaCursadaNota AlumnoMateriaCursadaNotas { get; set; }


    }
}
