using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.ViewModels
{
    public class CambiarContrasenia
    {

        public int Id { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "Contraseña actual")]
        public string CurrentPassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "Contraseña nueva")]
        public string NewPassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "Confirmar contraseña nueva")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarNuevaContrasenia { get; set; }
    }
}
