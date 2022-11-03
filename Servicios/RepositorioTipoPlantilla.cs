using Dapper;
using Microsoft.Extensions.Configuration;
using PresupuestoDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PresupuestoDemo.Servicios
{
    public interface IRepositorioTipoPlantilla
    {
        Task Create(TipoPlantilla tipoPlantilla);
        Task<bool> ExisteTipoPlantilla(string descripcionPlantilla);
    }

    public class RepositorioTipoPlantilla : IRepositorioTipoPlantilla
    {
        private readonly string connectionString;
        public RepositorioTipoPlantilla(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(TipoPlantilla tipoPlantilla)
        {
            using var connection = new SqlConnection(connectionString);

            //TODO: el campo FechaModificacionLog no funciona
            var idTipoPlantilla = await connection.QuerySingleAsync<int>
                ($@"insert into GRL.TipoPlantilla(TipoPlantilla, EsActiva, UsuarioLog, FechaModificacionLog, Codigo) 
                values (@TipoDePlantilla, @EsActiva, @UsuarioLog, '20221102', @Codigo);
                select SCOPE_IDENTITY();", tipoPlantilla);

            tipoPlantilla.Id_TipoPlantilla = idTipoPlantilla;
        }



        //Metodo de validacion de ingreso de datos 
        public async Task<bool> ExisteTipoPlantilla(string tipoPlantilla)
        {
            using var connection = new SqlConnection(connectionString);
            var existeTipoPlantilla = await connection.QueryFirstOrDefaultAsync<int>(
                @"select 1 
                from GRL.TipoPlantilla 
                where TipoPlantilla = @TipoPlantilla;",
                new { tipoPlantilla });

            return existeTipoPlantilla == 1;
        }
    }
}
