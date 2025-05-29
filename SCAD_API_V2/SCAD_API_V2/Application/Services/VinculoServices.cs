using AutoMapper;
using SCAD_API_V2.Application.DTO;
using SCAD_API_V2.Application.Interfaces;
using SCAD_API_V2.Domain.Entities;
using SCAD_API_V2.Domain.Interfaces;

namespace SCAD_API_V2.Application.Services
{
    public class VinculoServices : IVinculoServices
    {
        private readonly IVinculoRepository _repo;
        private readonly IMapper _mapper;

        public VinculoServices(IVinculoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<VinculoDto>> ListarVinculosAsync()
        {
            var entidades = await _repo.ListarVinculosAsync();
            return _mapper.Map<List<VinculoDto>>(entidades);
        }

        public async Task<VinculoDto> BuscarVinculoPorIdAsync(int id)
        {
            var entidade = await _repo.BuscarVinculoPorIdAsync(id);
            return _mapper.Map<VinculoDto>(entidade);
        }

        public async Task<VinculoDto> BuscarVinculoPorLicencaAsync(string licenca)
        {
            var entidade = await _repo.BuscarVinculoPorLicencaAsync(licenca);
            return _mapper.Map<VinculoDto>(entidade);
        }

        public async Task<VinculoDto> BuscarVinculoPorMaquinaAsync(string maquina)
        {
            var entidade = await _repo.BuscarVinculoPorMaquinaAsync(maquina);
            return _mapper.Map<VinculoDto>(entidade);
        }

        public async Task<VinculoDto> BuscarVinculoPorLicencaIdAsync(int licencaId)
        {
            var entidade = await _repo.BuscarVinculoPorLicencaIdAsync(licencaId);
            return _mapper.Map<VinculoDto>(entidade);
        }

        public async Task<VinculoDto> BuscarVinculoPorNomeMaquinaAsync(string nomeMaquina)
        {
            var entidade = await _repo.BuscarVinculoPorNomeMaquinaAsync(nomeMaquina);
            return _mapper.Map<VinculoDto>(entidade);
        }

        public async Task<VinculoDto> CadastrarVinculoAsync(VinculoDto vinculoDto)
        {
            var entidade = _mapper.Map<Vinculo>(vinculoDto);
            var cadastrado = await _repo.CadastrarVinculoAsync(entidade);
            return _mapper.Map<VinculoDto>(cadastrado);
        }

        public async Task<VinculoDto> EditarVinculoAsync(VinculoDto vinculoDto)
        {
            var entidade = _mapper.Map<Vinculo>(vinculoDto);
            var editado = await _repo.EditarVinculoAsync(entidade);
            return _mapper.Map<VinculoDto>(editado);
        }

        public async Task<bool> ExcluirVinculoAsync(int id)
        {
            return await _repo.ExcluirVinculoAsync(id);
        }
    }
}
