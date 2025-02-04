﻿using AprenderHolandes.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Models
{
    public class Profesor : Persona
    {
        //[Required(ErrorMessage = Validaciones.Required)]
        [Required (ErrorMessage =Validaciones.Required)]
        public string Legajo { get; set; }
        public ICollection<MateriaCursada> MateriasCursadasActivas { get; set; }
        public ICollection<Calificacion> CalificacionesRealizadas { get; set; }


    }
}
