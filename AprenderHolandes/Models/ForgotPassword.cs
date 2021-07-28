using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class ForgotPassword
    {
        public int Id { get; set; }
        [Required,EmailAddress, Display(Name = "Correo Registrado")]
        public string Email { get; set; }
        public bool Emailsent { get; set; }
    }
}
