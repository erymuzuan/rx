using System.Data.SqlClient;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlRepository.Extensions
{
    public static class SqlExceptionExtension
    {
        public static BuildError ToBuildError(this SqlException exception)
        {
            return new BuildError(exception.Message, exception.LineNumber, exception.Procedure)
            {
                FileName = exception.Source
            };
        }
    }
}
