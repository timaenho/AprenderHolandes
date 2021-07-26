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
using Microsoft.AspNetCore.Identity;

namespace AprenderHolandes.Controllers
{
   
        [Authorize]
        public class CalificacionesController : Controller
        {
            private readonly DbContextInstituto _context;
            private readonly UserManager<Persona> _userManager;

            public CalificacionesController(DbContextInstituto context, UserManager<Persona> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            // GET: Calificaciones
            [Authorize(Roles = "Profesor")]
            public async Task<IActionResult> Index()
            {
                Profesor profesor = (Profesor)await _userManager.GetUserAsync(HttpContext.User);

                var calificaciones = _context.Calificaciones
                    .Include(c => c.AlumnoMateriaCursada)
                    .ThenInclude(amc => amc.Alumno)
                    .Include(c => c.Materia)
                    .Where(c => c.ProfesorId == profesor.Id && c.NotaFinal != -1111).OrderBy(c => c.Materia.MateriaId).ThenBy(c => c.AlumnoMateriaCursada.MateriaCursadaId);

                return View(calificaciones);
            }

            [Authorize(Roles = "Alumno")]
            // GET: Calificaciones/Details/5
            public async Task<IActionResult> Details(Guid? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var calificacion = await _context.Calificaciones
                    .Include(c => c.Profesor)
                    .FirstOrDefaultAsync(m => m.CalificacionId == id);
                if (calificacion == null)
                {
                    return NotFound();
                }

                return View(calificacion);
            }

            //GET: Calificaciones/Create
            //public IActionResult Create()
            //{
            //    ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Apellido");
            //    return View();
            //}

            // POST: Calificaciones/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            //[HttpPost]
            //[ValidateAntiForgeryToken]
            //public async Task<IActionResult> Create([Bind("CalificacionId,NotaFinal,ProfesorId")] Calificacion calificacion)
            //{
            //    if (ModelState.IsValid)
            //    {
            //        calificacion.CalificacionId = Guid.NewGuid();
            //        _context.Add(calificacion);
            //        await _context.SaveChangesAsync();
            //        return RedirectToAction(nameof(Index));
            //    }
            //    ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Apellido", calificacion.ProfesorId);
            //    return View(calificacion);
            //}
            //authorize
            // GET: Calificaciones/Edit/5
            [Authorize(Roles = "Profesor")]
            public IActionResult Edit(Guid? id) //validaciones
            {
                if (id == null)
                {
                    return NotFound();
                }
                Calificacion calificacion = _context.Calificaciones
                    .Include(c => c.AlumnoMateriaCursada)
                    .ThenInclude(amc => amc.MateriaCursada)
                    .Include(c => c.AlumnoMateriaCursada)
                    .ThenInclude(amc => amc.Alumno)
                    .FirstOrDefault(c => c.CalificacionId == id);

                return View(calificacion);
            }

            // POST: Calificaciones/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            [Authorize(Roles = "Profesor")]
            public async Task<IActionResult> Edit(Guid? id, int NotaFinal)
            {

                Profesor profesor = (Profesor)await _userManager.GetUserAsync(HttpContext.User);
                if (id == null)
                {
                    return NotFound();
                }

                if (NotaFinal > 10 || NotaFinal < 0)
                {
                    TempData["Message"] = "Entra un valor entre 0 y 10";
                    return RedirectToAction("Edit");
                }


                Calificacion c = _context.Calificaciones
                    .Include(c => c.AlumnoMateriaCursada)
                    .ThenInclude(amc => amc.MateriaCursada)
                    .Include(c => c.AlumnoMateriaCursada)
                    .ThenInclude(amc => amc.Alumno)
                    .FirstOrDefault(c => c.CalificacionId == id);

                if (c.MateriaCursada.ProfesorId != profesor.Id)
                {
                    TempData["Message"] = "La calificacion solo la puede otorgar el profesor titular.";
                    return RedirectToAction("Edit");
                }

                c.NotaFinal = NotaFinal;
                c.ProfesorId = profesor.Id;
                c.Profesor = profesor;
                //profesor.CalificacionesRealizadas.Add(c);
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Calificaciones.Update(c);
                        _context.Profesores.Update(profesor);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CalificacionExists(c.CalificacionId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    var Id = c.MateriaCursada.MateriaCursadaId;
                    return RedirectToAction("Index");
                }

                return View(c);
            }

            //GET: Calificaciones/Delete/5
            //public async Task<IActionResult> Delete(Guid? id)
            //{
            //    if (id == null)
            //    {
            //        return NotFound();
            //    }

            //    var calificacion = await _context.Calificaciones
            //        .Include(c => c.Profesor)
            //        .FirstOrDefaultAsync(m => m.CalificacionId == id);
            //    if (calificacion == null)
            //    {
            //        return NotFound();
            //    }

            //    return View(calificacion);
            //}

            //POST: Calificaciones/Delete/5
            //[HttpPost, ActionName("Delete")]
            //[ValidateAntiForgeryToken]
            //public async Task<IActionResult> DeleteConfirmed(Guid id)
            //{
            //    var calificacion = await _context.Calificaciones.FindAsync(id);
            //    _context.Calificaciones.Remove(calificacion);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}

            private bool CalificacionExists(Guid id)
            {
                return _context.Calificaciones.Any(e => e.CalificacionId == id);
            }
        }
    }
