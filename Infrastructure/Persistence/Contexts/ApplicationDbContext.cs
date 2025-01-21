using Application.Common.Interfaces;
using Application.Interfaces.Services.CurrentUserService;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;


namespace Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        public ApplicationDbContext(
            DbContextOptions options,
           // IOptions<OperationalStoreOptions> operationalStoreOptions,
            ICurrentUserService currentUserService) : base(options)
        {
            this._currentUserService = currentUserService;
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HealthTest> HealthTests { get; set; }
        public DbSet<MedicalHistory> MedicalHistory { get; set; }
        public DbSet<UploadFile> UploadFile { get; set; }
        public DbSet<UploadFileMorph> UploadFileMorph { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        public DbSet<CustomerMessage> CustomerMessages { get; set; }
     


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.Now;
                        entry.Entity.UpdatedBy = _currentUserService.UserId;
                        break;
                        //case EntityState.Deleted:
                        //    entry.Reload();
                        //    break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.Now;
                        entry.Entity.UpdatedBy = _currentUserService.UserId;
                        break;
                }
            }
            return base.SaveChanges();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            //builder.Entity<Portfolio>()
            // .HasOne(p => p.Student)
            // .WithMany(s => s.Portfolios)
            // .HasForeignKey(p => p.StudentId)
            // .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Portfolio>()
           .HasOne(p => p.Student) // A portfolio has one student
           .WithOne(s => s.Portfolios) // A student has one portfolio
           .HasForeignKey<Portfolio>(p => p.StudentId) // Set the foreign key in the Portfolio entity
           .OnDelete(DeleteBehavior.Cascade); // Specify the delete behavior (optional)

            base.OnModelCreating(builder);
        }




    }

}
