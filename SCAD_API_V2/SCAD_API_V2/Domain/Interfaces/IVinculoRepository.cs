using SCAD_API_V2.Domain.Entities;

namespace SCAD_API_V2.Domain.Interfaces
{
    public interface IVinculoRepository
    {
        Task<List<Vinculo>> ListarVinculosAsync();
        Task<Vinculo> BuscarVinculoPorIdAsync(int id);
        Task<Vinculo> BuscarVinculoPorLicencaAsync(string licenca);
        Task<Vinculo> BuscarVinculoPorMaquinaAsync(string maquina);
        Task<Vinculo> BuscarVinculoPorLicencaIdAsync(int licencaId);
        Task<Vinculo> BuscarVinculoPorNomeMaquinaAsync(string nomeMaquina);
        Task<Vinculo> CadastrarVinculoAsync(Vinculo vinculoDto);
        Task<Vinculo> EditarVinculoAsync(Vinculo vinculoDto);
        Task<bool> ExcluirVinculoAsync(int id);
    }
}
