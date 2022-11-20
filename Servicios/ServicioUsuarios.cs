using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresupuestoDemo.Servicios
{
    public interface IServicioUsuarios
    {
        string GetUserLog();
    }
    public class ServicioUsuarios:IServicioUsuarios
    {
        public string GetUserLog()
        {
            return "Admin";
        }
    }
}
