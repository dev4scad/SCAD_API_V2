using Dapper;
using MySql.Data.MySqlClient;
using SCAD_API_V2.Domain.Entities;
using SCAD_API_V2.Domain.Interfaces;
using System.Data;

namespace SCAD_API_V2.Infrastructure.Data.Base
{
    public abstract class ClienteRepositoryBase : IClienteInterface
    {
        private readonly string _connStr;

        protected ClienteRepositoryBase(string connectionString)
        {
            _connStr = connectionString;
        }

        protected IDbConnection Connection() => new MySqlConnection(_connStr);

        public async Task<List<Cliente>> BuscarUsuariosAsync()
        {
            const string sql = "SELECT * FROM clientes";
            using var db = Connection();
            return (await db.QueryAsync<Cliente>(sql)).AsList();
        }

        public async Task<Cliente> BuscarUsuarioPorIdAsync(int usuarioId)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM clientes WHERE ClienteId = @ClienteId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { ClienteId = usuarioId }) > 0;
            if (!exists) return null!;

            const string selectSql = "SELECT * FROM clientes WHERE ClienteId = @ClienteId";
            return await db.QueryFirstAsync<Cliente>(selectSql, new { ClienteId = usuarioId });
        }

        public async Task<Cliente> BuscarUsuarioPorCPF_CNPJAsync(string CPF_CNPJ)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM clientes WHERE CPF_CNPJ = @CPF";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { CPF = CPF_CNPJ }) > 0;
            if (!exists) return null!;

            const string selectSql = "SELECT * FROM clientes WHERE CPF_CNPJ = @CPF_CNPJ";
            return await db.QueryFirstAsync<Cliente>(selectSql, new { CPF_CNPJ = CPF_CNPJ });
        }

        public async Task<Cliente> BuscarUsuarioPorEmailAsync(string email)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM clientes WHERE Email LIKE @Email";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { Email = email }) > 0;
            if (!exists) return null!;

            const string selectSql = "SELECT * FROM clientes WHERE Email LIKE @Email";
            return await db.QueryFirstAsync<Cliente>(selectSql, new { Email = email });
        }

        public async Task<List<Cliente>> BuscarUsuarioPorNomeAsync(string nome)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM clientes WHERE Nome LIKE @Nome";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { Nome = nome }) > 0;
            if (!exists) return new List<Cliente>();

            const string selectSql = "SELECT * FROM clientes WHERE Nome LIKE @Nome";
            return (await db.QueryAsync<Cliente>(selectSql, new { Nome = nome })).AsList();
        }

        public async Task<List<Cliente>> BuscarUsuarioPorTelefoneAsync(string telefone)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM clientes WHERE Telefone LIKE @Tel";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { Tel = telefone }) > 0;
            if (!exists) return new List<Cliente>();

            const string selectSql = "SELECT * FROM clientes WHERE Telefone LIKE @Tel";
            return (await db.QueryAsync<Cliente>(selectSql, new { Tel = telefone })).AsList();
        }

        public async Task<List<Cliente>> CriarUsuarioAsync(Cliente cliente)
        {
            using var db = Connection();

            const string countSql = @"
                SELECT COUNT(1)
                FROM clientes
                WHERE CNPJ_CPF = @CNPJ_CPF";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { cliente.CPF_CNPJ }) > 0;
            if (exists)
                return null!;

            const string insertSql = @"
                INSERT INTO clientes
                (CPF_CNPJ, Email, Nome, NomeEmpresa, Telefone, DataCadastro)
                VALUES
                (@CPF_CNPJ, @Email, @Nome, @NomeEmpresa, @Telefone, @DataCadastro);";

            await db.ExecuteAsync(insertSql, new
            {
                cliente.CPF_CNPJ,
                cliente.Email,
                cliente.Nome,
                cliente.NomeEmpresa,
                cliente.Telefone,
                cliente.DataCadastro
            });

            const string selectSql = @"
        SELECT
            ClienteId,
            CPF_CNPJ,
            Email,
            Nome,
            NomeEmpresa,
            Telefone,
            DataCadastro,
            DataExpira
          FROM clientes
         WHERE ClienteId = LAST_INSERT_ID();";

            var criado = await db.QuerySingleAsync<Cliente>(selectSql);
            return new List<Cliente> { criado };
        }

        public async Task<Cliente> EditarUsuarioAsync(Cliente cliente)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM Cliente WHERE ClienteId = @ClienteId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { cliente.ClienteId }) > 0;
            if (!exists) return null!;

            const string updateSql = @"
                UPDATE Cliente
                SET Nome = @Nome,
                    Email = @Email,
                    CPF_CNPJ = @CPF_CNPJ,
                    Telefone = @Telefone
                WHERE ClienteId = @ClienteId;";
            await db.ExecuteAsync(updateSql, cliente);

            const string selectSql = "SELECT * FROM Cliente WHERE ClienteId = @ClienteId";
            return await db.QuerySingleAsync<Cliente>(selectSql, new { cliente.ClienteId });
        }

        public async Task<bool> ExcluirUsuarioAsync(int usuarioId)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM Cliente WHERE ClienteId = @ClienteId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { ClienteId = usuarioId }) > 0;
            if (!exists) return false;

            const string deleteSql = "DELETE FROM Cliente WHERE ClienteId = @ClienteId";
            var rows = await db.ExecuteAsync(deleteSql, new { ClienteId = usuarioId });
            return rows > 0;
        }
    }
}