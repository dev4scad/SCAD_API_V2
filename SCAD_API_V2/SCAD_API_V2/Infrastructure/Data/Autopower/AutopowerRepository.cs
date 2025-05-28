using Microsoft.Extensions.Options;
using SCAD_API_V2.Infrastructure.Data.Base;

namespace SCAD_API_V2.Infrastructure.Data.Autopower;

public class AutopowerClienteRepository : ClienteRepositoryBase
{
    public AutopowerClienteRepository(IOptions<ConnectionString> options)
        : base(options.Value.Autopower)
    { }
}

public class AutopowerLicencaRepository : LicencaRepositoryBase
{
    public AutopowerLicencaRepository(IOptions<ConnectionString> options)
        : base(options.Value.Autopower)
    { }
}

public class AutopowerUsuarioRepository : UsuarioRepositoryBase
{
    public AutopowerUsuarioRepository(IOptions<ConnectionString> options)
        : base(options.Value.Autopower)
    { }
}

public class AutopowerVinculoRepository : BaseVinculoRepository
{
    public AutopowerVinculoRepository(IOptions<ConnectionString> options)
        : base(options.Value.Autopower)
    { }
}
