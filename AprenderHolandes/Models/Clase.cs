using AprenderHolandes.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class Clase
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public Guid MateriaCursadaId { get; set; }
        public MateriaCursada MateriaCursada { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public string Descripcion {get;set;}

        [Required(ErrorMessage = Validaciones.Required)]
        public string Link { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public Guid ProfesorId { get; set; }
        public Profesor Profesor { get; set; }







    }
}
