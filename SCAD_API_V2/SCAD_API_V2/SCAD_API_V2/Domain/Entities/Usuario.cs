namespace SCAD_API_V2.Domain.Entities
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string NomeDoUsuario { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }
    }
}
