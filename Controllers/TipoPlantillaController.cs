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
        private readonly IServicioUsuarios servicioUsuarios;

        public TipoPlantillaController(
            IRepositorioTipoPlantilla repositorioTipoPlantilla,
            IServicioUsuarios servicioUsuarios)
        {
            this.repositorioTipoPlantilla = repositorioTipoPlantilla;
            this.servicioUsuarios = servicioUsuarios;
        }

        public async Task<IActionResult> Index()
        {
            //tipoPlantilla.UsuarioLog = servicioUsuarios.GetUserLog();
            var tipoPlantilla = await repositorioTipoPlantilla.Get();
            return View(tipoPlantilla);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TipoPlantillaModel tipoPlantilla)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoPlantilla);
            }

            //@UsuarioLog, se lee de manera dinamica
            tipoPlantilla.UsuarioLog = servicioUsuarios.GetUserLog();

            var yaExisteTipoPlantilla = await repositorioTipoPlantilla.ExisteTipoPlantilla(tipoPlantilla.TipoPlantilla);

            if (yaExisteTipoPlantilla)
            {
                ModelState.AddModelError(nameof(tipoPlantilla.TipoPlantilla),
                                        $"El tipo de plantilla {tipoPlantilla.TipoPlantilla} ya existe");
                return View(tipoPlantilla);
            }

            await repositorioTipoPlantilla.Create(tipoPlantilla);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Update (int id)
        {
            string usuarioLog = servicioUsuarios.GetUserLog();
            var tipoPlantilla = await repositorioTipoPlantilla.GetByID(id, usuarioLog);

            if (tipoPlantilla is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoPlantilla);
        }

        [HttpPost]
        public async Task<ActionResult> Update (TipoPlantillaModel tipoPlantillaModel)
        {
            string usuarioLog = servicioUsuarios.GetUserLog();
            var tipoCuentaExiste = await repositorioTipoPlantilla.GetByID(tipoPlantillaModel.Id_TipoPlantilla, usuarioLog);

            if (tipoCuentaExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioTipoPlantilla.Update(tipoPlantillaModel);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoPlantilla(string tipoDePlantilla)
        {
            var yaExisteTipoPlantilla = await repositorioTipoPlantilla.ExisteTipoPlantilla(tipoDePlantilla);
            return yaExisteTipoPlantilla ? Json($"El tipo de plantilla {tipoDePlantilla} ya existe") : Json(true);
        }
    }
}
