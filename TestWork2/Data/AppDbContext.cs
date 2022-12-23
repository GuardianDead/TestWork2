using Microsoft.EntityFrameworkCore;
using TestWork2.Data.Models;

namespace TestWork2.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Models.File> Files { get; set; }
    public DbSet<FileValue> FileValues { get; set; }
    public DbSet<FileResult> FileResults { get; set; }
}