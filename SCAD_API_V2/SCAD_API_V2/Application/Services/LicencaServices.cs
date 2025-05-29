using AutoMapper;
using SCAD_API_V2.Application.DTO;
using SCAD_API_V2.Application.Interfaces;
using SCAD_API_V2.Domain.Entities;
using SCAD_API_V2.Domain.Interfaces;

namespace SCAD_API_V2.Application.Services
{
    public class LicencaServices : ILicencaServices
    {
        private readonly ILicencaRepository _repo;
        private readonly IMapper _mapper;
        private readonly IClienteRepository _clienteRepository;

        public LicencaServices(ILicencaRepository repo, IMapper mapper, IClienteRepository clienteRepository)
        {
            _repo = repo;
            _mapper = mapper;
            _clienteRepository = clienteRepository;
        }

        public async Task<List<LicencaDto>> BuscarLicencasAsync()
        {
            var entidades = await _repo.BuscarLicencasAsync();
            return _mapper.Map<List<LicencaDto>>(entidades);
        }

        public async Task<LicencaDto> BuscarLicencaPorIdAsync(int licencaId)
        {
            var entidade = await _repo.BuscarLicencaPorIdAsync(licencaId);
            return _mapper.Map<LicencaDto>(entidade);
        }

        public async Task<LicencaDto> BuscarLicencaPorKeyAsync(string licencaKey)
        {
            var entidade = await _repo.BuscarLicencaPorKeyAsync(licencaKey);
            return _mapper.Map<LicencaDto>(entidade);
        }

        public async Task<List<LicencaDto>> BuscarLicencaPorClienteIdAsync(int clienteId)
        {
            var entidades = await _repo.BuscarLicencaPorClienteIdAsync(clienteId);
            return _mapper.Map<List<LicencaDto>>(entidades);
        }

        public async Task<List<LicencaDto>> CriarLicencaAsync(LicencaDto licencaDto)
        {
            var cliente = await _clienteRepository.BuscarClientePorIdAsync(licencaDto.ClienteId);
            if (cliente == null)
                throw new ArgumentException("Cliente não encontrado");

            string chaveUnica = GerarChaveUnica();

            var entidade = _mapper.Map<Licenca>(licencaDto);
            entidade.LicencaKey = chaveUnica; 
            entidade.Ativo = true;

            var criadas = await _repo.CriarLicencaAsync(entidade);

            return _mapper.Map<List<LicencaDto>>(criadas);
        }

        public async Task<LicencaDto> EditarLicencaAsync(LicencaDto licencaDto)
        {
            var entidade = _mapper.Map<Licenca>(licencaDto);
            var editada = await _repo.EditarLicencaAsync(entidade);
            return _mapper.Map<LicencaDto>(editada);
        }

        public async Task<bool> ExcluirLicencaAsync(int licencaId)
        {
            return await _repo.ExcluirLicencaAsync(licencaId);
        }


        private static string GerarChaveUnica()
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var randomBytes = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            var hash = sha256.ComputeHash(randomBytes);
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
                if ((i + 1) % 5 == 0 && i < hash.Length - 1)
                {
                    sb.Append("-");
                }
            }
            return sb.ToString();
        }
    }
}
