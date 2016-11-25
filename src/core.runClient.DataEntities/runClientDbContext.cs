using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace core.runClient.DataEntities {
    public partial class runClientDbContext : DbContext
    {
        public runClientDbContext(DbContextOptions<runClientDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SmokeTest>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("integer");

                entity.Property(e => e.ExecuteScript)
                    .IsRequired()
                    .HasColumnType("nvarchar(500)");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasColumnType("nvarchar(500)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");

                entity.Property(e => e.PassMatch).HasColumnType("nvarchar(50)");
            });

            modelBuilder.Entity<SmokeTestJob>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("integer");

                entity.Property(e => e.CreateDate)
                    .IsRequired()
                    .HasColumnType("datetime");

                entity.Property(e => e.SmokeId).HasColumnType("integer");

                entity.HasOne(d => d.Smoke)
                    .WithMany(p => p.SmokeTestJob)
                    .HasForeignKey(d => d.SmokeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SmokeTestJobTask>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("integer");

                entity.Property(e => e.Device).HasColumnType("nvarchar(50)");

                entity.Property(e => e.ExecuteScript)
                    .IsRequired()
                    .HasColumnType("nvarchar(500)");

                entity.Property(e => e.ExecuteScriptResult).HasColumnType("ntext");

                entity.Property(e => e.JobId).HasColumnType("integer");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");

                entity.Property(e => e.PassMatch).HasColumnType("nvarchar(50)");

                entity.Property(e => e.ResultPath).HasColumnType("nvarchar(500)");

                entity.Property(e => e.RunDate).HasColumnType("datetime");

                entity.Property(e => e.RunStatus).IsRequired().HasColumnType("tinyint");

                entity.Property(e => e.PackageName).HasColumnType("nvarchar(50)");

                entity.Property(e => e.InstallApkFile).HasColumnType("nvarchar(500)");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.SmokeTestJobTask)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        public virtual DbSet<SmokeTest> SmokeTest { get; set; }
        public virtual DbSet<SmokeTestJob> SmokeTestJob { get; set; }
        public virtual DbSet<SmokeTestJobTask> SmokeTestJobTask { get; set; }
    }
}