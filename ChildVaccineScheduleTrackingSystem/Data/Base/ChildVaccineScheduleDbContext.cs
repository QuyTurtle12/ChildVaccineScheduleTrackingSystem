using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Base
{
    public class ChildVaccineScheduleDbContext : DbContext
    {
        public ChildVaccineScheduleDbContext()
        {
        }

        public ChildVaccineScheduleDbContext(DbContextOptions<ChildVaccineScheduleDbContext> options) : base(options) { }

        public virtual DbSet<Appointment> Appointments => Set<Appointment>();
        public virtual DbSet<Child> Children => Set<Child>();
        public virtual DbSet<Feedback> Feedbacks => Set<Feedback>();
        public virtual DbSet<Notification> Notifications => Set<Notification>();
        public virtual DbSet<Package> Packages => Set<Package>();
        public virtual DbSet<PackageVaccine> PackageVaccines => Set<PackageVaccine>();
        public virtual DbSet<Payment> Payments  => Set<Payment>();
        public virtual DbSet<Role> Roles => Set<Role>();
        public virtual DbSet<User> Users => Set<User>();
        public virtual DbSet<Vaccine> Vaccines => Set<Vaccine>();
        public virtual DbSet<VaccineRecord> VaccineRecords => Set<VaccineRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Package>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Feedbacks)
                .WithOne(fb => fb.User)
                .HasForeignKey(fb => fb.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Child>()
                .HasOne(c => c.User)
                .WithMany(u => u.Children)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Notification>()
                .HasOne(nf => nf.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(nf => nf.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Appointment)
                .WithMany(ap => ap.Payments)
                .HasForeignKey(p => p.AppointmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
                .HasMany(ap => ap.Notifications)
                .WithOne(nf => nf.Appointment)
                .HasForeignKey(nf => nf.AppointmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
                .HasOne(ap => ap.User)
                .WithMany(u => u.Appointments)
                .HasForeignKey(ap => ap.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Feedback>()
                .HasOne(fb => fb.Appointment)
                .WithMany(ap => ap.Feedbacks)
                .HasForeignKey(fb => fb.AppointmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Package>()
                .HasOne(pa => pa.Appointment)
                .WithMany(ap => ap.Packages)
                .HasForeignKey(pa => pa.AppointmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Package>()
                .HasMany(pa => pa.PackageVaccines)
                .WithOne(pv => pv.Package)
                .HasForeignKey(pv => pv.PackageId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Vaccine>()
                .HasMany(v => v.PackageVaccines)
                .WithOne(pv => pv.Vaccine)
                .HasForeignKey(pv => pv.VaccineId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Vaccine>()
                .HasMany (v => v.VaccineRecords)
                .WithOne(vr => vr.Vaccine)
                .HasForeignKey(vr => vr.VaccineId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<VaccineRecord>()
                .HasOne (vr => vr.Child)
                .WithMany(c => c.VaccineRecords)
                .HasForeignKey(vr => vr.ChildId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
