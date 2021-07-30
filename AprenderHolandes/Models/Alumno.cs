using AprenderHolandes.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class Alumno : Persona
    {
		public bool Activo { get; set; } // supongo que no resistriciones seran necesarios

		[Required(ErrorMessage = Validaciones.Required)]
		public int NumeroMatricula { get; set; }
		public List<AlumnoMateriaCursada> AlumnosMateriasCursadas { get; set; }

		[ForeignKey(nameof(Carrera))]
		public Guid CarreraId { get; set; }
		public Carrera Carrera { get; set; }

		public ICollection<AlumnoMateriaCursadaEvaluaciondaNota> AlumnoMateriaCursadaEvaluaciondaNotas { get; set; }
	}
}
