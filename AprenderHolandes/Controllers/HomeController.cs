using AprenderHolandes.Data;
using AprenderHolandes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AprenderHolandes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbContextInstituto _micontexto;

        public HomeController(ILogger<HomeController> logger, DbContextInstituto micontexto)
        {
            _logger = logger;
            _micontexto = micontexto;
        }

        public IActionResult Index()
        {
            var materiasCursadas = _micontexto.MateriaCursadas
                .Include(mc => mc.AlumnoMateriaCursadas)
                .Include(mc => mc.Materia)
                .Where(mc => mc.Activo);

            return View(materiasCursadas);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
