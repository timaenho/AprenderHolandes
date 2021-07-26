using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Data
{
    public class Validaciones
    {
        public const string Required = "El campo {0} es requerido";
        public const string MaxLength = "El campo {0} admite un máximo de {1} caracteres";
        public const string Dni = "Ingresá tu DNI sin puntos. Ej.: 12345678";
        public const string Email = "Ingresá una dirección de correo electrónico válida";
        public const string Telefono = "Ingresá tu número telefónico sin 0 ni 15. Ej.: Cód. Área: 11 y Número: 2345678";
        public const string NumeroMayorMenorA = "Ingresa un numero mayor a {1} y menor a {2}";
    }
}
