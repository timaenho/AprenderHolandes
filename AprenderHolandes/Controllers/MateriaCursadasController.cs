using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AprenderHolandes.Data;
using AprenderHolandes.Models;
using Microsoft.AspNetCore.Authorization;

namespace AprenderHolandes.Controllers
{
    [Authorize]
    public class MateriaCursadasController : Controller
    {
        private readonly DbContextInstituto _context;

        public MateriaCursadasController(DbContextInstituto context)
        {
            _context = context;
        }

        // GET: MateriaCursadas
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Index()
        {
            var dbContextInstituto = _context.MateriaCursadas.Include(m => m.Materia).Include(m => m.Profesor);
            return View(await dbContextInstituto.ToListAsync());
        }

        // GET: MateriaCursadas/Details/5


        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaCursada = await _context.MateriaCursadas
                .Include(m => m.Materia)
                .Include(m => m.Profesor)
                .Include(m => m.AlumnoMateriaCursadas)
                .ThenInclude(amc => amc.Alumno)
                .FirstOrDefaultAsync(m => m.MateriaCursadaId == id);
            if (materiaCursada == null)
            {
                return NotFound();
            }

            return View(materiaCursada);
        }

        // GET: MateriaCursadas/Create
        [Authorize(Roles = "Empleado")]
        public IActionResult Create()
        {
            ViewData["MateriaId"] = new SelectList(_context.Materias, "MateriaId", "Nombre");
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Apellido");
            return View();
        }

        // POST: MateriaCursadas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Create([Bind("MateriaCursadaId,Nombre,Anio,Cuatrimestre,Activo,MateriaId,ProfesorId")] MateriaCursada materiaCursada)
        {
            var materia = _context.Materias.
                Include(m => m.MateriasCursadas).
                FirstOrDefault(m => m.MateriaId == materiaCursada.MateriaId);

            if (ModelState.IsValid)
            {

                materiaCursada.MateriaCursadaId = Guid.NewGuid();
                materiaCursada.Nombre = materia.Nombre + " - " + (materia.MateriasCursadas.Count + 1) + ", " + DateTime.Now.Year + "- cuatrimestre " + materiaCursada.Cuatrimestre;
                _context.Add(materiaCursada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MateriaId"] = new SelectList(_context.Materias, "MateriaId", "Nombre", materiaCursada.MateriaId);
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Apellido", materiaCursada.ProfesorId);
            return View(materiaCursada);
        }

        // GET: MateriaCursadas/Edit/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaCursada = await _context.MateriaCursadas.FindAsync(id);
            if (materiaCursada == null)
            {
                return NotFound();
            }
            ViewData["MateriaId"] = new SelectList(_context.Materias, "MateriaId", "Nombre", materiaCursada.MateriaId);
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Apellido", materiaCursada.ProfesorId);
            return View(materiaCursada);
        }

        // POST: MateriaCursadas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(Guid id, [Bind("MateriaCursadaId,Nombre,Anio,Cuatrimestre,Activo,MateriaId,ProfesorId")] MateriaCursada materiaCursada)
        {
            if (id != materiaCursada.MateriaCursadaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materiaCursada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MateriaCursadaExists(materiaCursada.MateriaCursadaId))
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
            ViewData["MateriaId"] = new SelectList(_context.Materias, "MateriaId", "CodigoMateria", materiaCursada.MateriaId);
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Apellido", materiaCursada.ProfesorId);
            return View(materiaCursada);
        }

        // GET: MateriaCursadas/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var materiaCursada = await _context.MateriaCursadas
        //        .Include(m => m.Materia)
        //        .Include(m => m.Profesor)
        //        .FirstOrDefaultAsync(m => m.MateriaCursadaId == id);
        //    if (materiaCursada == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(materiaCursada);
        //}

        // POST: MateriaCursadas/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    var materiaCursada = await _context.MateriaCursadas.FindAsync(id);
        //    _context.MateriaCursadas.Remove(materiaCursada);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        [Authorize(Roles = "Empleado")]
        private bool MateriaCursadaExists(Guid id)
        {
            return _context.MateriaCursadas.Any(e => e.MateriaCursadaId == id);
        }

        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> ActivarMateriaCursada(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matCurs = await _context.MateriaCursadas.FindAsync(id);

            if (matCurs == null)
            {
                return NotFound();
            }

            if (matCurs.Activo == false)
            {
                matCurs.Activo = true;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
