using DbUp.Engine.Output;
using DbUp.Engine.Transactions;
using DbUp.Support;
using System.Data;
using System.Globalization;
using System;

namespace DbUp.Db2
{
    public class Db2TableJournal : TableJournal
    {
        bool journalExists;
        /// <summary>
        /// Creates a new Db2 table journal.
        /// </summary>
        /// <param name="connectionManager">The Db2 connection manager.</param>
        /// <param name="logger">The upgrade logger.</param>
        /// <param name="schema">The name of the schema the journal is stored in.</param>
        /// <param name="table">The name of the journal table.</param>
        public Db2TableJournal(Func<IConnectionManager> connectionManager, Func<IUpgradeLog> logger, string schema, string table)
            : base(connectionManager, logger, new Db2ObjectParser(), schema, table)
        {
        }

        public static CultureInfo English = new CultureInfo("en-US", false);

        protected override string CreateSchemaTableSql(string quotedPrimaryKeyName)
        {
            var fqSchemaTableName = UnquotedSchemaTableName.ToUpper(English);
            var schema = SchemaTableSchema.ToUpper(English);
            return
                $@" CREATE TABLE {schema}.{fqSchemaTableName} 
                (
                    schemaversionid integer not null GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
                    scriptname varchar(512) NOT NULL,
                    applied TIMESTAMP NOT NULL,
                    CONSTRAINT PK_{fqSchemaTableName} PRIMARY KEY (schemaversionid) 
                )";
        }

        protected override string GetInsertJournalEntrySql(string scriptName, string applied)
        {
            var unquotedSchemaTableName = UnquotedSchemaTableName.ToUpper(English);
            var schema = SchemaTableSchema.ToUpper(English);
            return $"insert into {schema}.{unquotedSchemaTableName} (ScriptName, Applied) values (?, ?)";
        }

        protected override string GetJournalEntriesSql()
        {
            var unquotedSchemaTableName = UnquotedSchemaTableName.ToUpper(English);
            var schema = SchemaTableSchema.ToUpper(English);

            return $"select scriptname from {schema}.{unquotedSchemaTableName} order by scriptname";
        }

        protected override string DoesTableExistSql()
        {
            var unquotedSchemaTableName = UnquotedSchemaTableName.ToUpper(English);
            var schema = SchemaTableSchema.ToUpper(English);
            return $"SELECT 1 from SYSIBM.SYSTABLES where NAME = '{unquotedSchemaTableName}' and CREATOR = '{schema}'";
        }
         
        public override void EnsureTableExistsAndIsLatestVersion(Func<IDbCommand> dbCommandFactory)
        {
            if (!journalExists && !DoesTableExist(dbCommandFactory))
            {
                Log().LogInformation("Creating the {0} table", FqSchemaTableName);


                // We will never change the schema of the initial table create.
                using (var command = GetCreateTableCommand(dbCommandFactory))
                    command.ExecuteNonQuery();
                
                Log().LogInformation("The {0} table has been created", FqSchemaTableName);

                OnTableCreated(dbCommandFactory);
            }

            journalExists = true;
        }
    }
}

