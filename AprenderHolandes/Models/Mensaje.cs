using AprenderHolandes.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class Mensaje
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey(nameof(Persona))]
        public Guid PersonaId { get; set; }
        public Persona Persona { get; set; }

        [Required]
        [ForeignKey(nameof(MateriaCursada))]
        public Guid MateriaCursadaId { get; set; }
        public MateriaCursada MateriaCursada { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public string Contenido { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

    }
}
