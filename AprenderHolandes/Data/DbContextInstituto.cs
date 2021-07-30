using AprenderHolandes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Data
{
    public class DbContextInstituto : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
      
            public DbContextInstituto(DbContextOptions options) : base(options) // constructor contexto
            {


            }

            public DbSet<Persona> Personas { get; set; }
            public DbSet<Alumno> Alumnos { get; set; }
            public DbSet<AlumnoMateriaCursada> AlumnoMateriaCursadas { get; set; }
            public DbSet<Calificacion> Calificaciones { get; set; }
            public DbSet<Carrera> Carreras { get; set; }
            public DbSet<Empleado> Empleados { get; set; }
            public DbSet<Materia> Materias { get; set; }
            public DbSet<MateriaCursada> MateriaCursadas { get; set; }
            public DbSet<Profesor> Profesores { get; set; }
            public DbSet<Rol> Roles { get; set; }

            protected override void OnModelCreating(ModelBuilder modelbuilder)
            {
                base.OnModelCreating(modelbuilder);

                #region N:M Alumno MateriaCursada -> AlumnoMateriaCursada

                modelbuilder.Entity<AlumnoMateriaCursada>()
                       .HasKey(am => new { am.AlumnoId, am.MateriaCursadaId });

                modelbuilder.Entity<AlumnoMateriaCursada>()
                    .HasOne(ma => ma.Alumno)
                    .WithMany(a => a.AlumnosMateriasCursadas)
                    .HasForeignKey(ma => ma.AlumnoId);

                modelbuilder.Entity<AlumnoMateriaCursada>()
                    .HasOne(ma => ma.MateriaCursada)
                    .WithMany(m => m.AlumnoMateriaCursadas)
                    .HasForeignKey(ma => ma.MateriaCursadaId);


            #endregion

                #region N:M MateriaCursada --> Evaluaciòn 
            #endregion

                #region Model Builders
            modelbuilder.Entity<IdentityUser<Guid>>().ToTable("Personas");
                modelbuilder.Entity<IdentityRole<Guid>>().ToTable("Roles");
                modelbuilder.Entity<IdentityUserRole<Guid>>().ToTable("PersonasRoles");
                #endregion

            }
        }
}
