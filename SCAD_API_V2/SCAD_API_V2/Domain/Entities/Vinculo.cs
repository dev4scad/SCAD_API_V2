    namespace SCAD_API_V2.Domain.Entities
{
    public class Vinculo
    {
        public int VinculoId { get; set; }
        public string LicencaKey { get; set; } = string.Empty;
        public string Maquina { get; set; } = string.Empty;
        public int LicencaId { get; set; }
        public string DataVinculo { get; set; } = string.Empty;
        public string NomeMaquina { get; set; } = string.Empty;
    }
}
