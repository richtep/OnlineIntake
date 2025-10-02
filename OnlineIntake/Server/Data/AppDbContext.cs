using Microsoft.EntityFrameworkCore;
using OnlineIntake.Server.Data.Entities;


namespace OnlineIntake.Server.Data;


public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts => Set<Contact>();


    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Contact>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            e.Property(x => x.BirthNumber).HasMaxLength(20);
            e.Property(x => x.Email).HasMaxLength(254).IsRequired();
            e.Property(x => x.Nationality).HasMaxLength(100).IsRequired();
            e.Property(x => x.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            e.HasIndex(x => x.Email);
        });
    }
}