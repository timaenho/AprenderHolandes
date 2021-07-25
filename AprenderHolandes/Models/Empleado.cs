using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class Empleado : Persona
    {
        [Required(ErrorMessage = Validaciones.Required)]
        public string Legajo { get; set; }
    }
}
