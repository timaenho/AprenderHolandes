using AprenderHolandes.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.ViewModels
{
    public class RegistroUsuario
    {
        public int Id { get; set; }

        [Required (ErrorMessage = Validaciones.Required)]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = Validaciones.Required)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        [RegularExpression(@"[0-9]{8}", ErrorMessage = "Ingresá tu DNI sin puntos. Ej.: 12345678")]
        public string Dni { get; set; }
        
        [Required(ErrorMessage = Validaciones.Required)]
        [EmailAddress]
        [Remote(action: "EmailLibre", controller: "Accounts")]
        public string Email { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        [RegularExpression(@"[0-9]{10}", ErrorMessage = "Ingresá tu número telefónico sin 0 ni 15. Ej.: Cód. Área: 11 y Número: 2345678")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = Validaciones.Required)]
        public string Direccion { get; set; }

        [DataType(DataType.Password)]
        [MinLength (5, ErrorMessage = "Ingresa un minimo de 5 caracteres")]
        [Required(ErrorMessage = Validaciones.Required)]
        public string Contrasena { get; set; }

        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Ingresa un minimo de 5 caracteres")]
        [Display(Name = "Confirmación de Password")]
        [Compare("Contrasena", ErrorMessage = "La password de confirmación no es igual. Por favor, verifiquela.")]
        [Required(ErrorMessage = Validaciones.Required)]
        public string ConfirmacionContrasena { get; set; }

        [Required]
        public Guid CarreraId { get; set; }
        


    }
}
