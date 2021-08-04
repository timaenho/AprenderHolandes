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
        public async Task<IActionResult> Create([Bind("MateriaCursadaId,Nombre,FechaInicio,FechaTermino,Dia,Hora,CantidadHorasPorSemana,Descripcion,Precio,Activo,MateriaId,ProfesorId")] MateriaCursada materiaCursada)
        {
            var materia = _context.Materias.
                Include(m => m.MateriasCursadas).
            
                FirstOrDefault(m => m.MateriaId == materiaCursada.MateriaId);
            var profesor = _context.Profesores
                .Include(p => p.MateriasCursadasActivas)
                .FirstOrDefault(p => p.Id == materiaCursada.ProfesorId);

            if (ModelState.IsValid)
            {

                materiaCursada.MateriaCursadaId = Guid.NewGuid();
                materiaCursada.Nombre = materia.Nombre + " - " + materiaCursada.FechaInicio.ToShortDateString() + " hasta " + materiaCursada.FechaTermino.ToShortDateString() + (" - ") + materiaCursada.Hora;
                if (materiaCursada.Activo)
                {
                    profesor.MateriasCursadasActivas.Add(materiaCursada);
                }
               
                _context.Add(materiaCursada);
                _context.Update(profesor);
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
        public async Task<IActionResult> Edit(Guid id, [Bind("MateriaCursadaId,Nombre,Activo,MateriaId,ProfesorId,FechaInicio,FechaTermino,Dia,Hora,CantidadHorasPorSemana,Descripcion,Precio")] MateriaCursada materiaCursada)
        {
           
   
            
            if (id != materiaCursada.MateriaCursadaId)
            {
                return NotFound();
            }
            var materia = _context.Materias.
            Include(m => m.MateriasCursadas).
            FirstOrDefault(m => m.MateriaId == materiaCursada.MateriaId);
            var profesor = _context.Profesores
                .Include(p => p.MateriasCursadasActivas)
                .FirstOrDefault(p => p.Id == materiaCursada.ProfesorId);

            materiaCursada.Nombre = materia.Nombre + " - " + materiaCursada.FechaInicio.ToShortDateString() + " hasta " + materiaCursada.FechaTermino.ToShortDateString() + (" - ") + materiaCursada.Hora;
            if (materiaCursada.Activo)
            {
             
                if (profesor.MateriasCursadasActivas.FirstOrDefault(mc => mc.MateriaCursadaId == materiaCursada.MateriaCursadaId)==null)
                {
                    profesor.MateriasCursadasActivas.Add(materiaCursada);
                }
              
            }
            else
            {
                var mcEncontrado = profesor.MateriasCursadasActivas.FirstOrDefault(mc => mc.MateriaCursadaId == materiaCursada.MateriaCursadaId);
                if (mcEncontrado != null)
                {
                    profesor.MateriasCursadasActivas.Remove(materiaCursada);
                }

            }
            if (ModelState.IsValid)
            {
               
                try
                {
                    var materiaCursadaDB = _context.MateriaCursadas.First(mc => mc.MateriaCursadaId == materiaCursada.MateriaCursadaId);
                    _context.Entry(materiaCursadaDB).CurrentValues.SetValues(materiaCursada);
                    var profesorDB = _context.Profesores.First(p => p.Id == profesor.Id);
                    _context.Entry(profesorDB).CurrentValues.SetValues(profesor);

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
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Discriminator", materiaCursada.ProfesorId);
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
