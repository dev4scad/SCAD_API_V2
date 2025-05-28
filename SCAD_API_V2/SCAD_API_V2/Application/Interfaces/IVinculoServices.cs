using SCAD_API_V2.Application.DTO;

namespace SCAD_API_V2.Application.Interfaces
{
    public interface IVinculoServices
    {
        Task<List<VinculoDto>> ListarVinculosAsync();
        Task<VinculoDto> BuscarVinculoPorIdAsync(int id);
        Task<VinculoDto> BuscarVinculoPorLicencaAsync(string licenca);
        Task<VinculoDto> BuscarVinculoPorMaquinaAsync(string maquina);
        Task<VinculoDto> BuscarVinculoPorLicencaIdAsync(int licencaId);
        Task<VinculoDto> BuscarVinculoPorNomeMaquinaAsync(string nomeMaquina);
        Task<VinculoDto> CadastrarVinculoAsync(VinculoDto vinculoDto);
        Task<VinculoDto> EditarVinculoAsync(VinculoDto vinculoDto);
        Task<bool> ExcluirVinculoAsync(int id);
    }
}
