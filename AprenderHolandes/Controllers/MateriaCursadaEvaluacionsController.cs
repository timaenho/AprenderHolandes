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
    public class MateriaCursadaEvaluacionsController : Controller
    {
        private readonly DbContextInstituto _context;

        public MateriaCursadaEvaluacionsController(DbContextInstituto context)
        {
            _context = context;
        }

        // GET: MateriaCursadaEvaluacions
        public async Task<IActionResult> Index()
        {
            var dbContextInstituto = _context.MateriaCursadaEvaluaciones.Include(m => m.Evaluacion).Include(m => m.MateriaCursada);
            return View(await dbContextInstituto.ToListAsync());
        }

        // GET: MateriaCursadaEvaluacions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaCursadaEvaluacion = await _context.MateriaCursadaEvaluaciones
                .Include(m => m.Evaluacion)
                .Include(m => m.MateriaCursada)
                .FirstOrDefaultAsync(m => m.MateriaCursadaId == id);
            if (materiaCursadaEvaluacion == null)
            {
                return NotFound();
            }

            return View(materiaCursadaEvaluacion);
        }

        // GET: MateriaCursadaEvaluacions/Create
        public IActionResult Create()
        {
            ViewData["EvaluacionId"] = new SelectList(_context.Evaluaciones, "Id", "Titulo");
            ViewData["MateriaCursadaId"] = new SelectList(_context.MateriaCursadas, "MateriaCursadaId", "Nombre");
            return View();
        }

        // POST: MateriaCursadaEvaluacions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EvaluacionId,MateriaCursadaId,Activo")] MateriaCursadaEvaluacion materiaCursadaEvaluacion)
        {
            var materiaCursada = _context.MateriaCursadas
                .Include(mc => mc.MateriaCursadaEvaluaciones)
                .ThenInclude(mce => mce.Evaluacion)
                .FirstOrDefault(mc => mc.MateriaCursadaId == materiaCursadaEvaluacion.MateriaCursadaId)
                ;
            
            if (ModelState.IsValid)
            {
                materiaCursadaEvaluacion.MateriaCursadaId = Guid.NewGuid();
                if(materiaCursada.MateriaCursadaEvaluaciones
                    .FirstOrDefault(mce => mce.EvaluacionId == materiaCursadaEvaluacion.EvaluacionId) != null)
                {
                    ViewData["EvaluacionId"] = new SelectList(_context.Evaluaciones, "Id", "Titulo", materiaCursadaEvaluacion.EvaluacionId);
                    ViewData["MateriaCursadaId"] = new SelectList(_context.MateriaCursadas, "MateriaCursadaId", "Nombre", materiaCursadaEvaluacion.MateriaCursadaId);
                    ViewData["Mensaje"] = "Este grupo ya tiene esta evaluación asignada";
                    return View(materiaCursadaEvaluacion);
                   
                }
                materiaCursada.MateriaCursadaEvaluaciones.Add(materiaCursadaEvaluacion);
                _context.Add(materiaCursadaEvaluacion);
                _context.Update(materiaCursada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["EvaluacionId"] = new SelectList(_context.Evaluaciones, "Id", "Titulo", materiaCursadaEvaluacion.EvaluacionId);
            ViewData["MateriaCursadaId"] = new SelectList(_context.MateriaCursadas, "MateriaCursadaId", "Nombre", materiaCursadaEvaluacion.MateriaCursadaId);
            return View(materiaCursadaEvaluacion);
        }

        // GET: MateriaCursadaEvaluacions/Edit/5
        public async Task<IActionResult> Edit(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var materiaCursadaEvaluacion =  _context.MateriaCursadaEvaluaciones.FirstOrDefault(mce => mce.Id == Id);
            if (materiaCursadaEvaluacion == null)
            {
                return NotFound();
            }
            ViewData["EvaluacionId"] = new SelectList(_context.Evaluaciones, "Id", "Titulo", materiaCursadaEvaluacion.EvaluacionId);
            ViewData["MateriaCursadaId"] = new SelectList(_context.MateriaCursadas, "MateriaCursadaId", "Nombre", materiaCursadaEvaluacion.MateriaCursadaId);
            return View(materiaCursadaEvaluacion);
        }

        // POST: MateriaCursadaEvaluacions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,EvaluacionId,MateriaCursadaId,Activo")] MateriaCursadaEvaluacion materiaCursadaEvaluacion)
        {
            if (id != materiaCursadaEvaluacion.MateriaCursadaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materiaCursadaEvaluacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MateriaCursadaEvaluacionExists(materiaCursadaEvaluacion.MateriaCursadaId))
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
            ViewData["EvaluacionId"] = new SelectList(_context.Evaluaciones, "Id", "Titulo", materiaCursadaEvaluacion.EvaluacionId);
            ViewData["MateriaCursadaId"] = new SelectList(_context.MateriaCursadas, "MateriaCursadaId", "Nombre", materiaCursadaEvaluacion.MateriaCursadaId);
            return View(materiaCursadaEvaluacion);
        }

        // GET: MateriaCursadaEvaluacions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaCursadaEvaluacion = await _context.MateriaCursadaEvaluaciones
                .Include(m => m.Evaluacion)
                .Include(m => m.MateriaCursada)
                .FirstOrDefaultAsync(m => m.MateriaCursadaId == id);
            if (materiaCursadaEvaluacion == null)
            {
                return NotFound();
            }

            return View(materiaCursadaEvaluacion);
        }

        // POST: MateriaCursadaEvaluacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var materiaCursadaEvaluacion = await _context.MateriaCursadaEvaluaciones.FindAsync(id);
            _context.MateriaCursadaEvaluaciones.Remove(materiaCursadaEvaluacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MateriaCursadaEvaluacionExists(Guid id)
        {
            return _context.MateriaCursadaEvaluaciones.Any(e => e.MateriaCursadaId == id);
        }
    }
}
