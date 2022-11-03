using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PresupuestoDemo.Models;
using PresupuestoDemo.Servicios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PresupuestoDemo.Controllers
{
    public class TipoPlantillaController : Controller
    {
        private readonly IRepositorioTipoPlantilla repositorioTipoPlantilla;

        public TipoPlantillaController(IRepositorioTipoPlantilla repositorioTipoPlantilla)
        {
            this.repositorioTipoPlantilla = repositorioTipoPlantilla;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TipoPlantilla tipoPlantilla)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoPlantilla);
            }

            var yaExisteTipoPlantilla = await repositorioTipoPlantilla.ExisteTipoPlantilla(tipoPlantilla.TipoDePlantilla);

            if (yaExisteTipoPlantilla)
            {
                ModelState.AddModelError(nameof(tipoPlantilla.TipoDePlantilla),
                                        $"El tipo de plantilla {tipoPlantilla.TipoDePlantilla} ya existe");
                return View(tipoPlantilla);
            }

            await repositorioTipoPlantilla.Create(tipoPlantilla);

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoPlantilla(string tipoDePlantilla)
        {
            var yaExisteTipoPlantilla = await repositorioTipoPlantilla.ExisteTipoPlantilla(tipoDePlantilla);
            return yaExisteTipoPlantilla ? Json($"El tipo de plantilla {tipoDePlantilla} ya existe") : Json(true);
        }
    }
}
