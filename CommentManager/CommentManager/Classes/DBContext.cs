using CommentManager.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CommentManager.Classes
{
    /// <summary>
    /// Database class
    /// </summary>
    public class DBContext : DbContext
    {
        /// <summary>
        /// Table
        /// </summary>
        public DbSet<Comment> Comment { get; set; }

        /// <summary>
        /// Database connection config method
        /// </summary>
        /// <param name="optionsBuilder">Builder for customization</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "CommentManager.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }
    }
}