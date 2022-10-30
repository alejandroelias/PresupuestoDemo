using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PresupuestoDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PresupuestoDemo.Controllers
{
    public class TipoPlantillaController : Controller
    {
        private readonly string connectionString;
        public TipoPlantillaController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = connection.Query("select 1").FirstOrDefault();
            }

            return View();
        }
        [HttpPost]
        public IActionResult Create(TipoPlantilla tipoPlantilla)
        {
            if (!ModelState.IsValid)
            {
                return View (tipoPlantilla);
            }
            return View();
        }
    }
}
