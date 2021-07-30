using AprenderHolandes.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
	public class MateriaCursada
	{
		public Guid MateriaCursadaId { get; set; }

		[Required(ErrorMessage = Validaciones.Required)]
		[MaxLength(100, ErrorMessage = Validaciones.MaxLength)]
		public string Nombre { get; set; }

		[Required(ErrorMessage = Validaciones.Required)]
		[Range(1, 6, ErrorMessage = Validaciones.NumeroMayorMenorA)]
		public int Anio { get; set; }

		[Required(ErrorMessage = Validaciones.Required)]
		[Range(1, 2, ErrorMessage = Validaciones.NumeroMayorMenorA)]
		public int Cuatrimestre { get; set; }

		public bool Activo { get; set; }

		[ForeignKey(nameof(Materia))]
		public Guid MateriaId { get; set; }
		public Materia Materia { get; set; }

		[ForeignKey(nameof(Profesor))]
		public Guid ProfesorId { get; set; }
		public Profesor Profesor { get; set; }

		public List<AlumnoMateriaCursada> AlumnoMateriaCursadas { get; set; }

		public ICollection<Calificacion> Calificaciones { get; set; }

		public ICollection<MateriaCursadaEvaluacion> MateriaCursadaEvaluaciones {get; set;}
	}
}
