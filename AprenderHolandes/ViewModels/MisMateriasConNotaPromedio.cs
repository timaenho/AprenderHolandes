using AprenderHolandes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.ViewModels
{
    public class MisMateriasConNotaPromedio
    {
        public int id { get; set; }

        public Materia materia { get; set; }

        public Guid materiaId
        {
            get
            {
                return materia.MateriaId;
            }
        }
        public string CodigoMateria
        {
            get
            {
                return materia.CodigoMateria;
            }
        }

        public double promedioMateria
        {
            get
            {
                var total = 0;
                var cont = 0;
                foreach (Calificacion c in materia.Calificaciones)
                {
                   
                    if(c.NotaFinal != -1111)
                    {
                        total += c.NotaFinal;
                        cont++;
                    }
                }

                if(cont == 0)
                {
                    return -1111;
                }
                double promedio = total / cont;
                return promedio;
            }
        }


    }
}
