using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresupuestoDemo.Models
{
    public class Plantilla
    {
        public int Id_Plantilla { get; set; }
        public int Id_TipoPlantilla { get; set; }
        public int Id_Empresa { get; set; }
        public string DescPlantilla { get; set; }
        public bool EsActiva { get; set; }
        public int Id_UsuarioModifica { get; set; }
        public DateTime FechaModificacionLog { get; set; }
        public int Id_Presupuesto { get; set; }
        public string Observaciones { get; set; }
        public int Id_DptoEmpresa { get; set; }
    }
}
