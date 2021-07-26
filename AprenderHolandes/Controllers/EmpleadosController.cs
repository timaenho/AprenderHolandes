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
using Microsoft.AspNetCore.Authorization;

namespace AprenderHolandes.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        private readonly DbContextInstituto _context;
        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly RoleManager<Rol> _rolManager;

        public EmpleadosController(DbContextInstituto context, UserManager<Persona> userManager, SignInManager<Persona> signInManager, RoleManager<Rol> rolManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _rolManager = rolManager;
        }

        // GET: Empleados
        [Authorize(Roles = "Empleado")]
        public IActionResult Index()
        {
            var Empleados = _context.Empleados;
            List<Empleado> EmpleadosSinProfes = new List<Empleado>();

            foreach (Empleado e in Empleados)
            {
                if (!(e is Profesor))
                {
                    EmpleadosSinProfes.Add(e);
                }
            }

            return View(EmpleadosSinProfes);
        }

        // GET: Empleados/Details/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleados/Create
        [Authorize(Roles = "Empleado")]
        public IActionResult Create()
        {
            TempData["Message"] = "Se otorga una contraseña por sistema la cual es 'Password1'.";

            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Create([Bind("FechaAlta,Nombre,Apellido,Dni,Telefono,Direccion,Legajo,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                var contrasenia = "Password1";
                var Legajo = "empl - " + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Second + _context.Empleados.Count();
                empleado.Legajo = Legajo;
                empleado.Id = Guid.NewGuid();
                empleado.FechaAlta = DateTime.Today;
                empleado.UserName = empleado.Email;
                empleado.PasswordHash = contrasenia;

                var resultado = await _userManager.CreateAsync(empleado, empleado.PasswordHash);
                if (resultado.Succeeded)
                {
                    Rol rolEmpleado = null;
                    var name = "Empleado";
                    rolEmpleado = await _rolManager.FindByNameAsync(name);

                    if (rolEmpleado == null)
                    {
                        rolEmpleado = new Rol();
                        rolEmpleado.Name = name;
                        var resultNewRol = await _rolManager.CreateAsync(rolEmpleado);
                    }

                    var resultAddToRol = await _userManager.AddToRoleAsync(empleado, name);
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(empleado);
        }

        // GET: Empleados/Edit/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(Guid id, [Bind("FechaAlta,Nombre,Apellido,Dni,Telefono,Direccion,Legajo,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.Id))
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
            return View(empleado);
        }

        // GET: Empleados/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var empleado = await _context.Empleados
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (empleado == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(empleado);
        //}

        // POST: Empleados/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    var empleado = await _context.Empleados.FindAsync(id);
        //    _context.Empleados.Remove(empleado);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        [Authorize(Roles = "Empleado")]
        private bool EmpleadoExists(Guid id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }
    }
}
