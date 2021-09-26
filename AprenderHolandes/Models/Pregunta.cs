using AprenderHolandes.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class Pregunta : Mensaje
    {
        
        [Required(ErrorMessage = Validaciones.Required)]
        public string Titulo { get; set; }

        public ICollection<Respuesta> Respuestas { get; set; }

    }
}

