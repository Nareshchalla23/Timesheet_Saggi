using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TImesheet_demo2.Models
{
    public partial class Timesheet_Demo2Context : DbContext
    {
        public Timesheet_Demo2Context()
        {
        }

        public Timesheet_Demo2Context(DbContextOptions<Timesheet_Demo2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Timesheet> Timesheets { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=chandu;Database=Timesheet_Demo2;Persist Security Info=True;Trusted_Connection=True;Integrated Security=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK__AuditLog__5E5499A8E4740508");

                entity.Property(e => e.LogId).HasColumnName("LogID");

                entity.Property(e => e.ActionType).HasMaxLength(50);

                entity.Property(e => e.Details).HasMaxLength(1000);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AuditLogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AuditLogs__UserI__5AEE82B9");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.ProjectCode).HasMaxLength(50);

                entity.Property(e => e.ProjectManagerId).HasColumnName("ProjectManagerID");

                entity.Property(e => e.ProjectName).HasMaxLength(100);

                entity.HasOne(d => d.ProjectManager)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.ProjectManagerId)
                    .HasConstraintName("FK__Projects__Projec__5165187F");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.Property(e => e.ReportId).HasColumnName("ReportID");

                entity.Property(e => e.DateGenerated).HasColumnType("datetime");

                entity.Property(e => e.FiltersApplied).HasMaxLength(1000);

                entity.Property(e => e.GeneratedByUserId).HasColumnName("GeneratedByUserID");

                entity.Property(e => e.ReportType).HasMaxLength(50);

                entity.HasOne(d => d.GeneratedByUser)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.GeneratedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reports__Generat__5812160E");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<Timesheet>(entity =>
            {
                entity.Property(e => e.TimesheetId).HasColumnName("TimesheetID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ManagerComments).HasMaxLength(500);

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Timesheets)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Timesheet__Proje__5535A963");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Timesheets)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Timesheet__UserI__5441852A");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.EmployeeId, "UQ__Users__7AD04FF07AFA3D11")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Users__A9D1053432D86BD9")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(50)
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PasswordHash).HasMaxLength(255);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Users__RoleID__4E88ABD4");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
