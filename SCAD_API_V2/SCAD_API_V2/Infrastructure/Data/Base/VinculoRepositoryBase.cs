using Dapper;
using MySql.Data.MySqlClient;
using SCAD_API_V2.Domain.Entities;
using SCAD_API_V2.Domain.Interfaces;
using System.Data;

namespace SCAD_API_V2.Infrastructure.Data.Base
{
    public abstract class BaseVinculoRepository : IVinculoInterface
    {
        private readonly string _connStr;
        protected BaseVinculoRepository(string connectionString) => _connStr = connectionString;
        protected IDbConnection Connection() => new MySqlConnection(_connStr);

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

        public async Task<Vinculo> BuscarVinculoPorMaquinaAsync(string maquina)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM vinculos WHERE Maquina = @Maquina";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { Maquina = maquina }) > 0;
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
                 WHERE Maquina = @Maquina";
            return await db.QueryFirstAsync<Vinculo>(selectSql, new { Maquina = maquina });
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

            const string countSql = "SELECT COUNT(1) FROM vinculos WHERE LicencaId = @LicencaId AND Maquina = @Maquina";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { vinculo.LicencaId, vinculo.Maquina }) > 0;
            if (exists) return null!;

            const string insertSql = @"
                INSERT INTO vinculos
                    (Licenca, Maquina, LicencaId, DataVinculo, NomeMaquina)
                VALUES
                    (@Licenca, @Maquina, @LicencaId, @DataVinculo, @NomeMaquina);";
            await db.ExecuteAsync(insertSql, vinculo);

            const string selectSql = "SELECT * FROM vinculos WHERE VinculoId = LAST_INSERT_ID()";
            return await db.QuerySingleAsync<Vinculo>(selectSql);
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
