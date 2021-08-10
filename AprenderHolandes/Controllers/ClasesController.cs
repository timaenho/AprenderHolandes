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
    public class ClasesController : Controller
    {
        private readonly DbContextInstituto _context;
        private readonly UserManager<Persona> _userManager;

        public ClasesController(DbContextInstituto context, UserManager<Persona> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }

        public async Task<IActionResult> IndexAlumno()
        {
            Alumno al = (Alumno)await _userManager.GetUserAsync(HttpContext.User);
            if(al == null)
            {
                return NotFound();
            }
            var clases = new List<Clase>();
            var alumno = _context.Alumnos
                .Include(a=> a.AlumnosMateriasCursadas)
                .ThenInclude(amc => amc.MateriaCursada)
                .ThenInclude(mc => mc.Clases)
                .FirstOrDefault(a => a.Id == al.Id);

            foreach(AlumnoMateriaCursada amc in alumno.AlumnosMateriasCursadas)
            {
                clases.AddRange(amc.MateriaCursada.Clases);
            }

            return View(clases);
        } 
        public async Task<IActionResult> Index()
        {

            Persona profesor =(Profesor) await _userManager.GetUserAsync(HttpContext.User);
                var dbContextInstituto = _context.Clases.Include(c => c.MateriaCursada)
               .Where(c => c.ProfesorId == profesor.Id && c.Fecha > DateTime.Now)
               .Include(c => c.Profesor).OrderBy(c =>c.Fecha);
            return View(await dbContextInstituto.ToListAsync());
        }

        // GET: Clases/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clase = await _context.Clases
                .Include(c => c.MateriaCursada)
                .Include(c => c.Profesor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clase == null)
            {
                return NotFound();
            }

            return View(clase);
        }

        // GET: Clases/Create
        public IActionResult Create()
        {
            ViewData["MateriaCursadaId"] = new SelectList(_context.MateriaCursadas, "MateriaCursadaId", "Nombre");
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Nombre");
            return View();
        }

        // POST: Clases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MateriaCursadaId,Fecha,Descripcion,Link,ProfesorId")] Clase clase)
        {
            if (ModelState.IsValid)
            {
                clase.Id = Guid.NewGuid();
                _context.Add(clase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MateriaCursadaId"] = new SelectList(_context.MateriaCursadas, "MateriaCursadaId", "Nombre", clase.MateriaCursadaId);
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Discriminator", clase.ProfesorId);
            return View(clase);
        }

        // GET: Clases/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clase = await _context.Clases.FindAsync(id);
            if (clase == null)
            {
                return NotFound();
            }
            ViewData["MateriaCursadaId"] = new SelectList(_context.MateriaCursadas, "MateriaCursadaId", "Nombre", clase.MateriaCursadaId);
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Discriminator", clase.ProfesorId);
            return View(clase);
        }

        // POST: Clases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,MateriaCursadaId,Fecha,Descripcion,Link,ProfesorId")] Clase clase)
        {
            if (id != clase.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClaseExists(clase.Id))
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
            ViewData["MateriaCursadaId"] = new SelectList(_context.MateriaCursadas, "MateriaCursadaId", "Nombre", clase.MateriaCursadaId);
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Discriminator", clase.ProfesorId);
            return View(clase);
        }

        // GET: Clases/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clase = await _context.Clases
                .Include(c => c.MateriaCursada)
                .Include(c => c.Profesor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clase == null)
            {
                return NotFound();
            }

            return View(clase);
        }

        // POST: Clases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var clase = await _context.Clases.FindAsync(id);
            _context.Clases.Remove(clase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClaseExists(Guid id)
        {
            return _context.Clases.Any(e => e.Id == id);
        }
    }
}
