using CleanArchitecture.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Persistance.Context
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyReference).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entires = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entires)
            {
                if (entry.State == EntityState.Added)
                    entry.Property(p => p.CreatedDate)
                        .CurrentValue = DateTime.UtcNow;

                if (entry.State == EntityState.Modified)
                    entry.Property(p => p.UpdatedDate)
                        .CurrentValue = DateTime.UtcNow;
            }
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
