using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace core.runClient.DataEntities {
    public partial class runClientDbContext : DbContext {
        public runClientDbContext(DbContextOptions<runClientDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Jobs>(entity => {
                entity.Property(e => e.Id).HasColumnType("integer");

                entity.Property(e => e.CreateDate)
                    .IsRequired()
                    .HasColumnType("datetime");




                entity.Property(e => e.TestId).HasColumnType("integer");

                entity.Property(e => e.TestType).HasColumnType("tinyint");
            });

            modelBuilder.Entity<JobsTask>(entity => {
                entity.ToTable("JobsTask");

                entity.Property(e => e.Id).HasColumnType("integer");

                entity.Property(e => e.JobId).HasColumnType("integer");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");

                entity.Property(e => e.CaseFilePath)
                    .IsRequired()
                    .HasColumnType("nvarchar(500)");

                entity.Property(e => e.ResultPath)
                    .HasColumnType("nvarchar(500)");

                entity.Property(e => e.Device)
                    .HasColumnType("nvarchar(50)");


                entity.Property(e => e.ExecuteScript)
                    .IsRequired()
                    .HasColumnType("nvarchar(500)");


                entity.Property(e => e.Param)
                .HasColumnType("nvarchar(500)");



                entity.Property(e => e.RunDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.RunStatus).IsRequired().HasColumnType("tinyint");

                entity.Property(e => e.Result)
                    .HasColumnType("ntext");



                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobsTask)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SmokeTest>(entity => {
                entity.Property(e => e.Id).HasColumnType("integer");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasColumnType("nvarchar(500)");

                entity.Property(e => e.ExecuteScript)
                    .HasColumnType("nvarchar(500)");



                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");
            });
        }

        public virtual DbSet<Jobs> Jobs { get; set; }
        public virtual DbSet<JobsTask> JobsTask { get; set; }
        public virtual DbSet<SmokeTest> SmokeTest { get; set; }
    }
}