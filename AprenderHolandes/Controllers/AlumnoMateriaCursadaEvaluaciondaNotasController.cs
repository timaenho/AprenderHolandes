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
using AprenderHolandes.ViewModels;

namespace AprenderHolandes.Controllers
{
    public class AlumnoMateriaCursadaEvaluaciondaNotasController : Controller
    {
        private readonly DbContextInstituto _context;
        private readonly UserManager<Persona> _usermanager;

        public AlumnoMateriaCursadaEvaluaciondaNotasController(DbContextInstituto context, UserManager<Persona> usermanager)
        {
            _context = context;
            _usermanager = usermanager;

        }

        // GET: AlumnoMateriaCursadaEvaluaciondaNotas
        public async Task<IActionResult> Index()
        {
            // lista con de  grupos y los mce's --> button ver alumnos
            Profesor profesor = (Profesor)await _usermanager.GetUserAsync(HttpContext.User);
            var listaMateriaCursadas = _context.MateriaCursadas
                .Include(mc => mc.MateriaCursadaEvaluaciones)
                .ThenInclude(mce => mce.Evaluacion)
                .ThenInclude(e => e.Materia)
                .Include(mc => mc.Materia)
                .Where(mc => mc.ProfesorId == profesor.Id).OrderBy(mc => mc.Nombre);
            return View(listaMateriaCursadas);
           
        }
        public async Task<IActionResult> ListaAlumnosPorMateriaCursada(Guid? Id)
        {
            var materiaCursadaEvaluacion =  _context.MateriaCursadaEvaluaciones
                .Include(mce => mce.MateriaCursada)
                .ThenInclude(mc => mc.AlumnoMateriaCursadas)
                .ThenInclude(amc => amc.Alumno)
                .ThenInclude(amc=>amc.AlumnoMateriaCursadaEvaluaciondaNotas)
                .Include(mce=>mce.Evaluacion)
                .FirstOrDefault(mce => mce.Id == Id);


            //Titulo de la pagina
            ViewData["Titulo"] = materiaCursadaEvaluacion.Evaluacion.Titulo;
            var viewModelsAlumnosNota = new List<AlumnoNota>();

             string nota = null;
            //Crear la lista de los viewmodels
            foreach(AlumnoMateriaCursada amc in materiaCursadaEvaluacion.MateriaCursada.AlumnoMateriaCursadas)
            {
                
                 nota = _context.AlumnoMateriaCursadaEvaluaciondaNotas
                               .Include(amcen => amcen.Alumno)
                               .Include(amcen => amcen.MateriaCursadaEvaluacion)
                                .FirstOrDefault(amcen =>
                                amcen.AlumnoId == amc.AlumnoId
                                && amcen.MateriaCursadaEvaluacionId == Id)?.Nota;
                var newAlumnoNota = new AlumnoNota
                {
                    AlumnoMateriaCursada = amc,
                    Nota = nota,
                    MateriaCursadaEvaluacion = materiaCursadaEvaluacion
                };

                viewModelsAlumnosNota.Add(newAlumnoNota);
            }


            return View(viewModelsAlumnosNota);
        }

        //segunda pagina lista de los alumnos --> dar nota

        //index alumno --> todas las notas por evaluación

        // GET: AlumnoMateriaCursadaEvaluaciondaNotas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alumnoMateriaCursadaEvaluaciondaNota = await _context.AlumnoMateriaCursadaEvaluaciondaNotas
                .Include(a => a.Alumno)
                .Include(a => a.Profesor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alumnoMateriaCursadaEvaluaciondaNota == null)
            {
                return NotFound();
            }

            return View(alumnoMateriaCursadaEvaluaciondaNota);
        }

        // GET: AlumnoMateriaCursadaEvaluaciondaNotas/Create
        public IActionResult Create()
        {
            ViewData["AlumnoId"] = new SelectList(_context.Alumnos, "Id", "Discriminator");
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Discriminator");
            return View();
        }

        // POST: AlumnoMateriaCursadaEvaluaciondaNotas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string? AlumnoId, string Nota, string MateriaCursadaEvaluacionId)
        {
            Profesor _Profesor = (Profesor)await _usermanager.GetUserAsync(HttpContext.User);
            if(_Profesor == null)
            {
                return NotFound();
            }

            Alumno _Alumno = (Alumno)_context.Alumnos
                .Include(a => a.AlumnoMateriaCursadaEvaluaciondaNotas)
                .FirstOrDefault(a => a.Id == Guid.Parse(AlumnoId));
            if(_Alumno == null)
            {
                return NotFound();
            }

            var _MateriaCursadaEvaluacion = _context.MateriaCursadaEvaluaciones
              
                .FirstOrDefault(mce => mce.Id == Guid.Parse(MateriaCursadaEvaluacionId));

            if(_MateriaCursadaEvaluacion == null)
            {
                return NotFound();
            }
            var nota = Nota;

            if(nota!="0" && nota != "1" && nota != "2" && nota != "3" && nota != "4" && nota != "5" && nota != "6" && nota != "7" && nota != "8" && nota != "9" &&
                nota != "10")
            {
                TempData["Mensaje"] = "Ingrese un numero >= 0 y <= 10"; 
                return RedirectToAction("ListaAlumnosPorMateriaCursada", new {Id = _MateriaCursadaEvaluacion.Id });
            }

            var amcen = new AlumnoMateriaCursadaEvaluaciondaNota
            {
                Id = Guid.NewGuid(),
                Profesor = _Profesor,
                ProfesorId = _Profesor.Id,
                Alumno = _Alumno,
                AlumnoId = _Alumno.Id,
                Nota = nota,
                MateriaCursadaEvaluacion = _MateriaCursadaEvaluacion,
                MateriaCursadaEvaluacionId = _MateriaCursadaEvaluacion.Id
                
            };
           
            _Alumno.AlumnoMateriaCursadaEvaluaciondaNotas.Add(amcen);
            _context.AlumnoMateriaCursadaEvaluaciondaNotas.Add(amcen);
            _context.Alumnos.Update(_Alumno);
            _context.SaveChanges();

            return RedirectToAction("ListaAlumnosPorMateriaCursada", new { Id = _MateriaCursadaEvaluacion.Id });
        }

        // GET: AlumnoMateriaCursadaEvaluaciondaNotas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alumnoMateriaCursadaEvaluaciondaNota = await _context.AlumnoMateriaCursadaEvaluaciondaNotas.FindAsync(id);
            if (alumnoMateriaCursadaEvaluaciondaNota == null)
            {
                return NotFound();
            }
            ViewData["AlumnoId"] = new SelectList(_context.Alumnos, "Id", "Discriminator", alumnoMateriaCursadaEvaluaciondaNota.AlumnoId);
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Discriminator", alumnoMateriaCursadaEvaluaciondaNota.ProfesorId);
            return View(alumnoMateriaCursadaEvaluaciondaNota);
        }

        // POST: AlumnoMateriaCursadaEvaluaciondaNotas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,AlumnoId,Nota,ProfesorId")] AlumnoMateriaCursadaEvaluaciondaNota alumnoMateriaCursadaEvaluaciondaNota)
        {
            if (id != alumnoMateriaCursadaEvaluaciondaNota.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alumnoMateriaCursadaEvaluaciondaNota);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlumnoMateriaCursadaEvaluaciondaNotaExists(alumnoMateriaCursadaEvaluaciondaNota.Id))
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
            ViewData["AlumnoId"] = new SelectList(_context.Alumnos, "Id", "Discriminator", alumnoMateriaCursadaEvaluaciondaNota.AlumnoId);
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Discriminator", alumnoMateriaCursadaEvaluaciondaNota.ProfesorId);
            return View(alumnoMateriaCursadaEvaluaciondaNota);
        }

        // GET: AlumnoMateriaCursadaEvaluaciondaNotas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alumnoMateriaCursadaEvaluaciondaNota = await _context.AlumnoMateriaCursadaEvaluaciondaNotas
                .Include(a => a.Alumno)
                .Include(a => a.Profesor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alumnoMateriaCursadaEvaluaciondaNota == null)
            {
                return NotFound();
            }

            return View(alumnoMateriaCursadaEvaluaciondaNota);
        }

        // POST: AlumnoMateriaCursadaEvaluaciondaNotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var alumnoMateriaCursadaEvaluaciondaNota = await _context.AlumnoMateriaCursadaEvaluaciondaNotas.FindAsync(id);
            _context.AlumnoMateriaCursadaEvaluaciondaNotas.Remove(alumnoMateriaCursadaEvaluaciondaNota);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlumnoMateriaCursadaEvaluaciondaNotaExists(Guid id)
        {
            return _context.AlumnoMateriaCursadaEvaluaciondaNotas.Any(e => e.Id == id);
        }
    }
}
