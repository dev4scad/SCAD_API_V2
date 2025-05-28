using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SCAD_API_V2.Application.DTO;
using SCAD_API_V2.Application.Interfaces;
using SCAD_API_V2.Domain.Entities;
using SCAD_API_V2.Domain.Interfaces;

namespace SCAD_API_V2.Application.Services.Base
{

    public abstract class ClienteServicesBase<TRepo> : IClienteServices
        where TRepo : IClienteInterface
    {
        protected readonly TRepo _repo;

        protected ClienteServicesBase(TRepo repo)
        {
            _repo = repo;
        }

        public virtual async Task<List<ClienteDto>> BuscarUsuariosAsync()
        {
            var entidades = await _repo.BuscarUsuariosAsync();
            return entidades.Select(MapToDto).ToList();
        }

        public virtual async Task<ClienteDto> BuscarUsuarioPorIdAsync(int usuarioId)
        {
            var entidade = await _repo.BuscarUsuarioPorIdAsync(usuarioId);
            return entidade == null ? null : MapToDto(entidade);
        }

        public virtual async Task<ClienteDto> BuscarUsuarioPorCPF_CNPJAsync(string cpf_cnpj)
        {
            var entidade = await _repo.BuscarUsuarioPorCPF_CNPJAsync(cpf_cnpj);
            return entidade == null ? null : MapToDto(entidade);
        }

        public virtual async Task<ClienteDto> BuscarUsuarioPorEmailAsync(string email)
        {
            var entidade = await _repo.BuscarUsuarioPorEmailAsync(email);
            return entidade == null ? null : MapToDto(entidade);
        }

        public virtual async Task<List<ClienteDto>> BuscarUsuarioPorNomeAsync(string nome)
        {
            var lista = await _repo.BuscarUsuarioPorNomeAsync(nome);
            return lista.Select(MapToDto).ToList();
        }

        public virtual async Task<List<ClienteDto>> BuscarUsuarioPorTelefoneAsync(string telefone)
        {
            var lista = await _repo.BuscarUsuarioPorTelefoneAsync(telefone);
            return lista.Select(MapToDto).ToList();
        }

        public virtual async Task<List<ClienteDto>> CriarUsuarioAsync(ClienteDto clienteDto)
        {
            var entidade = new Cliente
            {
                CPF_CNPJ = clienteDto.CPF_CNPJ,
                Email = clienteDto.Email,
                Nome = clienteDto.Nome,
                NomeEmpresa = clienteDto.NomeEmpresa,
                Telefone = clienteDto.Telefone,
                DataCadastro = clienteDto.DataCadastro,
                DataExpira = clienteDto.DataExpira
            };
            var criados = await _repo.CriarUsuarioAsync(entidade);
            return criados.Select(MapToDto).ToList();
        }

        public virtual async Task<ClienteDto> EditarUsuarioAsync(ClienteDto clienteDto)
        {
            var entidade = new Cliente
            {
                CPF_CNPJ = clienteDto.CPF_CNPJ,
                Email = clienteDto.Email,
                Nome = clienteDto.Nome,
                NomeEmpresa = clienteDto.NomeEmpresa,
                Telefone = clienteDto.Telefone,
                DataExpira = clienteDto.DataExpira
            };
            var editado = await _repo.EditarUsuarioAsync(entidade);
            return editado == null ? null : MapToDto(editado);
        }

        public virtual Task<bool> ExcluirUsuarioAsync(int usuarioId)
        {
            return _repo.ExcluirUsuarioAsync(usuarioId);
        }

        private static ClienteDto MapToDto(Cliente e) => new ClienteDto
        {
            CPF_CNPJ = e.CPF_CNPJ,
            Email = e.Email,
            Nome = e.Nome,
            NomeEmpresa = e.NomeEmpresa,
            Telefone = e.Telefone,
            DataCadastro = e.DataCadastro,
            DataExpira = e.DataExpira
        };
    }
}
