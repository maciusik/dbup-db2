using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbUp.Support;

namespace DbUp.Db2
{
    public class Db2CommandSplitter
    {
        private readonly Func<string, SqlCommandReader> commandReaderFactory;

        public Db2CommandSplitter(char delimiter)
        {
            this.commandReaderFactory = scriptContents => new Db2CustomDelimiterCommandReader(scriptContents, delimiter);
        }

        /// <summary>
        /// Splits a script with multiple delimited commands into commands
        /// </summary>
        /// <param name="scriptContents"></param>
        /// <returns></returns>
        public IEnumerable<string> SplitScriptIntoCommands(string scriptContents)
        {
            using (var reader = commandReaderFactory(scriptContents))
            {
                var commands = new List<string>();
                reader.ReadAllCommands(c => commands.Add(c));
                return commands;
            }
        }
    }
}
