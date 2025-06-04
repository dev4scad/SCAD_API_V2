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
    public class VinculoRepository : IVinculoRepository
    {
        private readonly ConnectionString _connStr;
        private readonly ICurrentDatabase _currentDb;

        public VinculoRepository(IOptions<ConnectionString> options, ICurrentDatabase currentDb)
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

        public async Task<List<Vinculo>> ListarVinculosAsync()
        {
            const string sql = @"
                SELECT
                    VinculoId,
                    Licenca,
                    Maquina,
                    LicencaId,
                    DataVinculo,
                    NomeMaquina
                  FROM vinculos";
            using var db = Connection();
            return (await db.QueryAsync<Vinculo>(sql)).AsList();
        }

        public async Task<Vinculo> BuscarVinculoPorIdAsync(int vinculoId)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM vinculos WHERE VinculoId = @VinculoId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { VinculoId = vinculoId }) > 0;
            if (!exists) return null!;

            const string selectSql = @"
                SELECT
                    VinculoId,
                    Licenca,
                    Maquina,
                    LicencaId,
                    DataVinculo,
                    NomeMaquina
                  FROM vinculos
                 WHERE VinculoId = @VinculoId";
            return await db.QueryFirstAsync<Vinculo>(selectSql, new { VinculoId = vinculoId });
        }

        public async Task<Vinculo> BuscarVinculoPorLicencaAsync(string licencaKey)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM vinculos WHERE Licenca = @Licenca";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { Licenca = licencaKey }) > 0;
            if (!exists) return null!;

            const string selectSql = @"
                SELECT
                    VinculoId,
                    Licenca,
                    Maquina,
                    LicencaId,
                    DataVinculo,
                    NomeMaquina
                  FROM vinculos
                 WHERE Licenca = @Licenca";
            return await db.QueryFirstAsync<Vinculo>(selectSql, new { Licenca = licencaKey });
        }

        public async Task<Vinculo> BuscarVinculoPorMaquinaAsync(string maquina, int softwareId)
        {
            using var db = Connection();
            // Verifica se existe vínculo para a máquina e software
            const string countSql = @"
                SELECT COUNT(1)
                  FROM vinculos v
                  JOIN licencas l ON v.LicencaId = l.LicencaId
                 WHERE v.Maquina = @Maquina
                   AND l.SoftwareId = @SoftwareId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { Maquina = maquina, SoftwareId = softwareId }) > 0;
            if (!exists) return null!;

            const string selectSql = @"
                SELECT
                    v.VinculoId,
                    v.Licenca,
                    v.Maquina,
                    v.LicencaId,
                    v.DataVinculo,
                    v.NomeMaquina
                  FROM vinculos v
                  JOIN licencas l ON v.LicencaId = l.LicencaId
                 WHERE v.Maquina = @Maquina
                   AND l.SoftwareId = @SoftwareId";
            return await db.QueryFirstAsync<Vinculo>(selectSql, new { Maquina = maquina, SoftwareId = softwareId });
        }

        public async Task<Vinculo> BuscarVinculoPorLicencaIdAsync(int licencaId)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM vinculos WHERE LicencaId = @LicencaId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { LicencaId = licencaId }) > 0;
            if (!exists) return null!;

            const string selectSql = @"
                SELECT
                    VinculoId,
                    Licenca,
                    Maquina,
                    LicencaId,
                    DataVinculo,
                    NomeMaquina
                  FROM vinculos
                 WHERE LicencaId = @LicencaId";
            return await db.QueryFirstAsync<Vinculo>(selectSql, new { LicencaId = licencaId });
        }

        public async Task<Vinculo> BuscarVinculoPorNomeMaquinaAsync(string nomeMaquina)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM vinculos WHERE NomeMaquina = @NomeMaquina";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { NomeMaquina = nomeMaquina }) > 0;
            if (!exists) return null!;

            const string selectSql = @"
                SELECT
                    VinculoId,
                    Licenca,
                    Maquina,
                    LicencaId,
                    DataVinculo,
                    NomeMaquina
                  FROM vinculos
                 WHERE NomeMaquina = @NomeMaquina";
            return await db.QueryFirstAsync<Vinculo>(selectSql, new { NomeMaquina = nomeMaquina });
        }

        public async Task<Vinculo> CadastrarVinculoAsync(Vinculo vinculo)
        {
            using var db = Connection();

            const string countLicencaSql = "SELECT COUNT(1) FROM licencas WHERE LicencaId = @LicencaId";
            var licencaExists = await db.ExecuteScalarAsync<int>(countLicencaSql, new { vinculo.LicencaId }) > 0;
            if (!licencaExists)
                throw new KeyNotFoundException($"LicencaId {vinculo.LicencaId} não encontrado.");

            // Nova verificação: a licença já está vinculada a alguma máquina?
            const string countLicencaVinculoSql = "SELECT COUNT(1) FROM vinculos WHERE LicencaId = @LicencaId";
            var licencaJaVinculada = await db.ExecuteScalarAsync<int>(countLicencaVinculoSql, new { vinculo.LicencaId }) > 0;
            if (licencaJaVinculada)
                return null!;

            // Obter o SoftwareId da licença que está sendo vinculada
            const string getSoftwareIdSql = "SELECT SoftwareId FROM licencas WHERE LicencaId = @LicencaId";
            var softwareId = await db.ExecuteScalarAsync<int>(getSoftwareIdSql, new { vinculo.LicencaId });

            // Verificar se já existe vínculo para a mesma máquina e mesmo software
            const string countSql = @"
                SELECT COUNT(1)
                  FROM vinculos v
                  JOIN licencas l ON v.LicencaId = l.LicencaId
                 WHERE v.Maquina = @Maquina
                   AND l.SoftwareId = @SoftwareId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { vinculo.Maquina, SoftwareId = softwareId }) > 0;
            if (exists) return null!;

            const string LicencaKeySql = "SELECT Licenca FROM licencas WHERE LicencaId = @LicencaId";
            vinculo.Licenca = await db.ExecuteScalarAsync<string>(LicencaKeySql, new { vinculo.LicencaId });

            const string insertSql = @"
                INSERT INTO vinculos
                    (VinculoId, Licenca, Maquina, LicencaId, DataVinculo, NomeMaquina)
                VALUES
                    (@VinculoId, @Licenca, @Maquina, @LicencaId, @DataVinculo, @NomeMaquina);";
            await db.ExecuteAsync(insertSql, vinculo);

            const string selectSql = "SELECT * FROM vinculos WHERE VinculoId = @VinculoId";
            var result = await db.QuerySingleOrDefaultAsync<Vinculo>(selectSql, vinculo);
            if (result == null)
                throw new InvalidOperationException("Houve um erro ao cadastrar/retornar o vinculo.");
            return result;
        }

        public async Task<Vinculo> EditarVinculoAsync(Vinculo vinculo)
        {
            using var db = Connection();

            const string countSql = "SELECT COUNT(1) FROM vinculos WHERE VinculoId = @VinculoId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { vinculo.VinculoId }) > 0;
            if (!exists) return null!;

            const string countLicencaSql = "SELECT COUNT(1) FROM licencas WHERE LicencaId = @LicencaId";
            var licencaExists = await db.ExecuteScalarAsync<int>(countLicencaSql, new { vinculo.LicencaId }) > 0;
            if (!licencaExists)
                throw new KeyNotFoundException($"LicencaId {vinculo.LicencaId} não encontrado.");

            const string updateSql = @"
                UPDATE vinculos
                   SET Licenca     = @Licenca,
                       Maquina     = @Maquina,
                       LicencaId   = @LicencaId,
                       DataVinculo = @DataVinculo,
                       NomeMaquina = @NomeMaquina
                 WHERE VinculoId  = @VinculoId;";
            await db.ExecuteAsync(updateSql, vinculo);

            const string selectSql = "SELECT * FROM vinculos WHERE VinculoId = @VinculoId";
            return await db.QuerySingleAsync<Vinculo>(selectSql, new { vinculo.VinculoId });
        }

        public async Task<bool> ExcluirVinculoAsync(int vinculoId)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM vinculos WHERE VinculoId = @VinculoId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { VinculoId = vinculoId }) > 0;
            if (!exists) return false;

            const string deleteSql = "DELETE FROM vinculos WHERE VinculoId = @VinculoId";
            var rows = await db.ExecuteAsync(deleteSql, new { VinculoId = vinculoId });
            return rows > 0;
        }
    }
}
