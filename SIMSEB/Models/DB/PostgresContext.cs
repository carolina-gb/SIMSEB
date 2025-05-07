using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SIMSEB.Models.DB;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EmergenciesType> EmergenciesTypes { get; set; }

    public virtual DbSet<Emergency> Emergencies { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Infraction> Infractions { get; set; }

    public virtual DbSet<InfractionsType> InfractionsTypes { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<ReportsStage> ReportsStages { get; set; }

    public virtual DbSet<ReportsTracking> ReportsTrackings { get; set; }

    public virtual DbSet<ReportsType> ReportsTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersNotice> UsersNotices { get; set; }

    public virtual DbSet<UsersStatus> UsersStatuses { get; set; }

    public virtual DbSet<UsersType> UsersTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;Port=3434;UserId=postgres;Password=hoodSystem");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmergenciesType>(entity =>
        {
            entity.HasKey(e => e.EmergencyTypeId).HasName("emergencies_types_pkey");

            entity.ToTable("emergencies_types");

            entity.Property(e => e.EmergencyTypeId).HasColumnName("emergency_type_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ShowName)
                .HasMaxLength(100)
                .HasColumnName("show_name");
        });

        modelBuilder.Entity<Emergency>(entity =>
        {
            entity.HasKey(e => e.EmergencyId).HasName("emergencies_pkey");

            entity.ToTable("emergencies");

            entity.Property(e => e.EmergencyId).HasColumnName("emergency_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Type).WithMany(p => p.Emergencies)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("emergencies_type_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Emergencies)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("emergencies_user_id_fkey");
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("files_pkey");

            entity.ToTable("files");

            entity.Property(e => e.FileId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("file_id");
            entity.Property(e => e.Path).HasColumnName("path");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasColumnName("type");
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("uploaded_at");
        });

        modelBuilder.Entity<Infraction>(entity =>
        {
            entity.HasKey(e => e.InfractionId).HasName("infractions_pkey");

            entity.ToTable("infractions");

            entity.Property(e => e.InfractionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("infraction_id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.InfractionNumber)
                .HasMaxLength(50)
                .HasColumnName("infraction_number");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Type).WithMany(p => p.Infractions)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("infractions_type_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Infractions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("infractions_user_id_fkey");
        });

        modelBuilder.Entity<InfractionsType>(entity =>
        {
            entity.HasKey(e => e.InfractionTypeId).HasName("infractions_types_pkey");

            entity.ToTable("infractions_types");

            entity.Property(e => e.InfractionTypeId).HasColumnName("infraction_type_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ShowName)
                .HasMaxLength(100)
                .HasColumnName("show_name");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("reports_pkey");

            entity.ToTable("reports");

            entity.Property(e => e.ReportId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("report_id");
            entity.Property(e => e.CaseNumber)
                .HasMaxLength(50)
                .HasColumnName("case_number");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EvidenceFileId).HasColumnName("evidence_file_id");
            entity.Property(e => e.RejectBy).HasColumnName("reject_by");
            entity.Property(e => e.RejectReason).HasColumnName("reject_reason");
            entity.Property(e => e.StageId).HasColumnName("stage_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.EvidenceFile).WithMany(p => p.Reports)
                .HasForeignKey(d => d.EvidenceFileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reports_evidence_file_id_fkey");

            entity.HasOne(d => d.RejectByNavigation).WithMany(p => p.ReportRejectByNavigations)
                .HasForeignKey(d => d.RejectBy)
                .HasConstraintName("reports_reject_by_fkey");

            entity.HasOne(d => d.Stage).WithMany(p => p.Reports)
                .HasForeignKey(d => d.StageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reports_stage_id_fkey");

            entity.HasOne(d => d.Type).WithMany(p => p.Reports)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reports_type_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.ReportUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reports_user_id_fkey");
        });

        modelBuilder.Entity<ReportsStage>(entity =>
        {
            entity.HasKey(e => e.ReportStageId).HasName("reports_stages_pkey");

            entity.ToTable("reports_stages");

            entity.Property(e => e.ReportStageId).HasColumnName("report_stage_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ShowName)
                .HasMaxLength(100)
                .HasColumnName("show_name");
        });

        modelBuilder.Entity<ReportsTracking>(entity =>
        {
            entity.HasKey(e => e.ReportTrackingId).HasName("reports_trackings_pkey");

            entity.ToTable("reports_trackings");

            entity.Property(e => e.ReportTrackingId).HasColumnName("report_tracking_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.NewStageId).HasColumnName("new_stage_id");
            entity.Property(e => e.ReportId).HasColumnName("report_id");

            entity.HasOne(d => d.NewStage).WithMany(p => p.ReportsTrackings)
                .HasForeignKey(d => d.NewStageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reports_trackings_new_stage_id_fkey");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportsTrackings)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reports_trackings_report_id_fkey");
        });

        modelBuilder.Entity<ReportsType>(entity =>
        {
            entity.HasKey(e => e.ReportTypeId).HasName("reports_types_pkey");

            entity.ToTable("reports_types");

            entity.Property(e => e.ReportTypeId).HasColumnName("report_type_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ShowName)
                .HasMaxLength(100)
                .HasColumnName("show_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Identification)
                .HasMaxLength(255)
                .HasColumnName("identification");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PasswordHint)
                .HasMaxLength(56)
                .HasColumnName("password_hint");
            entity.Property(e => e.Status)
                .HasDefaultValue(1)
                .HasColumnName("status");
            entity.Property(e => e.TypeId)
                .HasDefaultValue(3)
                .HasColumnName("type_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(56)
                .HasColumnName("username");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_status_fkey");

            entity.HasOne(d => d.Type).WithMany(p => p.Users)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_type_id_fkey");
        });

        modelBuilder.Entity<UsersNotice>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("users_notices");

            entity.Property(e => e.UserNoticeId).HasColumnName("user_notice_id");
            entity.Property(e => e.UserOwnerId).HasColumnName("user_owner_id");

            entity.HasOne(d => d.UserNotice).WithMany()
                .HasForeignKey(d => d.UserNoticeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_notices_user_notice_id_fkey");

            entity.HasOne(d => d.UserOwner).WithMany()
                .HasForeignKey(d => d.UserOwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_notices_user_owner_id_fkey");
        });

        modelBuilder.Entity<UsersStatus>(entity =>
        {
            entity.HasKey(e => e.UserStatusId).HasName("users_statuses_pkey");

            entity.ToTable("users_statuses");

            entity.Property(e => e.UserStatusId).HasColumnName("user_status_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ShowName)
                .HasMaxLength(100)
                .HasColumnName("show_name");
        });

        modelBuilder.Entity<UsersType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("users_types_pkey");

            entity.ToTable("users_types");

            entity.Property(e => e.UserTypeId).HasColumnName("user_type_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ShowName)
                .HasMaxLength(100)
                .HasColumnName("show_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
