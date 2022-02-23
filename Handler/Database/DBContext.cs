using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Handler
{
    // docker run --name some-postgres -e POSTGRES_PASSWORD=P@ssw0rd -p 5432:5432 -d postgres
    [Table("items")]
    public class DBLogItem
    {
        [Key] public int id { get; set; }

        public DateTime dt   { get; set; }
        public string   host { get; set; }
        public string   data { get; set; }
    }

    public class DBContext : DbContext
    {
        public DbSet<DBLogItem> Items { get; set; }

        readonly string connectionString;

        public DBContext(string connectionString) => this.connectionString = connectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(connectionString);
    }
}