using AprenderHolandes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.ViewModels
{
    public class AlumnoNota
    {
        public Guid Id { get; set; }

        public string Nota { get; set; }

        public AlumnoMateriaCursada AlumnoMateriaCursada { get; set; }

        public MateriaCursadaEvaluacion MateriaCursadaEvaluacion { get; set; }
    }
}
