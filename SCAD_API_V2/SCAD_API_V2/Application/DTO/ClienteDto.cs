namespace SCAD_API_V2.Application.DTO
{
    public class ClienteDto
    {
        public string Email { get; set; }
        public string CPF_CNPJ { get; set; }
        public string Nome { get; set; }
        public string NomeEmpresa { get; set; }
        public string Telefone { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataExpira { get; set; }
    }
}
