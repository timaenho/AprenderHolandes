using AprenderHolandes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.ViewModels
{
    public class MateriaCursadaConNotaPromedio
    {
        public int Id { get; set; }

        public MateriaCursada materiaCursada { get; set; }
        
        public Guid materiaCursadaId 
        { get
            {
                return materiaCursada.MateriaCursadaId;
            } 
        }
        public string Nombre
        {
            get
            {
                return materiaCursada.Nombre;
            }
        }
        public int Anio
        {
            get
            {
                return materiaCursada.Anio;
            }
        }

        public int Cuatrimestre
        {
            get
            {
                return materiaCursada.Cuatrimestre;
            }
        }

        public double NotaPromedio
        {
            get
            { double promedio = 0;
                double total = 0;
                double cont = 0;
                foreach (Calificacion c in materiaCursada.Calificaciones)
                {
                    if (c.NotaFinal != -1111)
                    {
                        total += c.NotaFinal;
                        cont++;
                    }
                }
                promedio = total / cont;
                return promedio;
            }
        }

    }
}
