using AprenderHolandes.Data;
using AprenderHolandes.Models;
using AprenderHolandes.Repository;
using AprenderHolandes.Servicios;
using AprenderHolandes.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {

        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly DbContextInstituto _miContexto;
        private readonly RoleManager<Rol> _roleManager;
        private readonly IAccountRepository _accountRepository;


        public AccountsController(
            UserManager<Persona> userManager,
            SignInManager<Persona> signInManager,
            DbContextInstituto miContexto,
            RoleManager<Rol> roleManager,
            IAccountRepository accountRepository


            )
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._miContexto = miContexto;
            this._roleManager = roleManager;
            this._accountRepository = accountRepository;
        
        }
        [HttpGet, AllowAnonymous]
        public IActionResult Registrar()
        {
            ViewData["CarreraId"] = new SelectList(_miContexto.Carreras, "CarreraId", "Nombre");
            return View();
        }
        [HttpGet, AllowAnonymous]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult>ChangePassword(CambiarContrasenia model)
        {
            if(ModelState.IsValid){
                var result = await ChangePasswordAsync(model);
                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                    return View();

                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Registrar(RegistroUsuario modelo)
        {
            if (ModelState.IsValid)
            {
                var Alumnos = _miContexto.Alumnos;
                var MatriculaMax = 0;

                foreach (Alumno alumno in Alumnos)
                {

                    if (alumno.NumeroMatricula != 0)
                    {
                        var MatriculaAlumno = alumno.NumeroMatricula;
                        if (MatriculaAlumno > MatriculaMax)
                        {
                            MatriculaMax = MatriculaAlumno;
                        }
                    }
                }

                MatriculaMax = MatriculaMax + 1;

                Persona persona = new Alumno()
                {
                    Id = new Guid(),
                    Nombre = modelo.Nombre,
                    UserName = modelo.Email,
                    Email = modelo.Email,
                    CarreraId = modelo.CarreraId,
                    Apellido = modelo.Apellido,
                    FechaAlta = DateTime.Now,
                    Activo = false,
                    NumeroMatricula = MatriculaMax,
                    AlumnoMateriaCursadaEvaluaciondaNotas = new List<AlumnoMateriaCursadaEvaluaciondaNota>()
                };

                var resultadoRegistracion = await _userManager.CreateAsync(persona, modelo.Contrasena);

                if (resultadoRegistracion.Succeeded)
                {
                    Rol rolAlumno = null;
                    var Name = "Alumno";
                    rolAlumno = await _roleManager.FindByNameAsync(Name);

                    if (rolAlumno == null)
                    {
                        rolAlumno = new Rol();
                        rolAlumno.Name = Name;
                        var resuNewRol = await _roleManager.CreateAsync(rolAlumno);
                    }

                    var resuAddToRole = await _userManager.AddToRoleAsync(persona, Name);
                    await _signInManager.SignInAsync(persona, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }


                foreach (var error in resultadoRegistracion.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            ViewData["CarreraId"] = new SelectList(_miContexto.Carreras, "CarreraId", "Nombre");
            return View(modelo);
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> EmailLibre(string email)
        {
            var usuarioExistente = await _userManager.FindByEmailAsync(email);


            if (usuarioExistente == null)
            {

                return Json(true);
            }
            else
            {

                return Json($"El correo {email} ya está en uso.");
            }

        }


        [HttpGet, AllowAnonymous]
        public IActionResult IniciarSesion(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            if (returnUrl != null)
            {
                ViewBag.Mensaje = "Para acceder al recurso " + returnUrl + ", primero debe Iniciar sesión";

            }
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> IniciarSesion(Login modelo)
        {
            string returnUrl = TempData["returnUrl"] as string;

            if (ModelState.IsValid)
            {
                var resultadoInicioSesion = await _signInManager.PasswordSignInAsync(modelo.Email, modelo.Password, modelo.Recordarme, false);

                if (resultadoInicioSesion.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Inicio de sesión inválido.");
            }
            return View(modelo);
        }


        public async Task<IActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous, HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [AllowAnonymous, HttpPost("forgot-password")]
        public async Task <IActionResult> ForgotPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountRepository.GetUserByEmailAsync(model.Email);
                if(user!= null)
                {
                    await _accountRepository.GenerateForgotPasswordTokenAsync(user);
                }
                ModelState.Clear();
                model.Emailsent = true;
            }

            return View(model);
        }

        //public async Task<IActionResult> CrearRoles()
        //{
        //    if (!_roleManager.Roles.Any())
        //    {
        //        Rol rol = new Rol();
        //        rol.Name = "Admin";


        //        var resu1 = await _roleManager.CreateAsync(rol);

        //    };

        //    return RedirectToAction("Roles");
        //}




        //public async Task<IActionResult> CrearAdministrador()
        //{
        //    var name = "Admin";
        //    var email = "admin@admin.com";
        //    Persona admin = new Persona()
        //    {
        //        Nombre = name,
        //        Apellido = name,
        //        UserName = email,
        //        Email = email
        //    };

        //    var resuNewAdmin = await _userManager.CreateAsync(admin, "Password1!");

        //    if (resuNewAdmin.Succeeded)
        //    {
        //        Rol rolAdmin = await _roleManager.FindByNameAsync(name);

        //        if (rolAdmin == null)
        //        {
        //            rolAdmin = new Rol();
        //            rolAdmin.Name = name;
        //            var resuNewRol = await _roleManager.CreateAsync(rolAdmin);
        //        }

        //        var resuAddToRole = await _userManager.AddToRoleAsync(admin, name);
        //    }

        //    return RedirectToAction("Roles");
        //}

        [HttpGet,AllowAnonymous]
        public IActionResult AccesoDenegado(string returnurl)
        {

            return View(model: returnurl);
        }

        private async Task <IdentityResult> ChangePasswordAsync (CambiarContrasenia model)
        {
            Persona persona = await _userManager.GetUserAsync(HttpContext.User);
            return await _userManager.ChangePasswordAsync(persona, model.CurrentPassword, model.ConfirmarNuevaContrasenia);
        }

        [AllowAnonymous, HttpGet("reset-password")]
        public IActionResult ResetPassword(string uid, string token)
        {
            ResetPassword resetPasswordModel = new ResetPassword
            {
                Token = token,
                UserId = uid
            };
            return View(resetPasswordModel);
        }

        [AllowAnonymous, HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                model.Token = model.Token.Replace(' ', '+');
                var result = await _accountRepository.ResetPasswordAsync(model);
                if (result.Succeeded)
                {
                    ModelState.Clear();
                    model.IsSuccess = true;
                    return View(model);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }




    }
}
