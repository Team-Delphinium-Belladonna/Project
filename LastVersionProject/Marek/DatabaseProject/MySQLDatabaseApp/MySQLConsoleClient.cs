namespace MySQLDatabaseApp
{
    using System;
    using System.Linq;

    using Markets.Data;
    using MySql.Data.MySqlClient;

    public class MySQLConsoleClient
    {
        public static void Main()
        {
            var sqlServerContext = new ChainOfSupermarketsContext();
            var mysqlDatabase = new MySQLDatabase(sqlServerContext);

            mysqlDatabase.Migrate();
        }
    }
}