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

        [Required(ErrorMessage = Validaciones.Required)]
        [RegularExpression(@"[0-9]{8}", ErrorMessage = "Ingresá tu DNI sin puntos. Ej.: 12345678")]
        public string Dni { get; set; }

        //[Required(ErrorMessage = Validaciones.Required)]
        //[EmailAddress(ErrorMessage = "Ingresá una dirección de correo electrónico válida")]
        //public string Email { get; set; }

        [RegularExpression(@"[0-9]{10}", ErrorMessage = "Ingresá tu número telefónico sin 0 ni 15. Ej.: Cód. Área: 11 y Número: 2345678")]
        public string Telefono { get; set; }

        [MaxLength(100, ErrorMessage = Validaciones.MaxLength)]
        public string Direccion { get; set; }

        //public string Legajo { get; set; }

        public ICollection<Mensaje> Mensajes { get; set; }



    }
}
