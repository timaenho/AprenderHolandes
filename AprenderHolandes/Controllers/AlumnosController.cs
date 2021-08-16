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

namespace AprenderHolandes.Controllers
{
    [Authorize(Roles = "Alumno, Empleado")]
    public class AlumnosController : Controller
    {
        private readonly DbContextInstituto _context;
        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly RoleManager<Rol> _roleManager;

        public AlumnosController(DbContextInstituto context, UserManager<Persona> userManager, SignInManager<Persona> signInManager, RoleManager<Rol> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: Alumnos
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Index()
        {

            var dbContextInstituto = _context.Alumnos
                .Include(a => a.AlumnosMateriasCursadas)
                .ThenInclude(amc => amc.Calificacion)
                .Include(a => a.Carrera);
            return View(await dbContextInstituto.ToListAsync());
        }

        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumnos
                .Include(a => a.Carrera)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alumno == null)
            {
                return NotFound();
            }

            return View(alumno);
        }

        // GET: Alumnos/Create
        [Authorize(Roles = "Empleado")]
        public IActionResult Create()
        {
            TempData["Message"] = "Se otorga una contraseña por sistema la cual es 'Password1'.";
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "CarreraId", "Nombre");
            return View();
        }

        //POST: Alumnos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Create([Bind("Activo,NumeroMatricula,FechaAlta,CarreraId,Nombre,Apellido,Dni,Telefono,Direccion,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {

                var contrasenia = "Password1";
                var Alumnos = _context.Alumnos;
                var MatriculaMax = 0;

                alumno.Activo = true;
                alumno.Id = Guid.NewGuid();
                alumno.FechaAlta = DateTime.Now;
                alumno.UserName = alumno.Email;

                foreach (Alumno a in Alumnos)
                {
                    if (a.NumeroMatricula != 0)
                    {
                        var MatriculaAlumno = a.NumeroMatricula;
                        if (MatriculaAlumno > MatriculaMax)
                        {
                            MatriculaMax = MatriculaAlumno;
                        }
                    }
                }

                MatriculaMax = MatriculaMax + 1;
                alumno.PasswordHash = contrasenia;

                var resultado = await _userManager.CreateAsync(alumno, alumno.PasswordHash);
                if (resultado.Succeeded)
                {
                    Rol rolAlumno = null;
                    var name = "Alumno";
                    rolAlumno = await _roleManager.FindByNameAsync(name);

                    if (rolAlumno == null)
                    {
                        rolAlumno = new Rol();
                        rolAlumno.Name = name;
                        var resultNewRol = await _roleManager.CreateAsync(rolAlumno);
                    }

                    var resultAddToRol = await _userManager.AddToRoleAsync(alumno, name);
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "CarreraId", "Nombre", alumno.CarreraId);
            return View(alumno);
        }

        //GET: Alumnos/Edit/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null)
            {
                return NotFound();
            }
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "CarreraId", "Nombre", alumno.CarreraId);
            return View(alumno);
        }

        // POST: Alumnos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Activo,NumeroMatricula,CarreraId,FechaAlta,Nombre,Apellido,Dni,Telefono,Direccion,Legajo,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] Alumno alumno)
        {
            if (id != alumno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alumno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlumnoExists(alumno.Id))
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
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "CarreraId", "Nombre", alumno.CarreraId);
            return View(alumno);
        }

        // GET: Alumnos/Delete/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumnos
                .Include(a => a.Carrera)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (alumno == null)
            {
                return NotFound();
            }

            return View(alumno);
        }

        // POST: Alumnos/Delete/5
        [Authorize(Roles = "Empleado")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var alumno = await _context.Alumnos
                .Include(a => a.AlumnosMateriasCursadas)
                .ThenInclude(amc => amc.Calificacion)
                .Include(a => a.Carrera)
                .FirstOrDefaultAsync(m => m.Id == id);

            var tieneCalificacion = false;

            foreach (AlumnoMateriaCursada amc in alumno.AlumnosMateriasCursadas)
            {
                if (amc.Calificacion.NotaFinal != -1111)
                {
                    tieneCalificacion = true;
                    break;
                }
            }

            if (!tieneCalificacion || alumno.AlumnosMateriasCursadas == null)
            {
                if (alumno.AlumnosMateriasCursadas != null)
                {
                    foreach (AlumnoMateriaCursada amc in alumno.AlumnosMateriasCursadas)
                    {
                        _context.Calificaciones.Remove(amc.Calificacion);
                        // _context.AlumnoMateriaCursadas.Remove(amc);

                    }
                }
                _context.SaveChanges();
                _context.Alumnos.Remove(alumno);
                await _context.SaveChangesAsync();
                TempData["MensajeExito"] = "El alumno esta eliminado con exito";
            }
            else
            {

                TempData["MensajeFalla"] = "El alumno tiene una calificacion asociada no podes eliminar el alumno";
            }

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Empleado")]
        private bool AlumnoExists(Guid id)
        {
            return _context.Alumnos.Any(e => e.Id == id);
        }

        [Authorize(Roles = "Alumno")]
        public async Task<IActionResult> RegistrarMaterias()
        {
            //var alumno = _userManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            Alumno alumno = (Alumno)await _userManager.GetUserAsync(HttpContext.User);
            var carreraId = alumno.CarreraId;
            var carrera = await _context.Carreras.Include(carrera => carrera.Materias)
                .ThenInclude(m => m.MateriasCursadas)
                .ThenInclude(mc => mc.AlumnoMateriaCursadas)
                .FirstOrDefaultAsync(c => carreraId == c.CarreraId);
            bool esta = false;
            bool esActivo = false;
            //var materias = carrera.Materias;
            var listaMaterias = new List<Materia>();
            if (alumno.Activo == true)
            {
                foreach (Materia m in carrera.Materias)
                {
                    foreach (MateriaCursada mc in m.MateriasCursadas)
                    {
                        if (mc.Activo) { esActivo = true; }
                        foreach (AlumnoMateriaCursada amc in mc.AlumnoMateriaCursadas)
                        {
                            if (amc.AlumnoId == alumno.Id)
                            {
                                esta = true;
                                break;
                            }
                        }
                        if (esta)
                        {
                            break;
                        }

                    }
                    if (!esta && esActivo)
                    {
                        listaMaterias.Add(m);
                    }
                    esta = false;
                    esActivo = false;
                }


                return View(listaMaterias);

            }
            else
            {
                return RedirectToAction("AccesoDenegado", "Accounts");
            }



        }
        //[HttpPost, ActionName("AgregarMateria")]
        [Authorize(Roles = "Alumno")]
        public async Task<IActionResult> AgregarMateriaMostrar(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materias.FindAsync(id);
            if (materia == null)
            {
                return NotFound();
            }

            return View(materia);
        }

        [Authorize(Roles = "Alumno")]
        public async Task<IActionResult> AgregarMateria(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = _context.Materias.Include(mc => mc.MateriasCursadas)
                .ThenInclude(ma => ma.AlumnoMateriaCursadas)
                .ThenInclude(m => m.MateriaCursada)
                .FirstOrDefault(m => m.MateriaId == id);

            if (materia == null)
            {
                return NotFound();
            }
            var alumnoid = Guid.Parse(_userManager.GetUserId(User));
            var alumno = _context.Alumnos.Include(a => a.AlumnosMateriasCursadas)
                .ThenInclude(am => am.MateriaCursada)
                .ThenInclude(mc => mc.Materia)
                .FirstOrDefault(a => a.Id == alumnoid);
            var cont = 0;
            if (alumno != null)
            {
                foreach (AlumnoMateriaCursada amc in alumno.AlumnosMateriasCursadas)
                {
                    if (amc.MateriaCursada.Activo)
                    {
                        cont++;
                    }
                }
                if (cont > 5)
                {
                    TempData["message"] = "No podes inscribirte en mas de 5 materias";
                    return RedirectToAction("RegistrarMaterias", "Alumnos");
                }
                else
                {
                    bool encontrado = false;
                    foreach (AlumnoMateriaCursada alumnomateriaCursada in alumno.AlumnosMateriasCursadas)
                    {
                        if (alumnomateriaCursada.MateriaCursada.Materia == materia)
                        {
                            encontrado = true;
                        }
                    }
                    if (encontrado == true)
                    {
                        TempData["message"] = "Ya te inscribiste para esta materia";
                        return RedirectToAction("RegistrarMaterias");
                    }
                    else
                    {
                        if (materia.MateriasCursadas.Count == 0)
                        {
                            TempData["message"] = "Disculpa pero, todavia no hay grupos disponibles. Intentá inscribirte en otro momento";
                            return RedirectToAction("RegistrarMaterias");
                        }

                        #region
                        int cupoMax = materia.CupoMaximo;
                        MateriaCursada materiaCursadaLibre = null;
                        var MateriaConMateriasCursadas = _context.Materias
                            .Include(m => m.MateriasCursadas)
                            .FirstOrDefault(m => m.MateriaId == materia.MateriaId);

                        foreach (MateriaCursada mc in materia.MateriasCursadas)
                        {
                            if (mc.AlumnoMateriaCursadas.Count < cupoMax || mc.AlumnoMateriaCursadas == null)
                            {
                                materiaCursadaLibre = mc;
                            }
                        }
                   

                        #endregion
                        Guid calificacionId = Guid.NewGuid();

                        AlumnoMateriaCursada amc = new AlumnoMateriaCursada
                        {

                            Alumno = alumno,
                            AlumnoId = alumno.Id,
                            MateriaCursada = materiaCursadaLibre,
                            MateriaCursadaId = materiaCursadaLibre.MateriaCursadaId,
                            CalificacionId = calificacionId

                        };

                        Calificacion calificacion = new Calificacion
                        {
                            AlumnoMateriaCursada = amc,
                            CalificacionId = calificacionId,
                            Profesor = amc.MateriaCursada.Profesor,
                            ProfesorId = amc.MateriaCursada.ProfesorId,
                            Materia = materia,
                            NotaFinal = -1111
                        };
                        MateriaCursada materiaCursada = _context.MateriaCursadas
                            .Include(mc => mc.Calificaciones)
                            .FirstOrDefault(mc => mc.MateriaCursadaId == amc.MateriaCursadaId);


                        materiaCursada.Calificaciones.Add(calificacion);
                        _context.AlumnoMateriaCursadas.Add(amc);
                        alumno.AlumnosMateriasCursadas.Add(amc);
                        _context.Calificaciones.Add(calificacion);
                        _context.Alumnos.Update(alumno);
                        _context.SaveChanges();

                        calificacion.AlumnoMateriaCursada = amc;
                        _context.Calificaciones.Update(calificacion);
                        _context.SaveChanges();
                    }

                }

            }
            else
            {
                return NotFound();
            }



            TempData["message"] = "Te inscribiste con exito";
            return RedirectToAction("RegistrarMaterias");
        }

        //private MateriaCursada checkMateriaCursadaLibre(Materia materia)
        //{
        //    int cupoMax = materia.CupoMaximo;
        //    MateriaCursada materiaCursadaLibre = null;

        //    foreach (MateriaCursada mc in materia.MateriasCursadas)
        //    {
        //        if (mc.AlumnoMateriaCursadas.Count < cupoMax|| mc.AlumnoMateriaCursadas == null)
        //        {
        //            materiaCursadaLibre = mc;
        //        }
        //    }
        //    if (materiaCursadaLibre == null)
        //    {

        //        var firstMateriaCursada = materia.MateriasCursadas.First();
        //        materiaCursadaLibre = new MateriaCursada
        //        {
        //            MateriaCursadaId = Guid.NewGuid(),
        //            MateriaId = materia.MateriaId,
        //            Anio = firstMateriaCursada.Anio,
        //            Cuatrimestre = firstMateriaCursada.Cuatrimestre,
        //            Activo = true,
        //            Materia = firstMateriaCursada.Materia,
        //            ProfesorId = firstMateriaCursada.ProfesorId,
        //            Nombre = materia.Nombre+firstMateriaCursada.Anio.ToString()+ materia.MateriasCursadas.Count.ToString()

        //            };
        //        _context.MateriaCursadas.Add(materiaCursadaLibre);
        //        _context.SaveChanges();
        //        }
        //    return materiaCursadaLibre;
        //    }

        [Authorize(Roles = "Alumno")]
        public async Task<IActionResult> VerMateriasCursadasAlumno()
        {

            var alumnoid = Guid.Parse(_userManager.GetUserId(User));
            var alumno = _context.Alumnos.Include(a => a.AlumnosMateriasCursadas).ThenInclude(am => am.Calificacion).ThenInclude(c => c.Materia).FirstOrDefault(a => a.Id == alumnoid);


            if (alumno.AlumnosMateriasCursadas == null)
            {
                return NotFound();
            }
            var alumnosMateriasCursadas = new List<AlumnoMateriaCursada>();
            foreach (AlumnoMateriaCursada am in alumno.AlumnosMateriasCursadas)
            {
                if (!(am.Calificacion.NotaFinal == -1111))
                {
                    alumnosMateriasCursadas.Add(am);


                }
            }

            return View(alumnosMateriasCursadas);
        }


        [Authorize(Roles = "Alumno")]
        public async Task<IActionResult> MisMaterias()
        {
            var alumnoid = Guid.Parse(_userManager.GetUserId(User));
            var Alumno = _context.Alumnos
                .Include(a => a.AlumnosMateriasCursadas)
                .ThenInclude(amc => amc.MateriaCursada)
                .ThenInclude(mc => mc.Materia)
                .FirstOrDefault(a => a.Id == alumnoid);
            var alumnoMateriasCursadas = Alumno.AlumnosMateriasCursadas;
            var amcActivos = new List<AlumnoMateriaCursada>();
            foreach (AlumnoMateriaCursada amc in alumnoMateriasCursadas)
            {
                if (amc.MateriaCursada.Activo)
                {
                    amcActivos.Add(amc);
                }
            }
            return View(amcActivos);
        }

        [Authorize(Roles = "Alumno")]
        public async Task<IActionResult> CancelarInscripcion(Guid? Id)

        {
            if (Id == null)
            {
                return NotFound();
            }
            var alumnoid = Guid.Parse(_userManager.GetUserId(User));
            var alumno = _context.Alumnos
               .FirstOrDefault(a => a.Id == alumnoid);

            var alumnosMateriaCursada = _context.AlumnoMateriaCursadas
                .Include(amc => amc.Calificacion)
                .FirstOrDefault(amc => amc.AlumnoId == alumnoid && amc.MateriaCursadaId == Id);
            if (alumnosMateriaCursada.Calificacion.NotaFinal != -1111)
            {
                TempData["Message"] = "No podes eliminar una materia cursada con una calificacion asociada";
                return RedirectToAction("MisMaterias");
            }

            alumno.AlumnosMateriasCursadas.Remove(alumnosMateriaCursada);
            _context.AlumnoMateriaCursadas.Remove(alumnosMateriaCursada);
            _context.Calificaciones.Remove(alumnosMateriaCursada.Calificacion);
            _context.Alumnos.Update(alumno);
            _context.SaveChanges();
            return RedirectToAction("MisMaterias");
        }


        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> ActivarAlumno(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumnos.FindAsync(id);

            if (alumno == null)
            {
                return NotFound();
            }

            if (alumno.Activo == false)
            {
                alumno.Activo = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Alumno")]
        public async Task<IActionResult> MisMateriasFinalizadas()
        {
            var alumnoid = Guid.Parse(_userManager.GetUserId(User));
            var Alumno = _context.Alumnos
                .Include(a => a.AlumnosMateriasCursadas)
                .ThenInclude(amc => amc.MateriaCursada)
                .ThenInclude(mc => mc.Materia)
                .Include(a => a.AlumnosMateriasCursadas)
                .ThenInclude(amc => amc.Calificacion)
                .FirstOrDefault(a => a.Id == alumnoid);
            var alumnoMateriasCursadas = Alumno.AlumnosMateriasCursadas;
            var amcInActivos = new List<AlumnoMateriaCursada>();
            foreach (AlumnoMateriaCursada amc in alumnoMateriasCursadas)
            {
                if (!amc.MateriaCursada.Activo && amc.Calificacion.NotaFinal != -1111)
                {
                    amcInActivos.Add(amc);
                }
            }
            return View(amcInActivos);
        }
        public async Task <IActionResult> ContenidoIndex()
        {
            //el estudiante sole puede tener una materiacursada activa
            var alumnoid = Guid.Parse(_userManager.GetUserId(User));
            var alumno = _context.Alumnos
                .Include(a=>a.Carrera)
                .Include(a => a.AlumnosMateriasCursadas)
                .ThenInclude(amc => amc.MateriaCursada)
                .ThenInclude(mc => mc.Materia)
                .FirstOrDefault(a => a.Id == alumnoid);
            var amcs = alumno.AlumnosMateriasCursadas;
            var listMC = new List<MateriaCursada>();
            MateriaCursada mcActivo = null;

            var nivel1 = "Nivel 1";
            var nivel2 = "Nivel 2";
            var nivel3 = "Nivel 3";
            var nivel4 = "Nivel 4";
            var nivel5 = "Nivel 5";
            var nivel6 = "Nivel 6";

            var holandes = "Holandés";


            foreach (AlumnoMateriaCursada amcAct in amcs)
            {
                if (amcAct.MateriaCursada.Activo)
                {
                     mcActivo = amcAct.MateriaCursada;
                }
             
            }

            if (mcActivo.Materia.Nombre == nivel1 && alumno.Carrera.Nombre == holandes)
            {
                return View("IndexHolandesNivel1");
            }
            if (mcActivo.Materia.Nombre == nivel2 && alumno.Carrera.Nombre == holandes)
            {
                return View("IndexHolandesNivel2");
            }
            if (mcActivo.Materia.Nombre == nivel3 && alumno.Carrera.Nombre == holandes)
            {
                return View("IndexHolandesNivel3");
            }
            if (mcActivo.Materia.Nombre == nivel4 && alumno.Carrera.Nombre == holandes)
            {
                return View("IndexHolandesNivel4");
            }
            if (mcActivo.Materia.Nombre == nivel5 && alumno.Carrera.Nombre == holandes)
            {
                return View("IndexHolandesNivel5");
            }
            if (mcActivo.Materia.Nombre == nivel6 && alumno.Carrera.Nombre == holandes)
            {
                return View("IndexHolandesNivel6");
            }

            return View("AccesoDenegado", "Accounts");
        }


        #region Holandes Nivel 1
        public IActionResult HolandesNivel1Clase1()
        {
            return View();
        }
        public IActionResult HolandesNivel1Clase2()
        {
            return View();
        }
        public IActionResult HolandesNivel1Clase3()
        {
            return View();
        }
        public IActionResult HolandesNivel1Clase4()
        {
            return View();
        }
        public IActionResult HolandesNivel1Clase5()
        {
            return View();
        }
        public IActionResult HolandesNivel1Clase6()
        {
            return View();
        }
        public IActionResult HolandesNivel1Clase7()
        {
            return View();
        }
        public IActionResult HolandesNivel1Clase8()
        {
            return View();
        }
        public IActionResult HolandesNivel1Clase9()
        {
            return View();
        }

        //....
        #endregion


    }



}
