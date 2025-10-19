using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Entities.Classification;
using Microsoft.EntityFrameworkCore;

namespace Alctel.CRM.Context.InMemory.Context;

public class CRMContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<AccessProfile> AccessProfiles { get; set; }
    public DbSet<ActionPermission> ActionPermissions { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<ServiceUnit> ServiceUnits { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<ServiceLevel> ServiceLevels { get; set; }
    public DbSet<DemandType> DemandTypes { get; set; }
    public DbSet<ReasonList> ReasonLists { get; set; }
    public DbSet<Reason> Reasons { get; set; }
    //public DbSet<AccessProfileModule> AccessProfileModules { get; set; }

    public CRMContext(DbContextOptions<CRMContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //modelBuilder.Entity<AccessProfile>()
        //    .HasKey(x => x.Id);

        //modelBuilder.Entity<Module>()
        //    .HasKey(x => x.Id);

        //modelBuilder.Entity<Module>()
        //    .HasOne(e => e.AccessProfile)
        //    .WithMany(e => e.Modules)
        //    .HasForeignKey(e => e.AccessProfileId)
        //    .HasPrincipalKey(e => e.Id);


    }
}