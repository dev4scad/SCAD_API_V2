
using SCAD_API_V2.Domain.Entities;

namespace SCAD_API_V2.Domain.Interfaces
{
    public interface ILicencaRepository
    {
        Task<List<Licenca>> BuscarLicencasAsync();
        Task<Licenca> BuscarLicencaPorIdAsync(int licencaId);
        Task<Licenca> BuscarLicencaPorKeyAsync(string licencaKey);
        Task<List<Licenca>> BuscarLicencaPorClienteIdAsync(int clienteId);
        Task<List<Licenca>> CriarLicencaAsync(Licenca licencaDto);
        Task<Licenca> EditarLicencaAsync(Licenca licencaDto);
        Task<bool> ExcluirLicencaAsync(int licencaId);
    }
}
