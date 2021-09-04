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

namespace AprenderHolandes.Controllers
{
    public class EvaluacionesController : Controller
    {
        private readonly DbContextInstituto _context;
        private readonly UserManager<Persona> _userManager;

        public EvaluacionesController(DbContextInstituto context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Evaluaciones
        public async Task<IActionResult> Index()
        {
            var evaluaciones =  _context.Evaluaciones
                .Include(e => e.Materia).OrderBy(e => e.Materia).ThenBy(e => e.Titulo);

            return View(evaluaciones.ToList());
        }

        // GET: Evaluaciones/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluacion = await _context.Evaluaciones
                .Include(e => e.Materia)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evaluacion == null)
            {
                return NotFound();
            }

            return View(evaluacion);
        }

        // GET: Evaluaciones/Create
        public async Task<IActionResult>Create()
        {
            Profesor profesor = (Profesor)await _userManager.GetUserAsync(HttpContext.User);
            ViewData["MateriaId"] = new SelectList(_context.Materias, "MateriaId", "Nombre");
            ViewData["ProfesorId"] = profesor.Id;
            return View();
        }

        // POST: Evaluaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descripcion, ProfesorId,MateriaId ")] Evaluacion evaluacion)
        {
            if (ModelState.IsValid)
            {
                evaluacion.Id = Guid.NewGuid();
                _context.Add(evaluacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(evaluacion);
        }

        // GET: Evaluaciones/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            Profesor profesor = (Profesor) await _userManager.GetUserAsync(HttpContext.User);
            if (id == null)
            {
                return NotFound();
            }

            var evaluacion = await _context.Evaluaciones.FindAsync(id);
            if (evaluacion == null)
            {
                return NotFound();
            }
            ViewData["MateriaId"] = new SelectList(_context.Materias, "MateriaId", "Nombre");
            ViewData["ProfesorId"] = profesor.Id;
            return View(evaluacion);
        }

        // POST: Evaluaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Titulo,Descripcion, ProfesorId, MateriaId")] Evaluacion evaluacion)
        {
            if (id != evaluacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    

                    _context.Update(evaluacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvaluacionExists(evaluacion.Id))
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
            return View(evaluacion);
        }

        // GET: Evaluaciones/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluacion = await _context.Evaluaciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evaluacion == null)
            {
                return NotFound();
            }

            return View(evaluacion);
        }

        // POST: Evaluaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var evaluacion = await _context.Evaluaciones.FindAsync(id);
            _context.Evaluaciones.Remove(evaluacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvaluacionExists(Guid id)
        {
            return _context.Evaluaciones.Any(e => e.Id == id);
        }
    }
}
