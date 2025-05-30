using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using SCAD_API_V2.Application.Interfaces;
using SCAD_API_V2.Domain.Entities;
using System.Data;

namespace SCAD_API_V2.Infrastructure.Data
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ConnectionString _connStr;

        public UsuarioRepository(IOptions<ConnectionString> options)
        {
            _connStr = options.Value;
        }

        private IDbConnection Connection()
        {
            var conn = _connStr.Autopower;

            return new MySqlConnection(conn);
        }

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
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { usuarioId }) > 0;
            if (!exists) return null!;

            const string selectSql = "SELECT * FROM usuarios WHERE usuarioId = @usuarioId";
            return await db.QueryFirstAsync<Usuario>(selectSql, new { usuarioId });
        }

        public async Task<Usuario> BuscarUsuarioPorEmailAsync(string email)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM usuarios WHERE email = @email";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { email }) > 0;
            if (!exists) return null!;

            const string selectSql = "SELECT * FROM usuarios WHERE email = @email";
            return await db.QueryFirstAsync<Usuario>(selectSql, new { email });
        }

        public async Task<Usuario> BuscarUsuarioPorNomeAsync(string nomeDeUsuario)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM usuarios WHERE nomeDoUsuario LIKE @nomeDeUsuario";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { nomeDeUsuario }) > 0;
            if (!exists) return null!;

            const string selectSql = "SELECT * FROM usuarios WHERE nomeDoUsuario LIKE @nomeDeUsuario";
            return await db.QueryFirstAsync<Usuario>(selectSql, new { nomeDeUsuario });
        }

        public async Task<Usuario> CadastrarUsuarioAsync(Usuario usuario)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM usuarios WHERE email = @Email";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { usuario.Email }) > 0;
            if (exists)
                return null!;

            const string insertSql = @"
                INSERT INTO usuarios (usuarioId, nomeDoUsuario, email, senha, ativo)
                VALUES (@UsuarioId, @NomeDoUsuario, @Email, @Senha, @Ativo);";
            await db.ExecuteAsync(insertSql, usuario);

            const string selectSql = "SELECT * FROM usuarios WHERE usuarioId = @usuarioId";
            return await db.QuerySingleAsync<Usuario>(selectSql, new { usuarioId = usuario.UsuarioId });
        }

        public async Task<Usuario> EditarUsuarioAsync(Usuario usuario)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM usuarios WHERE usuarioId = @usuarioId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { usuarioId = usuario.UsuarioId }) > 0;
            if (!exists) return null!;

            const string updateSql = @"
                UPDATE usuarios
                SET nomeDoUsuario = @nomeDoUsuario,
                    email = @Email,
                    senha = @Senha,
                    ativo = @Ativo
                WHERE UsuarioId = @UsuarioId;";
            await db.ExecuteAsync(updateSql, usuario);

            const string selectSql = "SELECT * FROM usuarios WHERE usuarioId = @usuarioId";
            return await db.QuerySingleAsync<Usuario>(selectSql, new { usuarioId = usuario.UsuarioId });
        }

        public async Task<bool> ExcluirUsuarioAsync(int usuarioId)
        {
            using var db = Connection();
            const string countSql = "SELECT COUNT(1) FROM usuarios WHERE usuarioId = @usuarioId";
            var exists = await db.ExecuteScalarAsync<int>(countSql, new { usuarioId = usuarioId }) > 0;
            if (!exists) return false;

            const string deleteSql = "DELETE FROM usuarios WHERE usuarioId = @usuarioId";
            var rows = await db.ExecuteAsync(deleteSql, new { usuarioId = usuarioId });
            return rows > 0;
        }
    }
}