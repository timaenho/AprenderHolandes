using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AprenderHolandes.Data;
using AprenderHolandes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AprenderHolandes.ViewModels;

namespace AprenderHolandes.Controllers
{
    [Authorize]
    public class ProfesoresController : Controller
    {
        private readonly DbContextInstituto _context;
        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly RoleManager<Rol> _rolManager;

        public ProfesoresController(DbContextInstituto context, UserManager<Persona> userManager, SignInManager<Persona> signInManager, RoleManager<Rol> rolManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _rolManager = rolManager;

        }

        // GET: Profesores
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Profesores.ToListAsync());
        }

        // GET: Profesores/Details/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profesor = await _context.Profesores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profesor == null)
            {
                return NotFound();
            }

            return View(profesor);
        }

        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> ListarMateriasCursadas(Guid? Id)
        {
            Profesor profesor = (Profesor)await _userManager.GetUserAsync(HttpContext.User);
            List<MateriaCursadaConNotaPromedio> listaMateriasActivasPorProfesor = new List<MateriaCursadaConNotaPromedio>();
            List<int> promedios = new List<int>();
            var materiaCursadas = _context.MateriaCursadas
                .Include(mc => mc.Calificaciones);

            var Materia = _context.Materias.FirstOrDefault(m => m.MateriaId == Id);
            ViewData["NombreMateria"] = Materia.Nombre;

            if (materiaCursadas == null)
            {
                ViewData["Message"] = "No hay materias cursadas";
                return View();
            }
            else

            {
                foreach (MateriaCursada mc in materiaCursadas)
                {
                    if (mc.ProfesorId == profesor.Id && mc.Activo && mc.MateriaId == Id)
                    {
                        MateriaCursadaConNotaPromedio mcp = new MateriaCursadaConNotaPromedio
                        {
                            materiaCursada = mc
                        };

                        listaMateriasActivasPorProfesor.Add(mcp);

                    }
                }
            }
            return View(listaMateriasActivasPorProfesor);
        }

        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> ListarMateriasCursadasFinalizadas()
        {
            Profesor profesor = (Profesor)await _userManager.GetUserAsync(HttpContext.User);
            List<MateriaCursadaConNotaPromedio> listaMateriasInActivasPorProfesor = new List<MateriaCursadaConNotaPromedio>();
            List<int> promedios = new List<int>();
            var materiaCursadas = _context.MateriaCursadas
                .Include(mc => mc.AlumnoMateriaCursadas)
                .ThenInclude(amc => amc.Calificacion)
                .Where(mc => profesor.Id == mc.ProfesorId && mc.Activo == false);
            Boolean listaConCalificiaciones = true;
            if (materiaCursadas == null)
            {
                ViewData["Message"] = "No hay materias cursadas";
                return View();
            }
            else

            {
                foreach (MateriaCursada mc in materiaCursadas)
                {
                    if (mc.AlumnoMateriaCursadas.Count != 0)
                    {
                        foreach (AlumnoMateriaCursada amc in mc.AlumnoMateriaCursadas)
                        {
                            if (amc.Calificacion.NotaFinal == -1111)
                            {
                                listaConCalificiaciones = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        listaConCalificiaciones = false;
                    }

                    if (listaConCalificiaciones)
                    {
                        MateriaCursadaConNotaPromedio mcp = new MateriaCursadaConNotaPromedio
                        {

                            materiaCursada = mc
                        };
                        listaMateriasInActivasPorProfesor.Add(mcp);
                    }

                    listaConCalificiaciones = true;


                }
            }
            return View(listaMateriasInActivasPorProfesor);
        }



        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> MostrarAlumnosPorMateriaCursada(Guid? id) // esta bien
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaCursada = _context.MateriaCursadas
                .Include(mc => mc.AlumnoMateriaCursadas)
                .ThenInclude(amc => amc.Alumno)
                .Include(mc => mc.AlumnoMateriaCursadas)
                .ThenInclude(amc => amc.Calificacion)
                .FirstOrDefault(mc => mc.MateriaCursadaId == id);

            ViewData["MateriaCursadaNombre"] = materiaCursada.Nombre;

            return View(materiaCursada.AlumnoMateriaCursadas);
        }


        // GET: Profesores/Create
        [Authorize(Roles = "Empleado")]
        public IActionResult Create()
        {
            TempData["Message"] = "Se otorga una contraseña por sistema la cual es 'Password1'.";

            return View();
        }

        // POST: Profesores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Empleado"))]
        public async Task<IActionResult> Create([Bind("FechaAlta,Nombre,Apellido,Dni,Telefono,Direccion,Legajo,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] Profesor profesor)
        {
            if (ModelState.IsValid)
            {
                var contrasenia = "Password1";
                var Legajo = "prof - " + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Second + _context.Empleados.Count();
                profesor.Legajo = Legajo;
                profesor.Id = Guid.NewGuid();
                profesor.FechaAlta = DateTime.Today;
                profesor.UserName = profesor.Email;
                profesor.PasswordHash = contrasenia;

                var resultado = await _userManager.CreateAsync(profesor, profesor.PasswordHash);
                if (resultado.Succeeded)
                {
                    Rol rolProfesor = null;
                    var name = "Profesor";
                    rolProfesor = await _rolManager.FindByNameAsync(name);

                    if (rolProfesor == null)
                    {
                        rolProfesor = new Rol();
                        rolProfesor.Name = name;
                        var resultNewRol = await _rolManager.CreateAsync(rolProfesor);
                    }

                    var resultAddToRol = await _userManager.AddToRoleAsync(profesor, name);
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }


            }
            return View(profesor);
        }


        // GET: Profesores/Edit/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profesor = await _context.Profesores.FindAsync(id);
            if (profesor == null)
            {
                return NotFound();
            }
            return View(profesor);
        }

        // POST: Profesores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(Guid id, [Bind("FechaAlta,Nombre,Apellido,Dni,Telefono,Direccion,Legajo,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] Profesor profesor)
        {
            if (id != profesor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profesor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfesorExists(profesor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(profesor);
        }

        // GET: Profesores/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var profesor = await _context.Profesores
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (profesor == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(profesor);
        //}

        // POST: Profesores/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles ="Empleado")]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    var profesor = await _context.Profesores.FindAsync(id);
        //    _context.Profesores.Remove(profesor);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool ProfesorExists(Guid id)
        {
            return _context.Profesores.Any(e => e.Id == id);
        }

        [AllowAnonymous]
        public async Task<IActionResult> MostrarProfesores()
        {

            return View(_context.Profesores);
        }


        //        //Dejo lo que seria la logica en general, pero de esta manera no funciona correctamente

        //public async Task<IActionResult> NotaPromedioMateriaCursada()
        //{
        //    Profesor profesor = (Profesor)await _userManager.GetUserAsync(HttpContext.User);
        //    var materiaCursadas = _context.MateriaCursadas
        //        .Include(mc => mc.AlumnoMateriaCursadas)
        //        .ThenInclude(amc => amc.Alumno)
        //        .FirstOrDefault(m => m.ProfesorId == profesor.Id);
        //    if (materiaCursadas == null)
        //    {
        //        return View();
        //    }

        //    var materiaCursada = _context.MateriaCursadas.ToList();
        //    var materiaCursada = profesor.MateriasCursadasActivas;
        //    int calificaciones = 0;
        //    Double promedio = 0;
        //    int alumnosPorCursada;

        //    foreach (var mCursada in materiaCursada)
        //    {
        //        alumnosPorCursada = mCursada.AlumnoMateriaCursadas.Count;

        //        foreach (var amc in mCursada.AlumnoMateriaCursadas)
        //        {
        //            calificaciones = +amc.Calificacion.NotaFinal;
        //        }

        //        promedio = calificaciones / alumnosPorCursada;
        //    }

        //    promedio = Math.Round(promedio, 2, MidpointRounding.AwayFromZero);

        //    return View("ListarMateriasCursadas", promedio);
        //}

        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> MisMaterias()
        {
            Profesor profesor = (Profesor)await _userManager.GetUserAsync(HttpContext.User);
            var materiasCursadas = _context.MateriaCursadas
                .Include(mc => mc.Materia)
                .ThenInclude(m => m.Calificaciones)
                .Where(mc => mc.ProfesorId == profesor.Id && mc.Activo);
            List<MisMateriasConNotaPromedio> materiasConPromedio = new List<MisMateriasConNotaPromedio>();

            foreach (MateriaCursada mc in materiasCursadas)
            {

                if (materiasConPromedio.FirstOrDefault(mcp => mcp.materia.MateriaId == mc.MateriaId) == null)
                {
                    var newMcp = new MisMateriasConNotaPromedio
                    {
                        materia = mc.Materia
                    };

                    materiasConPromedio.Add(newMcp);
                }
            }

            return View(materiasConPromedio);
        }

        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> MisAlumnos()
        {
            Profesor profesor = (Profesor)await _userManager.GetUserAsync(HttpContext.User);
            var ListaMateriasCursadas = _context.MateriaCursadas
                .Include(mc => mc.Materia)
                .Include(mc => mc.AlumnoMateriaCursadas)
                .ThenInclude(amc => amc.Alumno)
                .Include(mc => mc.AlumnoMateriaCursadas)
                .ThenInclude(amc => amc.Calificacion)
                .Where(mc => mc.ProfesorId == profesor.Id)
                .OrderByDescending(mc => mc.Activo)
                .ThenBy(mc => mc.MateriaId).ThenBy(mc => mc.MateriaCursadaId);

            return View(ListaMateriasCursadas);


        }

        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> MisMateriaFuturos()
        {
            Profesor profesor = (Profesor)await _userManager.GetUserAsync(HttpContext.User);
            List<MateriaCursada> listaMateriasInActivasPorProfesorSinAlumnos = new List<MateriaCursada>();
            List<int> promedios = new List<int>();
            var materiaCursadas = _context.MateriaCursadas
                .Include(mc => mc.AlumnoMateriaCursadas)
                .Include(mc => mc.Materia)
                .Where(mc => profesor.Id == mc.ProfesorId && mc.Activo == false);

            if (materiaCursadas == null)
            {
                ViewData["Message"] = "No hay materias cursadas";
                return View();
            }
            else

            {
                foreach (MateriaCursada mc in materiaCursadas)
                {
                    if (mc.AlumnoMateriaCursadas.Count == 0)
                    {
                        listaMateriasInActivasPorProfesorSinAlumnos.Add(mc);
                    }
                }
                return View(listaMateriasInActivasPorProfesorSinAlumnos);



            }
        }
    }

}
