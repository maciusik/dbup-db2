using DbUp.Engine.Output;
using DbUp.Engine.Transactions;
using DbUp.Engine;
using DbUp.Support;
using System.Collections.Generic;
using System;
using IBM.Data.Db2;

namespace DbUp.Db2
{
    public class Db2ScriptExecutor : ScriptExecutor
    {
        /// <summary>
        /// Initializes an instance of the <see cref="Db2ScriptExecutor"/> class.
        /// </summary>
        /// <param name="connectionManagerFactory"></param>
        /// <param name="log">The logging mechanism.</param>
        /// <param name="schema">The schema that contains the table.</param>
        /// <param name="variablesEnabled">Function that returns <c>true</c> if variables should be replaced, <c>false</c> otherwise.</param>
        /// <param name="scriptPreprocessors">Script Preprocessors in addition to variable substitution</param>
        /// <param name="journalFactory">Database journal</param>
        public Db2ScriptExecutor(Func<IConnectionManager> connectionManagerFactory, Func<IUpgradeLog> log, string schema, Func<bool> variablesEnabled,
            IEnumerable<IScriptPreprocessor> scriptPreprocessors, Func<IJournal> journalFactory)
            : base(connectionManagerFactory, new Db2ObjectParser(), log, schema, variablesEnabled, scriptPreprocessors, journalFactory)
        {
        }

        protected override string GetVerifySchemaSql(string schema)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Executes the specified script against a database at a given connection string.
        /// </summary>
        /// <param name="script">The script.</param>
        public override void Execute(SqlScript script)
        {
            Execute(script, null);
        }

        protected override void ExecuteCommandsWithinExceptionHandler(int index, SqlScript script, Action executeCommand)
        {
            try
            {
                executeCommand();
            }
            catch (DB2Exception exception)
            {
                Log().LogInformation("Db2 exception has occurred in script: '{0}'", script.Name);
                // Db2Exception.Number is the actual oracle error code
                Log().LogError("Script block number: {0}; Db2 error code: {1}; Message: {2}", index, exception.ErrorCode, exception.Message);
                Log().LogError(exception.ToString());
                throw;
            }
        }
    }
}

