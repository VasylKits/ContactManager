using ContactManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Db;

public class ContactContext : DbContext
{
    public ContactContext(DbContextOptions<ContactContext> options) : base(options)
    {
    }

    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Married)
                .IsRequired();

            entity.HasIndex(e => e.Phone)
                .IsUnique();

            entity.Property(c => c.Salary)
                .HasColumnType("decimal(10, 2)");
        });
    }
}