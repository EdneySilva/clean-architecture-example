using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using VaultInfrastructure.Data;

namespace VaultAppApi.Tests.Infrastructure
{
    internal class VaultApp<T> : WebApplicationFactory<T> where T : class
    {
        private IHostedService _deviceMonitorWorker;
        private string dbName;
        private IHost host;

        public VaultApp()
        {
            dbName = "VaultDbTest" + Guid.NewGuid().ToString().Split('-').First();
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbConnectionString));
                services.TryAddSingleton(new DbConnectionString($"server=localhost;initial catalog={dbName};user=sa;pwd=1234Test!;TrustServerCertificate=True"));
            });
            var host = base.CreateHost(builder);
            using(SqlConnection sqlConnection = new SqlConnection("server=localhost;initial catalog=master;user=sa;pwd=1234Test!;TrustServerCertificate=True"))
            {
                sqlConnection.Open();
                using (var command = new SqlCommand(File.ReadAllText("./infrastructure/dbcreate.sql").Replace("VaultDbTest", dbName), sqlConnection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                using (var command = new SqlCommand(File.ReadAllText("./infrastructure/dbinit.sql").Replace("VaultDbTest", dbName), sqlConnection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
            return this.host = host;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            using (SqlConnection sqlConnection = new SqlConnection("server=localhost;initial catalog=master;user=sa;pwd=1234Test!;TrustServerCertificate=True"))
            {
                sqlConnection.Open();
                using (var command = new SqlCommand($@"
USE MASTER
DECLARE @kill varchar(8000) = '';  
SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'  
FROM sys.dm_exec_sessions
WHERE database_id  = db_id('{dbName}')
EXEC(@kill)",
sqlConnection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
            using (SqlConnection sqlConnection = new SqlConnection("server=localhost;initial catalog=master;user=sa;pwd=1234Test!;TrustServerCertificate=True"))
            {
                sqlConnection.Open();
                using (var command = new SqlCommand($@"
USE MASTER
DROP DATABASE {dbName}",
sqlConnection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }
    }
}
