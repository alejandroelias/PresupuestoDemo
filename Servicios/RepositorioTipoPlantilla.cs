using Dapper;
using Microsoft.Extensions.Configuration;
using PresupuestoDemo.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PresupuestoDemo.Servicios
{
    public interface IRepositorioTipoPlantilla
    {
        Task Create(TipoPlantillaModel tipoPlantilla);

        Task<bool> ExisteTipoPlantilla(string descripcionPlantilla);

        Task<TipoPlantillaModel> GetByID(int tipoPlantillaID, string usuarioLog);
        Task<IEnumerable<TipoPlantillaModel>> Get();

        Task Update(TipoPlantillaModel tipoPlantilla);
        Task Delete(int Id_TipoPlantilla);
    }

    public class RepositorioTipoPlantilla : IRepositorioTipoPlantilla
    {
        private readonly string connectionString;

        public RepositorioTipoPlantilla(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(TipoPlantillaModel tipoPlantilla)
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

        public async Task<IEnumerable<TipoPlantillaModel>> Get()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoPlantillaModel>(@"SELECT *
                                                                FROM [GRL].[TipoPlantilla]");
        }

        public async Task Update(TipoPlantillaModel tipoPlantilla)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE GRL.TipoPlantilla SET TipoPlantilla=@TipoPlantilla
                                            WHERE Id_TipoPlantilla=@Id_TipoPlantilla", tipoPlantilla);
        }

        public async Task<TipoPlantillaModel> GetByID(int Id_TipoPlantilla, string usuarioLog)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoPlantillaModel>(@"SELECT
                                                                                Id_TipoPlantilla
                                                                                ,TipoPlantilla
                                                                                ,EsActiva
                                                                                ,Codigo
                                                                            FROM [GRL].[TipoPlantilla]
                                                                            WHERE [Id_TipoPlantilla]=@Id_TipoPlantilla
                                                                            AND [UsuarioLog]=@UsuarioLog",
                                                                          new { Id_TipoPlantilla, usuarioLog });
        }

        public async Task Delete (int Id_TipoPlantilla)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE [GRL].[TipoPlantilla] WHERE Id_TipoPlantilla=@Id_TipoPlantilla", new { Id_TipoPlantilla });

        }
    }
}