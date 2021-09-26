using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class Respuesta : Mensaje
    {
        [ForeignKey(nameof(Pregunta))]
        public Guid PreguntaId { get; set; }
        public Pregunta Pregunta{ get; set; }
    }
}
