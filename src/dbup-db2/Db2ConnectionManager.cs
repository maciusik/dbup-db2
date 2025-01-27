using DbUp.Engine.Transactions;
using System.Collections.Generic;
using System;
using IBM.Data.Db2;

namespace DbUp.Db2
{
    public class Db2ConnectionManager : DatabaseConnectionManager
    {
        private readonly Db2CommandSplitter commandSplitter;

        /// <summary>
        /// Creates a new Db2 database connection manager.
        /// </summary>
        /// <param name="connectionString">The Db2 connection string.</param>
        /// <param name="commandSplitter">A class that splits a string into individual Db2 SQL statements.</param>
        public Db2ConnectionManager(string connectionString, Db2CommandSplitter commandSplitter)
            : base(new DelegateConnectionFactory(l => new DB2Connection(connectionString)))
        {
            this.commandSplitter = commandSplitter;
        }

        public override IEnumerable<string> SplitScriptIntoCommands(string scriptContents)
        {
            var scriptStatements = commandSplitter.SplitScriptIntoCommands(scriptContents);
            return scriptStatements;
        }
    }
}
