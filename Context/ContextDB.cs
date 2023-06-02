using Microsoft.EntityFrameworkCore;
using ExoAPI.Models;
namespace ExoAPI.Context;
public class  ContextDB : DbContext
{
    public ContextDB(DbContextOptions options) : base(options) { }
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
}