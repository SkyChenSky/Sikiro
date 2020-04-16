using System;
using System.Linq;
using Sikiro.Chloe.Cap.ConnectionParser.Sprache;

namespace Sikiro.Chloe.Cap.ConnectionParser
{
    internal interface IConnectionStringParser
    {
        ConnectionConfiguration Parse(string connectionString);
    }

    internal class ConnectionStringParser : IConnectionStringParser
    {
        public ConnectionConfiguration Parse(string connectionString)
        {
            try
            {
                var updater = ConnectionStringGrammar.ParseConnectionString(connectionString);
                var connectionConfiguration = updater.Aggregate(new ConnectionConfiguration(), (current, updateFunction) => updateFunction(current));
                connectionConfiguration.Validate();
                return connectionConfiguration;
            }
            catch (ParseException parseException)
            {
                throw new Exception($"Connection String {parseException.Message}");
            }
        }
    }
}