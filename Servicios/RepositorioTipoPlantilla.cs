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
        Task<TipoPlantilla> GetByID(int tipoPlantillaID, string usuarioLog);
        Task<IEnumerable<TipoPlantilla>> Get(int plantillaID);
        Task Update(TipoPlantilla tipoPlantilla);
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

        public async Task<IEnumerable<TipoPlantilla>> Get(int plantillaID)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoPlantilla>(@"SELECT *
                                                                FROM [GRL].[TipoPlantilla]",
                                                                new { plantillaID });
        }

        public async Task Update(TipoPlantilla tipoPlantilla)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE GRL.TipoPlantilla SET TipoPlantilla=@TipoPlantilla WHERE Id_TipoPlantilla=@Id_TipoPlantilla", tipoPlantilla);
        }

        public async Task<TipoPlantilla> GetByID(int tipoPlantillaID, string usuarioLog)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoPlantilla>(@"SELECT [Id_TipoPlantilla]
                                                                              ,[TipoPlantilla]
                                                                              ,[EsActiva]
                                                                              ,[UsuarioLog]
                                                                              ,[Codigo]
                                                                          FROM [Presupuesto].[GRL].[TipoPlantilla]
                                                                          WHERE [Id_TipoPlantilla]=@Id_TipoPlantilla AND                                                    [UsuarioLog]=@UsuarioLog",
                                                                          new { tipoPlantillaID, usuarioLog}); 
        }
    }
}
