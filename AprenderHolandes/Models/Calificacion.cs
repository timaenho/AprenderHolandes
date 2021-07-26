using AprenderHolandes.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class Calificacion
    {
		
		
			public Guid CalificacionId { get; set; }

			[Required(ErrorMessage = Validaciones.Required)]
			public int NotaFinal { get; set; }

			public AlumnoMateriaCursada AlumnoMateriaCursada { get; set; }


			[Required(ErrorMessage = Validaciones.Required)]
			public Materia Materia { get; set; }



			[Required(ErrorMessage = Validaciones.Required)]
			public MateriaCursada MateriaCursada
			{
				get
				{
					return AlumnoMateriaCursada.MateriaCursada;
				}
			}

			[ForeignKey(nameof(Profesor))]
			public Guid ProfesorId { get; set; }
			public Profesor Profesor { get; set; }

			[Required(ErrorMessage = Validaciones.Required)]
			public Alumno Alumno
			{
				get
				{
					return AlumnoMateriaCursada.Alumno;
				}
			}

		}
	}
