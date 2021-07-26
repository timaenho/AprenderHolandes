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
    public class CarrerasController : Controller
    {
        private readonly DbContextInstituto _context;

        public CarrerasController(DbContextInstituto context)
        {
            _context = context;
        }

        // GET: Carreras


        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Carreras.ToListAsync());
        }

        // GET: Carreras/Details/5

        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrera = await _context.Carreras
                .FirstOrDefaultAsync(m => m.CarreraId == id);
            if (carrera == null)
            {
                return NotFound();
            }

            return View(carrera);
        }

        // GET: Carreras/Create
        [Authorize(Roles = "Empleado")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carreras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Empleado"))]
        public async Task<IActionResult> Create([Bind("CarreraId,Nombre")] Carrera carrera)
        {
            if (ModelState.IsValid)
            {
                foreach (var c in _context.Carreras)
                {
                    if (c.Nombre.ToLower() == carrera.Nombre.ToLower())
                    {
                        TempData["Message"] = "Ya existe una carrera con el nombre '" + carrera.Nombre + "'";
                        return View();
                    }
                }
                carrera.CarreraId = Guid.NewGuid();
                carrera.Materias = new List<Materia>();
                _context.Add(carrera);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carrera);
        }

        // GET: Carreras/Edit/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera == null)
            {
                return NotFound();
            }
            return View(carrera);
        }

        // POST: Carreras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Empleado")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CarreraId,Nombre")] Carrera carrera)
        {
            if (id != carrera.CarreraId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carrera);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarreraExists(carrera.CarreraId))
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
            return View(carrera);
        }

        //GET: Carreras/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var carrera = await _context.Carreras
        //        .FirstOrDefaultAsync(m => m.CarreraId == id);
        //    if (carrera == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(carrera);
        //}

        //POST: Carreras/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    var carrera = await _context.Carreras.FindAsync(id);
        //    _context.Carreras.Remove(carrera);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        [Authorize(Roles = "Empleado")]
        private bool CarreraExists(Guid id)
        {
            return _context.Carreras.Any(e => e.CarreraId == id);
        }

        [AllowAnonymous]
        public async Task<IActionResult> MostrarCarreras()
        {
            return View(_context.Carreras);
        }
    }
}
