using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class Carrera
    {
		[Key]
		public Guid CarreraId { get; set; }

		[Required(ErrorMessage = Validaciones.Required)]
		[MaxLength(50, ErrorMessage = Validaciones.MaxLength)]
		public string Nombre { get; set; }

		public ICollection<Materia> Materias { get; set; }

		public ICollection<Alumno> Alumnos { get; set; }
	}
}
