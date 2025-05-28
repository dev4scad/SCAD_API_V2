using Microsoft.Extensions.Options;
using SCAD_API_V2.Infrastructure.Data.Base;

namespace SCAD_API_V2.Infrastructure.Data.Autohidro
{
    public class AutohidroClienteRepository : ClienteRepositoryBase
    {
        public AutohidroClienteRepository(IOptions<ConnectionString> options)
            : base(options.Value.Autohidro)
        { }
    }

    public class AutohidroLicencaRepository : LicencaRepositoryBase
    {
        public AutohidroLicencaRepository(IOptions<ConnectionString> options)
            : base(options.Value.Autohidro)
        { }
    }

    public class AutohidroUsuarioRepository : UsuarioRepositoryBase
    {
        public AutohidroUsuarioRepository(IOptions<ConnectionString> options)
            : base(options.Value.Autohidro)
        { }
    }

    public class AutohidroVinculoRepository : BaseVinculoRepository
    {
        public AutohidroVinculoRepository(IOptions<ConnectionString> options)
            : base(options.Value.Autohidro)
        { }
    }
}
