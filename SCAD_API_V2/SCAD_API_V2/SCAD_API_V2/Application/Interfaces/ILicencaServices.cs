using SCAD_API_V2.Application.DTO;

namespace SCAD_API_V2.Application.Interfaces
{
    public interface ILicencaServices
    {
        Task<List<LicencaDto>> BuscarLicencasAsync();
        Task<LicencaDto> BuscarLicencaPorIdAsync(int licencaId);
        Task<LicencaDto> BuscarLicencaPorKeyAsync(string licencaKey);
        Task<List<LicencaDto>> BuscarLicencaPorClienteIdAsync(int clienteId);
        Task<List<LicencaDto>> CriarLicencaAsync(LicencaDto licencaDto);
        Task<LicencaDto> EditarLicencaAsync(LicencaDto licencaDto);
        Task<bool> ExcluirLicencaAsync(int licencaId);
    }
}
