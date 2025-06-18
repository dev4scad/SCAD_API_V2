using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using SCAD_API_V2.Application.Http;
using SCAD_API_V2.Domain.Entities;
using SCAD_API_V2.Domain.Enums;
using SCAD_API_V2.Domain.Interfaces;
using System.Data;

namespace SCAD_API_V2.Infrastructure.Data
{
    public class LicencaRepository : ILicencaRepository
    {
        private readonly ConnectionString _connStr;
        private readonly ICurrentDatabase _currentDb;

        public LicencaRepository(IOptions<ConnectionString> options, ICurrentDatabase currentDb)
        {
            _connStr = options.Value;
            _currentDb = currentDb;
        }

        private IDbConnection Connection()
        {
            var conn = _currentDb.Database == Database.Autopower
                ? _connStr.Autopower
                : _connStr.Autohidro;

            return new MySqlConnection(conn);
        }

        public async Task<List<Licenca>> BuscarLicencasAsync()
        {
            const string sql = @"
                SELECT 
                    LicencaId, 
                    Licenca AS LicencaKey, 
                    ClienteId, 
                    Ativo, 
                    TipoLicencaId, 
                    SoftwareId 
                FROM licencas";
            using var db = Connection();
            return (await db.QueryAsync<Licenca>(sql)).AsList();
        }

        public async Task<Licenca> BuscarLicencaPorIdAsync(int licencaId)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM licencas WHERE LicencaId = @LicencaId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { LicencaId = licencaId }) > 0;
            if (!exists) return null!;

            const string selectSql = @"
                SELECT 
                    LicencaId, 
                    Licenca AS LicencaKey, 
                    ClienteId, 
                    Ativo, 
                    TipoLicencaId, 
                    SoftwareId
                FROM licencas 
                WHERE LicencaId = @LicencaId";
            return await db.QueryFirstAsync<Licenca>(selectSql, new { LicencaId = licencaId });
        }

        public async Task<Licenca> BuscarLicencaPorKeyAsync(string licencaKey)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM licencas WHERE Licenca = @Key";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { Key = licencaKey }) > 0;
            if (!exists) return null!;

            const string selectSql = @"
                SELECT 
                    LicencaId, 
                    Licenca AS LicencaKey, 
                    ClienteId, 
                    Ativo, 
                    TipoLicencaId, 
                    SoftwareId
                FROM licencas 
                WHERE Licenca = @Key";
            return await db.QueryFirstAsync<Licenca>(selectSql, new { Key = licencaKey });
        }

        public async Task<List<Licenca>> BuscarLicencaPorClienteIdAsync(int clienteId)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM licencas WHERE ClienteId = @ClienteId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { ClienteId = clienteId }) > 0;
            if (!exists) return new List<Licenca>();

            const string selectSql = @"
                SELECT 
                    l.LicencaId, 
                    l.Licenca AS LicencaKey, 
                    l.ClienteId, 
                    l.Ativo, 
                    l.TipoLicencaId, 
                    l.SoftwareId,
                    s.Software,
                    tl.Tipo AS TipoLicenca,
                    tl.Periodo
                FROM licencas l
                INNER JOIN software s ON l.SoftwareId = s.Id
                INNER JOIN tiposlicenca tl ON l.TipoLicencaId = tl.TipoId
                WHERE l.ClienteId = @ClienteId";
            return (await db.QueryAsync<Licenca>(selectSql, new { ClienteId = clienteId })).AsList();
        }

        public async Task<List<Licenca>> CriarLicencaAsync(Licenca licenca)
        {
            using var db = Connection();

            const string countKeySql = "SELECT COUNT(1) FROM licencas WHERE Licenca = @LicencaKey";
            var keyExists = await db.ExecuteScalarAsync<int>(countKeySql, new { LicencaKey = licenca.LicencaKey }) > 0;
            if (keyExists)
                return new List<Licenca>();

            const string countClienteSql = "SELECT COUNT(1) FROM clientes WHERE ClienteId = @ClienteId";
            const string countTipoSql = "SELECT COUNT(1) FROM tiposlicenca WHERE TipoId = @TipoLicencaId";
            const string countSoftwareSql = "SELECT COUNT(1) FROM software WHERE Id = @SoftwareId";

            var clienteExists = await db.ExecuteScalarAsync<int>(countClienteSql, new { ClienteId = licenca.ClienteId }) > 0;
            var tipoExists = await db.ExecuteScalarAsync<int>(countTipoSql, new { TipoLicencaId = licenca.TipoLicencaId }) > 0;
            var softwareExists = await db.ExecuteScalarAsync<int>(countSoftwareSql, new { SoftwareId = licenca.SoftwareId }) > 0;

            if (!clienteExists || !tipoExists || !softwareExists)
                return new List<Licenca>();

            const string insertSql = @"
                INSERT INTO licencas
                    (Licenca, ClienteId, TipoLicencaId, SoftwareId, Ativo)
                VALUES
                    (@LicencaKey, @ClienteId, @TipoLicencaId, @SoftwareId, @Ativo);
                SELECT LAST_INSERT_ID();";
            var novoLicencaId = await db.ExecuteScalarAsync<int>(insertSql, licenca);

            const string selectSql = @"
                SELECT 
                    LicencaId, 
                    Licenca AS LicencaKey, 
                    ClienteId, 
                    Ativo, 
                    TipoLicencaId, 
                    SoftwareId
                FROM licencas 
                WHERE LicencaId = @LicencaId";
            var criado = await db.QuerySingleAsync<Licenca>(selectSql, new { LicencaId = novoLicencaId });
            return new List<Licenca> { criado };
        }

        public async Task<Licenca> EditarLicencaAsync(Licenca licenca)
        {
            using var db = Connection();

            const string countSql = "SELECT COUNT(1) FROM licencas WHERE LicencaId = @LicencaId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { licenca.LicencaId }) > 0;
            if (!exists) return null!;

            const string countClienteSql = "SELECT COUNT(1) FROM clientes WHERE ClienteId = @ClienteId";
            const string countTipoSql = "SELECT COUNT(1) FROM tiposlicenca WHERE TipoId = @TipoLicencaId";
            const string countSoftwareSql = "SELECT COUNT(1) FROM software WHERE Id = @SoftwareId";

            var clienteExists = await db.ExecuteScalarAsync<int>(countClienteSql, new { licenca.ClienteId }) > 0;
            var tipoExists = await db.ExecuteScalarAsync<int>(countTipoSql, new { licenca.TipoLicencaId }) > 0;
            var softwareExists = await db.ExecuteScalarAsync<int>(countSoftwareSql, new { licenca.SoftwareId }) > 0;

            if (!clienteExists || !tipoExists || !softwareExists)
                return null!;

            const string updateSql = @"
                UPDATE licencas
                SET
                    Licenca        = @LicencaKey,
                    ClienteId      = @ClienteId,
                    TipoLicencaId  = @TipoLicencaId,
                    SoftwareId     = @SoftwareId,
                    Ativo          = @Ativo
                WHERE LicencaId = @LicencaId;";
            await db.ExecuteAsync(updateSql, licenca);

            const string selectSql = @"
                SELECT 
                    LicencaId, 
                    Licenca AS LicencaKey, 
                    ClienteId, 
                    Ativo, 
                    TipoLicencaId, 
                    SoftwareId
                FROM licencas 
                WHERE LicencaId = @LicencaId";
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
