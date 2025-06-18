namespace SCAD_API_V2.Domain.Entities
{
    public class Licenca
    {
        public int LicencaId { get; set; }
        public string LicencaKey { get; set; } = null!;
        public int ClienteId { get; set; }
        public bool Ativo { get; set; }
        public int TipoLicencaId { get; set; }
        public int SoftwareId { get; set; }
        public string? Software { get; set; }
        public string? TipoLicenca { get; set; }
        public string? Periodo { get; set; }

    }
}
