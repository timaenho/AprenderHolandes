using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AprenderHolandes.Data;
using AprenderHolandes.Models;

namespace AprenderHolandes.Controllers
{
    public class AlumnoMateriaCursadaEvaluaciondaNotasController : Controller
    {
        private readonly DbContextInstituto _context;

        public AlumnoMateriaCursadaEvaluaciondaNotasController(DbContextInstituto context)
        {
            _context = context;
        }

        // GET: AlumnoMateriaCursadaEvaluaciondaNotas
        public async Task<IActionResult> Index()
        {
            var dbContextInstituto = _context.AlumnoMateriaCursadaEvaluaciondaNotas.Include(a => a.Alumno).Include(a => a.Profesor);
            return View(await dbContextInstituto.ToListAsync());
        }

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
        public async Task<IActionResult> Create([Bind("Id,AlumnoId,Nota,ProfesorId")] AlumnoMateriaCursadaEvaluaciondaNota alumnoMateriaCursadaEvaluaciondaNota)
        {
            if (ModelState.IsValid)
            {
                alumnoMateriaCursadaEvaluaciondaNota.Id = Guid.NewGuid();
                _context.Add(alumnoMateriaCursadaEvaluaciondaNota);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlumnoId"] = new SelectList(_context.Alumnos, "Id", "Discriminator", alumnoMateriaCursadaEvaluaciondaNota.AlumnoId);
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Discriminator", alumnoMateriaCursadaEvaluaciondaNota.ProfesorId);
            return View(alumnoMateriaCursadaEvaluaciondaNota);
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
