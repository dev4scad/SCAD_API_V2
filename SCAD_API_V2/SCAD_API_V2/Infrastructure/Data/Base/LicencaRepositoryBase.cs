using Dapper;
using MySql.Data.MySqlClient;
using SCAD_API_V2.Domain.Entities;
using SCAD_API_V2.Application.Interfaces;
using System.Data;

namespace SCAD_API_V2.Infrastructure.Data.Base
{
    public abstract class LicencaRepositoryBase : ILicencaInterface
    {
        private readonly string _connStr;

        protected LicencaRepositoryBase(string connectionString)
        {
            _connStr = connectionString;
        }

        protected IDbConnection Connection() => new MySqlConnection(_connStr);

        public async Task<List<Licenca>> BuscarLicencasAsync()
        {
            const string sql = "SELECT * FROM licencas";
            using var db = Connection();
            return (await db.QueryAsync<Licenca>(sql)).AsList();
        }

        public async Task<Licenca> BuscarLicencaPorIdAsync(int licencaId)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM licencas WHERE LicencaId = @LicencaId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { LicencaId = licencaId }) > 0;
            if (!exists) return null!;

            const string selectSql = "SELECT * FROM licencas WHERE LicencaId = @LicencaId";
            return await db.QueryFirstAsync<Licenca>(selectSql, new { LicencaId = licencaId });
        }

        public async Task<Licenca> BuscarLicencaPorKeyAsync(string licencaKey)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM licencas WHERE Licenca = @Key";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { Key = licencaKey }) > 0;
            if (!exists) return null!;

            const string selectSql = "SELECT * FROM licencas WHERE Licenca = @Key";
            return await db.QueryFirstAsync<Licenca>(selectSql, new { Key = licencaKey });
        }

        public async Task<List<Licenca>> BuscarLicencaPorClienteIdAsync(int clienteId)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM licencas WHERE ClienteId = @ClienteId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { ClienteId = clienteId }) > 0;
            if (!exists) return new List<Licenca>();

            const string selectSql = "SELECT * FROM licencas WHERE ClienteId = @ClienteId";
            return (await db.QueryAsync<Licenca>(selectSql, new { ClienteId = clienteId })).AsList();
        }

        public async Task<List<Licenca>> CriarLicencaAsync(Licenca licenca)
        {
            using var db = Connection();

            const string countKeySql = "SELECT COUNT(1) FROM licencas WHERE Licenca = @LicencaKey";
            var keyExists = await db.ExecuteScalarAsync<int>(countKeySql, new { licenca.LicencaKey }) > 0;
            if (keyExists)
                return new List<Licenca>();

            const string countClienteSql = "SELECT COUNT(1) FROM Cliente WHERE ClienteId = @ClienteId";
            const string countTipoSql = "SELECT COUNT(1) FROM TipoLicenca WHERE TipoLicencaId = @TipoLicencaId";
            const string countSoftwareSql = "SELECT COUNT(1) FROM Software WHERE SoftwareId = @SoftwareId";

            var clienteExists = await db.ExecuteScalarAsync<int>(countClienteSql, new { licenca.ClienteId }) > 0;
            var tipoExists = await db.ExecuteScalarAsync<int>(countTipoSql, new { licenca.TipoLicencaId }) > 0;
            var softwareExists = await db.ExecuteScalarAsync<int>(countSoftwareSql, new { licenca.SoftwareId }) > 0;

            if (!clienteExists || !tipoExists || !softwareExists)
                return new List<Licenca>();

            const string insertSql = @"
                INSERT INTO licencas
                    (Licenca, ClienteId, TipoLicencaId, SoftwareId, DataEmissao, DataValidade)
                VALUES
                    (@LicencaKey, @ClienteId, @TipoLicencaId, @SoftwareId, @DataEmissao, @DataValidade);";
            await db.ExecuteAsync(insertSql, licenca);

            const string selectSql = "SELECT * FROM licencas WHERE LicencaId = LAST_INSERT_ID()";
            var criado = await db.QuerySingleAsync<Licenca>(selectSql);
            return new List<Licenca> { criado };
        }

        public async Task<Licenca> EditarLicencaAsync(Licenca licenca)
        {
            using var db = Connection();

            const string countSql = "SELECT COUNT(1) FROM licencas WHERE LicencaId = @LicencaId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { licenca.LicencaId }) > 0;
            if (!exists) return null!;

            const string countClienteSql = "SELECT COUNT(1) FROM Cliente WHERE ClienteId = @ClienteId";
            const string countTipoSql = "SELECT COUNT(1) FROM TipoLicenca WHERE TipoLicencaId = @TipoLicencaId";
            const string countSoftwareSql = "SELECT COUNT(1) FROM Software WHERE SoftwareId = @SoftwareId";

            var clienteExists = await db.ExecuteScalarAsync<int>(countClienteSql, new { licenca.ClienteId }) > 0;
            var tipoExists = await db.ExecuteScalarAsync<int>(countTipoSql, new { licenca.TipoLicencaId }) > 0;
            var softwareExists = await db.ExecuteScalarAsync<int>(countSoftwareSql, new { licenca.SoftwareId }) > 0;

            if (!clienteExists || !tipoExists || !softwareExists)
                return null!;

            const string updateSql = @"
                UPDATE licencas
                SET
                    Licenca     = @LicencaKey,
                    ClienteId      = @ClienteId,
                    TipoLicencaId  = @TipoLicencaId,
                    SoftwareId     = @SoftwareId,
                    DataEmissao    = @DataEmissao,
                    DataValidade   = @DataValidade
                WHERE LicencaId = @LicencaId;";
            await db.ExecuteAsync(updateSql, licenca);

            const string selectSql = "SELECT * FROM licencas WHERE LicencaId = @LicencaId";
            return await db.QuerySingleAsync<Licenca>(selectSql, new { licenca.LicencaId });
        }

        public async Task<bool> ExcluirLicencaAsync(int licencaId)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM licencas WHERE LicencaId = @LicencaId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { LicencaId = licencaId }) > 0;
            if (!exists) return false;

            const string deleteSql = "DELETE FROM licencas WHERE LicencaId = @LicencaId";
            var rows = await db.ExecuteAsync(deleteSql, new { LicencaId = licencaId });
            return rows > 0;
        }
    }
}
