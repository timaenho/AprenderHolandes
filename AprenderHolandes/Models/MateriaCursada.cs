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

		public bool Activo { get; set; }

		[ForeignKey(nameof(Materia))]
		public Guid MateriaId { get; set; }
		public Materia Materia { get; set; }

		[ForeignKey(nameof(Profesor))]
		public Guid ProfesorId { get; set; }
		public Profesor Profesor { get; set; }

		[Required(ErrorMessage = Validaciones.Required)]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime FechaInicio { get; set; }

		[Required(ErrorMessage = Validaciones.Required)]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime FechaTermino { get; set; }

		[Required(ErrorMessage = Validaciones.Required)]
		public string Dia { get; set; }

		[Required(ErrorMessage = Validaciones.Required)]
		public string Hora { get; set; }

		[Required(ErrorMessage = Validaciones.Required)]
		public int CantidadHorasPorSemana { get; set; }

		[Required(ErrorMessage = Validaciones.Required)]
		public string Descripcion { get; set; }

		[Required(ErrorMessage = Validaciones.Required)]
		public int Precio { get; set; }

		public List<AlumnoMateriaCursada> AlumnoMateriaCursadas { get; set; }

		public ICollection<Calificacion> Calificaciones { get; set; }

		public ICollection<MateriaCursadaEvaluacion> MateriaCursadaEvaluaciones {get; set;}

		public ICollection<Clase> Clases { get; set; }

		public ICollection<Mensaje> Mensajes{ get; set; }
	}
}
