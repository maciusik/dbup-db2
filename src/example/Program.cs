using DbUp;
using DbUp.Db2;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace S2P.db.startup
{
    internal class Program
    {
        private static IConfiguration? _configration;
        private static int Main(string[] args)
        {
            _configration = GetConfigurationBuilder().Build();
            var loggerConfiguration = new LoggerConfiguration().ReadFrom.Configuration(_configration);
            var connection = GetConnectionString("DefaultConnection");
            var clientInstances = GetClientInstances("ScriptsFolders");

            Log.Logger = loggerConfiguration.CreateLogger();
            Log.Debug($"{"Running enviroment: " + string.Join(", ", clientInstances)}");

            //EnsureDatabase.For.Db2Database(connection);
            var upgradeEngineBuilder =
                DeployChanges.To.Db2Database(connection,"migration", ';')
                    .WithScriptsFromFileSystem("Scripts");

            if (!HasTransactionDisabled())
                upgradeEngineBuilder = upgradeEngineBuilder.WithTransaction();

            foreach (var item in clientInstances.Where(folderName => Directory.Exists(Path.Combine(Environment.CurrentDirectory, folderName!))))
                upgradeEngineBuilder.WithScriptsFromFileSystem(item);

            var upgradeEngine = upgradeEngineBuilder.WithVariablesDisabled().Build();

            var result = upgradeEngine.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Log.Debug($"{result.Error}");
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Log.Debug($"{"Success!"}");
            Console.ResetColor();
            return 0;
        }

        private static string? GetConnectionString(string name)
        {
            return GetConfigurationBuilder().Build().GetConnectionString(name);
        }

        //private static string GetClientInstance(string name)
        //{
        //    return GetConfigurationBuilder().Build().GetSection(name).Value;
        //}

        private static IEnumerable<string?> GetClientInstances(string name)
        {
            return _configration!.GetSection(name).GetChildren().Select(x => x.Value);
        }


        
        private static bool HasTransactionDisabled()
            => _configration!.GetSection("TransactionDisabled").Get<bool>();

        private static IConfigurationBuilder GetConfigurationBuilder()
        {
            var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            return new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                            .AddEnvironmentVariables();
        }
    }
}
