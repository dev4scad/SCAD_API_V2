namespace SCAD_API_V2.Application.DTO
{
    public class ClienteDto
    {
        public int ClienteId { get; set; }
        public string Email { get; set; }
        public string CNPJ_CPF { get; set; }
        public string Nome { get; set; }
        public string NomeEmpresa { get; set; }
        public string Telefone { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataExpira { get; set; }
        public List<LicencaDto>? Licencas { get; set; }
    }
}
