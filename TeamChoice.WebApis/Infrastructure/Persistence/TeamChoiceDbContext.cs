using Microsoft.EntityFrameworkCore;
using System;


namespace TeamChoice.WebApis.Infrastructure.Persistence;

public class TeamChoiceDbContext : DbContext
{
    public TeamChoiceDbContext(DbContextOptions<TeamChoiceDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 🔹 Apply Fluent Configurations automatically
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TeamChoiceDbContext).Assembly);

        // 🔹 Optional: default schema
        // modelBuilder.HasDefaultSchema("dbo");
    }
}
