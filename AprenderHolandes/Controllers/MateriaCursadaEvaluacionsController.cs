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
    public class MateriaCursadaEvaluacionsController : Controller
    {
        private readonly DbContextInstituto _context;
        private readonly UserManager<Persona> _usermanager;


        public MateriaCursadaEvaluacionsController(DbContextInstituto context, UserManager<Persona> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

        // GET: MateriaCursadaEvaluacions
        public async Task<IActionResult> Index()
        {
            var dbContextInstituto = _context.MateriaCursadaEvaluaciones.Include(m => m.Evaluacion).Include(m => m.MateriaCursada)
                .OrderBy(mce => mce.MateriaCursadaId);
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

        //GET: MateriaCursadaEvaluacions/Edit/5
        public IActionResult Edit(Guid? Id)
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

        //POST: MateriaCursadaEvaluacions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,EvaluacionId,MateriaCursadaId,Activo")] MateriaCursadaEvaluacion materiaCursadaEvaluacion)
        {
            if (id != materiaCursadaEvaluacion.Id)
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
                    if (!MateriaCursadaEvaluacionExists(materiaCursadaEvaluacion.Id))
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
        [HttpGet]
        public async Task<IActionResult> ListaMateriaCursadas()
        {
            Profesor profesor = (Profesor)await _usermanager.GetUserAsync(HttpContext.User);
            var listaMateriaCursadas = _context.MateriaCursadas
                .Include(mc => mc.MateriaCursadaEvaluaciones)
                .ThenInclude(mce => mce.Evaluacion)
                .ThenInclude(e => e.Materia)
                .Include(mc => mc.Materia)
                .Where(mc => mc.ProfesorId == profesor.Id);
            return View(listaMateriaCursadas);
        }

        public async Task <IActionResult> ListaEvaluacionesPorMateriaCursada (Guid? Id)
        {
            var materiaCursada = _context.MateriaCursadas
                .Include(mc => mc.Materia)
                .ThenInclude(m => m.Evaluaciones)
                .FirstOrDefault(mc => mc.MateriaCursadaId == Id);

            ViewData["Titulo"] = materiaCursada.Nombre;
            TempData["Id"] = materiaCursada.MateriaCursadaId;
            return View(materiaCursada.Materia.Evaluaciones);
        }

        public async Task<IActionResult> AsignarUnoPorUno(Guid? Id)
        {
            var materiaCursadaId = (Guid)TempData["Id"];
            var materiaCursada = _context.MateriaCursadas.FirstOrDefault(mc => mc.MateriaCursadaId == materiaCursadaId);
            var evaluacion = _context.Evaluaciones.FirstOrDefault(e => e.Id == Id);

            // completar
            return View();
        }

        //public async Task <IActionResult> AddEvaluaciones (Guid? Id)
        //{

        //       var materiaCursada = _context.MateriaCursadas
        //        .Include(mc => mc.Materia)
        //        .ThenInclude(m => m.Evaluaciones)
        //        .Include(mc => mc.MateriaCursadaEvaluaciones)
        //        .FirstOrDefault(mc => mc.MateriaCursadaId == Id);

        //    var evaluaciones = _context.Evaluaciones.Where(e => e.MateriaId == materiaCursada.MateriaId);

        //    if(materiaCursada.MateriaCursadaEvaluaciones != null)
        //    {
        //        ViewData["Mensaje"] = "Las evaluaciones ya estan cargadas in este curso";
        //        return View("ListaMateriaCursadas");
        //    }
        //    else
        //    {
        //        foreach(Evaluacion e in evaluaciones)
        //        {
        //            materiaCursada.MateriaCursadaEvaluaciones.Add(new MateriaCursadaEvaluacion
        //            {
        //                MateriaCursada = materiaCursada,
        //                MateriaCursadaId = materiaCursada.MateriaCursadaId,
        //                Activo = false,
        //                Evaluacion = e,
        //                EvaluacionId= e.Id,
        //                Id= new Guid()
        //            });
        //        }
        //        _context.MateriaCursadas.Update(materiaCursada);
        //        _context.SaveChanges();
        //        ViewData["Mensaje"] = "Agregaste las materias con exito";
        //        return View("ListaMateriaCursada");
        //    }




    }






    

   
}
