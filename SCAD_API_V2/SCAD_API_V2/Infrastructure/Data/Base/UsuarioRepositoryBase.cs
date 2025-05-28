using Dapper;
using MySql.Data.MySqlClient;
using SCAD_API_V2.Domain.Entities;
using SCAD_API_V2.Application.Interfaces;
using System.Data;

namespace SCAD_API_V2.Infrastructure.Data.Base
{
    public abstract class UsuarioRepositoryBase : IUsuarioInterface
    {
        private readonly string _connStr;

        protected UsuarioRepositoryBase(string connectionString)
        {
            _connStr = connectionString;
        }

        protected IDbConnection Connection() => new MySqlConnection(_connStr);

        public async Task<List<Usuario>> ListarUsuariosAsync()
        {
            const string sql = "SELECT * FROM usuarios";
            using var db = Connection();
            return (await db.QueryAsync<Usuario>(sql)).AsList();
        }

        public async Task<Usuario> BuscarUsuarioPorIdAsync(int usuarioId)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM usuarios WHERE usuarioId = @usuarioId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { usuarioId = usuarioId }) > 0;
            if (!exists) return null!;

            const string selectSql = "SELECT * FROM usuarios WHERE usuarioId = @usuarioId";
            return await db.QueryFirstAsync<Usuario>(selectSql, new { usuarioId = usuarioId });
        }

        public async Task<Usuario> BuscarUsuarioPorEmailAsync(string email)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM usuarios WHERE email = @email";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { email = email }) > 0;
            if (!exists) return null!;

            const string selectSql = "SELECT * FROM usuarios WHERE email = @email";
            return await db.QueryFirstAsync<Usuario>(selectSql, new { email = email });
        }

        public async Task<Usuario> BuscarUsuarioPorNomeAsync(string nomeDeUsuario)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM usuarios WHERE nomeDeUsuario LIKE @nomeDeUsuario";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { nomeDeUsuario = nomeDeUsuario }) > 0;
            if (!exists) return null!;

            const string selectSql = "SELECT * FROM usuarios WHERE nomeDeUsuario LIKE @nomeDeUsuario";
            return await db.QueryFirstAsync<Usuario>(selectSql, new { nomeDeUsuario = nomeDeUsuario });
        }

        public async Task<Usuario> CadastrarUsuarioAsync(Usuario usuario)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM usuarios WHERE email = @Email";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { usuario.Email }) > 0;
            if (exists)
                return null!;

            const string insertSql = @"
                INSERT INTO usuarios (nomeDeUsuario, email, senha, ativo)
                VALUES (@nomeDeUsuario, @Email, @Senha, @Ativo);";
            await db.ExecuteAsync(insertSql, usuario);

            const string selectSql = "SELECT * FROM usuarios WHERE usuarioId = LAST_INSERT_ID()";
            return await db.QuerySingleAsync<Usuario>(selectSql);
        }

        public async Task<Usuario> EditarUsuarioAsync(Usuario usuario)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM usuarios WHERE usuarioId = @usuarioId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { usuario.UsuarioId }) > 0;
            if (!exists) return null!;

            const string updateSql = @"
                UPDATE usuarios
                SET nomeDeUsuario = @nomeDeUsuario,
                    email = @Email,
                    senha = @Senha,
                    ativo = @Ativo
                WHERE UsuarioId = @UsuarioId;";
            await db.ExecuteAsync(updateSql, usuario);

            const string selectSql = "SELECT * FROM usuarios WHERE usuarioId = @usuarioId";
            return await db.QuerySingleAsync<Usuario>(selectSql, new { usuario.UsuarioId });
        }

        public async Task<bool> ExcluirUsuarioAsync(int usuarioId)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM usuarios WHERE usuarioId = @usuarioId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { UsuarioId = usuarioId }) > 0;
            if (!exists) return false;

            const string deleteSql = "DELETE FROM usuarios WHERE usuarioId = @usuarioId";
            var rows = await db.ExecuteAsync(deleteSql, new { UsuarioId = usuarioId });
            return rows > 0;
        }
    }
}