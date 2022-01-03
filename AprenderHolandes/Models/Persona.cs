using AprenderHolandes.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class Persona : IdentityUser<Guid>
    {
        //[Key]
        //public Guid Id { get; set; }

        //[Required(ErrorMessage = Validaciones.Required)]
        //[MaxLength(50, ErrorMessage = Validaciones.MaxLength)]
        //public string UserName { get; set; }

        //[Required(ErrorMessage = Validaciones.Required)]
        //[DataType(DataType.Password)]
        //[Display(Name ="Contraseña")]
        //public override string PasswordHash { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaAlta { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        [MaxLength(50, ErrorMessage = Validaciones.MaxLength)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        [MaxLength(50, ErrorMessage = Validaciones.MaxLength)]
        public string Apellido { get; set; }

        public ICollection<Mensaje> Mensajes { get; set; }



    }
}
