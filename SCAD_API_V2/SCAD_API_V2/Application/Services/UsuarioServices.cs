using AutoMapper;
using SCAD_API_V2.Application.DTO;
using SCAD_API_V2.Application.Interfaces;
using SCAD_API_V2.Domain.Entities;

namespace SCAD_API_V2.Application.Services
{
    public class UsuarioServices : IUsuarioServices
    {
        private readonly IUsuarioRepository _repo;
        private readonly IMapper _mapper;

        public UsuarioServices(IUsuarioRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDto>> ListarUsuariosAsync()
        {
            var entidades = await _repo.ListarUsuariosAsync();
            return _mapper.Map<List<UsuarioDto>>(entidades);
        }

        public async Task<UsuarioDto> BuscarUsuarioPorIdAsync(int usuarioId)
        {
            var entidade = await _repo.BuscarUsuarioPorIdAsync(usuarioId);
            return _mapper.Map<UsuarioDto>(entidade);
        }

        public async Task<UsuarioDto> BuscarUsuarioPorEmailAsync(string email)
        {
            var entidade = await _repo.BuscarUsuarioPorEmailAsync(email);
            return _mapper.Map<UsuarioDto>(entidade);
        }

        public async Task<UsuarioDto> BuscarUsuarioPorNomeAsync(string nome)
        {
            var entidade = await _repo.BuscarUsuarioPorNomeAsync(nome);
            return _mapper.Map<UsuarioDto>(entidade);
        }

        public async Task<UsuarioDto> CadastrarUsuarioAsync(UsuarioDto usuarioDto)
        {
            var entidade = _mapper.Map<Usuario>(usuarioDto);
            var cadastrado = await _repo.CadastrarUsuarioAsync(entidade);
            return _mapper.Map<UsuarioDto>(cadastrado);
        }

        public async Task<UsuarioDto> EditarUsuarioAsync(UsuarioDto usuarioDto)
        {
            var entidade = _mapper.Map<Usuario>(usuarioDto);
            var editado = await _repo.EditarUsuarioAsync(entidade);
            return _mapper.Map<UsuarioDto>(editado);
        }

        public async Task<bool> ExcluirUsuarioAsync(int usuarioId)
        {
            return await _repo.ExcluirUsuarioAsync(usuarioId);
        }
    }
}
