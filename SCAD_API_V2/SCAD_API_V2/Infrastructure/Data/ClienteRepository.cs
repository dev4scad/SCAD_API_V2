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
    public class ClienteRepository : IClienteRepository
    {
        private readonly ConnectionString _connStr;
        private readonly ICurrentDatabase _currentDb;

        public ClienteRepository(
            IOptions<ConnectionString> options,
            ICurrentDatabase currentDb)
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

        public async Task<List<Cliente>> BuscarClientesAsync()
        {
            const string sql = "SELECT * FROM clientes";
            using var db = Connection();
            return (await db.QueryAsync<Cliente>(sql)).AsList();
        }

        public async Task<Cliente> BuscarClientePorIdAsync(int usuarioId)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM clientes WHERE ClienteId = @ClienteId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { ClienteId = usuarioId }) > 0;
            if (!exists) return null!;

            const string selectSql = "SELECT * FROM clientes WHERE ClienteId = @ClienteId";
            return await db.QueryFirstAsync<Cliente>(selectSql, new { ClienteId = usuarioId });
        }

        public async Task<Cliente> BuscarClientePorCNPJ_CPFAsync(string CNPJ_CPF)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM clientes WHERE CNPJ_CPF = @CPF";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { CPF = CNPJ_CPF }) > 0;
            if (!exists) return null!;

            const string selectSql = "SELECT * FROM clientes WHERE CNPJ_CPF = @CNPJ_CPF";
            return await db.QueryFirstAsync<Cliente>(selectSql, new { CNPJ_CPF = CNPJ_CPF });
        }

        public async Task<Cliente> BuscarClientePorEmailAsync(string email)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM clientes WHERE Email LIKE @Email";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { Email = email }) > 0;
            if (!exists) return null!;

            const string selectSql = "SELECT * FROM clientes WHERE Email LIKE @Email";
            return await db.QueryFirstAsync<Cliente>(selectSql, new { Email = email });
        }

        public async Task<List<Cliente>> BuscarClientePorNomeAsync(string nome)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM clientes WHERE Nome LIKE @Nome";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { Nome = nome }) > 0;
            if (!exists) return new List<Cliente>();

            const string selectSql = "SELECT * FROM clientes WHERE Nome LIKE @Nome";
            return (await db.QueryAsync<Cliente>(selectSql, new { Nome = nome })).AsList();
        }

        public async Task<List<Cliente>> BuscarClientePorTelefoneAsync(string telefone)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM clientes WHERE Telefone LIKE @Tel";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { Tel = telefone }) > 0;
            if (!exists) return new List<Cliente>();

            const string selectSql = "SELECT * FROM clientes WHERE Telefone LIKE @Tel";
            return (await db.QueryAsync<Cliente>(selectSql, new { Tel = telefone })).AsList();
        }

        public async Task<List<Cliente>> CriarClienteAsync(Cliente cliente)
        {
            using var db = Connection();

            const string countSql = @"
                SELECT COUNT(1)
                FROM clientes
                WHERE CNPJ_CPF = @CNPJ_CPF";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { CNPJ_CPF = cliente.CNPJ_CPF }) > 0;
            if (exists)
                return null!;

            const string insertSql = @"
                INSERT INTO clientes
                (CNPJ_CPF, Email, Nome, NomeEmpresa, Telefone, DataCadastro)
                VALUES
                (@CNPJ_CPF, @Email, @Nome, @NomeEmpresa, @Telefone, @DataCadastro);
                SELECT LAST_INSERT_ID();";

            var novoClienteId = await db.ExecuteScalarAsync<int>(insertSql, new
            {
                CNPJ_CPF = cliente.CNPJ_CPF,
                Email = cliente.Email,
                Nome = cliente.Nome,
                NomeEmpresa = cliente.NomeEmpresa,
                Telefone = cliente.Telefone,
                DataCadastro = cliente.DataCadastro
            });

            const string selectSql = @"
                SELECT
                    ClienteId,
                    CNPJ_CPF,
                    Email,
                    Nome,
                    NomeEmpresa,
                    Telefone,
                    DataCadastro,
                    DataExpira
                FROM clientes
                WHERE ClienteId = @ClienteId;";

            var criado = await db.QuerySingleAsync<Cliente>(selectSql, new { ClienteId = novoClienteId });
            return new List<Cliente> { criado };
        }

        public async Task<Cliente> EditarClienteAsync(Cliente cliente)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM clientes WHERE ClienteId = @ClienteId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { cliente.ClienteId }) > 0;
            if (!exists) return null!;

            const string updateSql = @"
                UPDATE clientes
                SET Nome = @Nome,
                    Email = @Email,
                    CNPJ_CPF = @CNPJ_CPF,
                    Telefone = @Telefone
                WHERE ClienteId = @ClienteId;";
            await db.ExecuteAsync(updateSql, cliente);

            const string selectSql = "SELECT * FROM clientes WHERE ClienteId = @ClienteId";
            return await db.QuerySingleAsync<Cliente>(selectSql, new { cliente.ClienteId });
        }

        public async Task<bool> ExcluirClienteAsync(int usuarioId)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM clientes WHERE ClienteId = @ClienteId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { ClienteId = usuarioId }) > 0;
            if (!exists) return false;

            const string deleteSql = "DELETE FROM clientes WHERE ClienteId = @ClienteId";
            var rows = await db.ExecuteAsync(deleteSql, new { ClienteId = usuarioId });
            return rows > 0;
        }
    }
}