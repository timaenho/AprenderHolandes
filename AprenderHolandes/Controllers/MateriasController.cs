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
    public class MateriasController : Controller
    {
        private readonly DbContextInstituto _context;

        public MateriasController(DbContextInstituto context)
        {
            _context = context;
        }

        // GET: Materias
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Index()
        {
            var dbContextInstituto = _context.Materias.Include(m => m.Carrera);
            return View(await dbContextInstituto.ToListAsync());
        }

        // GET: Materias/Details/5

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materias
                .Include(m => m.Carrera)
                .FirstOrDefaultAsync(m => m.MateriaId == id);
            if (materia == null)
            {
                return NotFound();
            }

            return View(materia);
        }

        // GET: Materias/Create
        [Authorize(Roles = "Empleado")]
        public IActionResult Create()
        {
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "CarreraId", "Nombre");
            return View();
        }

        // POST: Materias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Empleado"))]
        public async Task<IActionResult> Create([Bind("MateriaId,Nombre,CodigoMateria,Descripcion,CupoMaximo,CarreraId")] Materia materia)
        {
            if (ModelState.IsValid)
            {
                foreach (var m in _context.Materias)
                {
                    if (m.Nombre.ToLower() == materia.Nombre.ToLower())
                    {
                        TempData["Message"] = "Ya existe una materia con el nombre '" + materia.Nombre + "'";
                        ViewData["CarreraId"] = new SelectList(_context.Carreras, "CarreraId", "Nombre");
                        return View();
                    }
                    if (m.CodigoMateria.ToLower() == materia.CodigoMateria.ToLower())
                    {
                        TempData["Message"] = "Ya existe una materia con el codigo '" + materia.CodigoMateria + "'";
                        ViewData["CarreraId"] = new SelectList(_context.Carreras, "CarreraId", "Nombre");
                        return View();
                    }
                }

                materia.MateriaId = Guid.NewGuid();
                var carrera = await _context.Carreras.FindAsync(materia.CarreraId);
                _context.Add(materia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "CarreraId", "Nombre", materia.CarreraId);
            return View(materia);
        }

        // GET: Materias/Edit/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materias.FindAsync(id);
            if (materia == null)
            {
                return NotFound();
            }
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "CarreraId", "Nombre", materia.CarreraId);
            return View(materia);
        }

        // POST: Materias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(Guid id, [Bind("MateriaId,Nombre,CodigoMateria,Descripcion,CupoMaximo,CarreraId")] Materia materia)
        {
            if (id != materia.MateriaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MateriaExists(materia.MateriaId))
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
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "CarreraId", "Nombre", materia.CarreraId);
            return View(materia);
        }

        // GET: Materias/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var materia = await _context.Materias
        //        .Include(m => m.Carrera)
        //        .FirstOrDefaultAsync(m => m.MateriaId == id);
        //    if (materia == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(materia);
        //}

        // POST: Materias/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    var materia = await _context.Materias.FindAsync(id);
        //    _context.Materias.Remove(materia);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool MateriaExists(Guid id)
        {
            return _context.Materias.Any(e => e.MateriaId == id);
        }
        [AllowAnonymous]
        public async Task<IActionResult> MostrarMateriasSegunCarrera(Guid ID)
        {
            //Guid cId = (Guid)ViewData["ID"];
            var carrera = _context.Carreras.Include(c => c.Materias).FirstOrDefault(c => c.CarreraId == ID);

            return View(carrera.Materias);
        }
    }


}
