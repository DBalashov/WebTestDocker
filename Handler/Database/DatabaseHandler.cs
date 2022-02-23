using System;
using System.Linq;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Handler
{
    public class DatabaseHandler : IDatabaseHandler
    {
        readonly DBContext                db;
        readonly ILogger<DatabaseHandler> logger;

        public bool Enabled => true;
        
        public DatabaseHandler(DBContext db, ILogger<DatabaseHandler> logger)
        {
            this.db     = db;
            this.logger = logger;
        }

        public void Put(DBLogItem item)
        {
            db.Items.Add(item);
            logger.LogInformation("Put result to database [{0}]", item.host);
            db.SaveChanges();
        }

        public DBLogItem[] GetLast(int n) => db.Items.OrderByDescending(p => p.dt).Take(n).ToArray();

        public void PrepareDatabase()
        {
            var myDatabaseName = new NpgsqlConnectionStringBuilder(db.Database.GetConnectionString()).Database;

            using (var connection = new NpgsqlConnection(new NpgsqlConnectionStringBuilder(db.Database.GetConnectionString()) { Database = "postgres" }.ToString()))
            {
                connection.Open();
                var oid = connection.ExecuteScalar<int>($"select oid from pg_catalog.pg_database where datname='{myDatabaseName}'");
                if (oid == 0)
                {
                    logger.LogInformation("Create database: {0}", myDatabaseName);
                    connection.Execute($"create database \"{myDatabaseName}\"");
                }
                else logger.LogInformation("Database already exists: {0}", myDatabaseName);
            }

            const string myTableName = "items";
            using (var connection = new NpgsqlConnection(db.Database.GetConnectionString()))
            {
                connection.Open();

                var tableName = connection.ExecuteScalar<string>($"select tablename from pg_catalog.pg_tables where tablename='{myTableName}'");
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    logger.LogInformation("Create table: {0}", "items");
                    connection.Execute($"create table {myTableName} (id serial, dt timestamp not null, host varchar(128) not null, data text not null)");
                }
                else logger.LogInformation("Table already exists: {0}", myTableName);
            }
        }
    }
}