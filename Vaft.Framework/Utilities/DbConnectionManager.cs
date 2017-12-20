using System;
using System.Configuration;
using System.Data.OracleClient;
using System.Data.SqlClient;
using Vaft.Framework.Settings;

namespace Vaft.Framework.Utilities
{
    public static class DbConnectionManager
    {
        // <summary>Executes SQL query in DB.</summary>
        /// <param name="sqlStatement">SQL statement.</param>
        public static object ExecuteSql(string sqlStatement)
        {
            string dbType = Config.Settings.RuntimeSettings.DatabaseType;
            switch (dbType)
            {
                case "mssql":
                case "MsSql":
                    return ExecuteMssql(sqlStatement);
                case "oracle":
                case "Oracle":
                    return ExecuteOracle(sqlStatement);
                default:
                    throw new ArgumentOutOfRangeException("'DatabaseType', Value: " + dbType + ". Please make sure that you have specified correct database type.");
            }
        }

        private static Object ExecuteMssql(string sqlStatement)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["mssql"].ConnectionString;

            var sqlConnection = new SqlConnection
            {
                ConnectionString = connectionString
            };

            var cmd = new SqlCommand
            {
                CommandType = System.Data.CommandType.Text,
                CommandText = sqlStatement,
                Connection = sqlConnection
            };

            sqlConnection.Open();
            Object returnValue = cmd.ExecuteScalar();
            sqlConnection.Close();

            return returnValue;
        }

        private static Object ExecuteOracle(string sqlStatement)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["oracle"].ConnectionString;

            var oraConnection = new OracleConnection
            {
                ConnectionString = connectionString
            };

            var cmd = new OracleCommand
            {
                CommandType = System.Data.CommandType.Text,
                CommandText = sqlStatement,
                Connection = oraConnection
            };

            oraConnection.Open();
            Object returnValue = cmd.ExecuteScalar();
            oraConnection.Close();

            return returnValue;
        }
    }
}
