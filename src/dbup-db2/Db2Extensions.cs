using DbUp.Builder;
using DbUp.Engine.Transactions;
using System.Linq;
using System;

namespace DbUp.Db2
{

#pragma warning disable IDE0060 // Remove unused parameter - The "SupportedDatabases" parameter is never used.
    public static class Db2Extensions
    {
       

        /// <summary>
        /// Create an upgrader for Db2 databases that uses the <c>/</c> character as the delimiter between statements.
        /// </summary>
        /// <param name="supported">Fluent helper type.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>
        /// A builder for a database upgrader designed for Db2 databases.
        /// </returns>
        public static UpgradeEngineBuilder Db2DatabaseWithDefaultDelimiter(this SupportedDatabases supported, string connectionString)
            => Db2Database(supported, connectionString, '/');

        /// <summary>
        /// Create an upgrader for Db2 databases that uses the <c>;</c> character as the delimiter between statements.
        /// </summary>
        /// <param name="supported">Fluent helper type.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>
        /// A builder for a database upgrader designed for Db2 databases.
        /// </returns>
        public static UpgradeEngineBuilder Db2DatabaseWithSemicolonDelimiter(this SupportedDatabases supported, string connectionString)
            => Db2Database(supported, connectionString, ';');

        /// <summary>
        /// Create an upgrader for Db2 databases.
        /// </summary>
        /// <param name="supported">Fluent helper type.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="delimiter">Character to use as the delimiter between statements.</param>
        /// <returns>
        /// A builder for a database upgrader designed for Db2 databases.
        /// </returns>
        public static UpgradeEngineBuilder Db2Database(this SupportedDatabases supported, string connectionString, char delimiter)
        {
            foreach (var pair in connectionString.Split(';').Select(s => s.Split('=')).Where(pair => pair.Length == 2).Where(pair => pair[0].ToLower() == "database"))
            {
                return Db2Database(new Db2ConnectionManager(connectionString, new Db2CommandSplitter(delimiter)), pair[1]);
            }

            return Db2Database(new Db2ConnectionManager(connectionString, new Db2CommandSplitter(delimiter)));
        }

        /// <summary>
        /// Create an upgrader for Db2 databases.
        /// </summary>
        /// <param name="supported">Fluent helper type.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="delimiter">Character to use as the delimiter between statements.</param>
        /// <returns>
        /// A builder for a database upgrader designed for Db2 databases.
        /// </returns>
        public static UpgradeEngineBuilder Db2Database(this SupportedDatabases supported, string connectionString, string schema, char delimiter)
        {
            return Db2Database(new Db2ConnectionManager(connectionString, new Db2CommandSplitter(delimiter)), schema);
        }

        /// <summary>
        /// Creates an upgrader for Db2 databases.
        /// </summary>
        /// <param name="supported">Fluent helper type.</param>
        /// <param name="connectionString">Db2 database connection string.</param>
        /// <param name="schema">Which Db2 schema to check for changes</param>
        /// <returns>
        /// A builder for a database upgrader designed for Db2 databases.
        /// </returns>


        /// <summary>
        /// Creates an upgrader for Db2 databases.
        /// </summary>
        /// <param name="supported">Fluent helper type.</param>
        /// <param name="connectionManager">The <see cref="Db2ConnectionManager"/> to be used during a database upgrade.</param>
        /// <returns>
        /// A builder for a database upgrader designed for Db2 databases.
        /// </returns>
        public static UpgradeEngineBuilder Db2Database(this SupportedDatabases supported, IConnectionManager connectionManager)
        {
            return Db2Database(connectionManager);
        }

        /// <summary>
        /// Creates an upgrader for Db2 databases.
        /// </summary>
        /// <param name="connectionManager">The <see cref="Db2ConnectionManager"/> to be used during a database upgrade.</param>
        /// <returns>
        /// A builder for a database upgrader designed for Db2 databases.
        /// </returns>
        public static UpgradeEngineBuilder Db2Database(IConnectionManager connectionManager)
        {
            return Db2Database(connectionManager, null);
        }

        /// <summary>
        /// Creates an upgrader for Db2 databases.
        /// </summary>
        /// <param name="connectionManager">The <see cref="Db2ConnectionManager"/> to be used during a database upgrade.</param>
        /// /// <param name="schema">Which Db2 schema to check for changes</param>
        /// <returns>
        /// A builder for a database upgrader designed for Db2 databases.
        /// </returns>
        public static UpgradeEngineBuilder Db2Database(IConnectionManager connectionManager, string schema)
        {
            var builder = new UpgradeEngineBuilder();
            builder.Configure(c => c.ConnectionManager = connectionManager);
            builder.Configure(c => c.ScriptExecutor = new Db2ScriptExecutor(() => c.ConnectionManager, () => c.Log, null, () => c.VariablesEnabled, c.ScriptPreprocessors, () => c.Journal));
            builder.Configure(c => c.Journal = new Db2TableJournal(() => c.ConnectionManager, () => c.Log, schema, "schemaversions"));
            builder.WithPreprocessor(new Db2Preprocessor());
            return builder;
        }

        /// <summary>
        /// Tracks the list of executed scripts in an Db2 table.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="schema">The schema.</param>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static UpgradeEngineBuilder JournalToDb2Table(this UpgradeEngineBuilder builder, string schema, string table)
        {
            builder.Configure(c => c.Journal = new Db2TableJournal(() => c.ConnectionManager, () => c.Log, schema, table));
            return builder;
        }
    }
}

