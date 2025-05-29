using SCAD_API_V2.Domain.Enums;

namespace SCAD_API_V2.Application.Http
{
    public interface ICurrentDatabase
    {
        Database Database { get; }
    }

    public class CurrentDatabase : ICurrentDatabase
    {
        public Database Database { get; }

        public CurrentDatabase(IHttpContextAccessor http)
        {
            var path = http.HttpContext?.Request.Path.Value?.ToLower() ?? "";
            Database = path.Contains("autohidro")
                       ? Database.Autohidro
                       : Database.Autopower;
        }
    }
}
