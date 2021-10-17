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
    public class ForoController : Controller
    {
        private readonly DbContextInstituto _context;
        private readonly UserManager<Persona> _usermanager;

        public ForoController(DbContextInstituto context, UserManager<Persona> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

        // GET: Foro
        public async Task<IActionResult> IndexAlumno()
        {
            var Alumno = (Alumno) await _usermanager
                .GetUserAsync(HttpContext.User);
            var _Alumno = _context.Alumnos
                .Include(a => a.AlumnosMateriasCursadas)
                .ThenInclude(amc => amc.MateriaCursada)
                .FirstOrDefault(a => a.Id == Alumno.Id);

            var materiaCursada = Alumno.AlumnosMateriasCursadas
                .FirstOrDefault(amc => amc.MateriaCursada.Activo);

            var materiaCursadaId = materiaCursada.MateriaCursadaId;

            var preguntas = _context.Preguntas
                .Include(m => m.MateriaCursada)
                .Include(m => m.Persona)
                .Where(p => p.MateriaCursadaId == materiaCursadaId);

            return View(await preguntas.ToListAsync());
        }

        // GET: Foro/Details/5

        //public async Task<IActionResult> IndexProfesor()
        //{
  
  
        //}
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mensaje = await _context.Mensajes
                .Include(m => m.MateriaCursada)
                .Include(m => m.Persona)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mensaje == null)
            {
                return NotFound();
            }

            return View(mensaje);
        }

        // GET: Foro/Create
        public IActionResult Create()
        {
            ViewData["MateriaCursadaId"] = new SelectList(_context.MateriaCursadas, "MateriaCursadaId", "Descripcion");
            ViewData["PersonaId"] = new SelectList(_context.Personas, "Id", "Discriminator");
            return View();
        }

        // POST: Foro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PersonaId,MateriaCursadaId,Contenido,Fecha")] Mensaje mensaje)
        {
            if (ModelState.IsValid)
            {
                mensaje.Id = Guid.NewGuid();
                _context.Add(mensaje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MateriaCursadaId"] = new SelectList(_context.MateriaCursadas, "MateriaCursadaId", "Descripcion", mensaje.MateriaCursadaId);
            ViewData["PersonaId"] = new SelectList(_context.Personas, "Id", "Discriminator", mensaje.PersonaId);
            return View(mensaje);
        }

        // GET: Foro/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mensaje = await _context.Mensajes.FindAsync(id);
            if (mensaje == null)
            {
                return NotFound();
            }
            ViewData["MateriaCursadaId"] = new SelectList(_context.MateriaCursadas, "MateriaCursadaId", "Descripcion", mensaje.MateriaCursadaId);
            ViewData["PersonaId"] = new SelectList(_context.Personas, "Id", "Discriminator", mensaje.PersonaId);
            return View(mensaje);
        }

        // POST: Foro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,PersonaId,MateriaCursadaId,Contenido,Fecha")] Mensaje mensaje)
        {
            if (id != mensaje.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mensaje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MensajeExists(mensaje.Id))
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
            ViewData["MateriaCursadaId"] = new SelectList(_context.MateriaCursadas, "MateriaCursadaId", "Descripcion", mensaje.MateriaCursadaId);
            ViewData["PersonaId"] = new SelectList(_context.Personas, "Id", "Discriminator", mensaje.PersonaId);
            return View(mensaje);
        }

        // GET: Foro/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mensaje = await _context.Mensajes
                .Include(m => m.MateriaCursada)
                .Include(m => m.Persona)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mensaje == null)
            {
                return NotFound();
            }

            return View(mensaje);
        }

        // POST: Foro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var mensaje = await _context.Mensajes.FindAsync(id);
            _context.Mensajes.Remove(mensaje);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MensajeExists(Guid id)
        {
            return _context.Mensajes.Any(e => e.Id == id);
        }
    }
}
