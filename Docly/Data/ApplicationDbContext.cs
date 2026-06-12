using Microsoft.EntityFrameworkCore;
using Docly.Data.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<FavoriteDoctor> FavoriteDoctors { get; set; }
    public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<ChatSession> ChatSessions { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Attachment> Attachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Patients: PK, UNIQUE on Email and TaxId, and the date mapping for DateOfBirth.
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.Email).IsUnique();
            entity.HasIndex(p => p.TaxId).IsUnique();
            entity.Property(p => p.Email).HasMaxLength(256).IsRequired();
            entity.Property(p => p.PasswordHash).IsRequired();
            entity.Property(p => p.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(p => p.LastName).HasMaxLength(100).IsRequired();
            entity.Property(p => p.DateOfBirth).HasColumnType("date");
            entity.Property(p => p.TaxId).HasMaxLength(16).IsRequired();
        });

        // Doctors: PK, UNIQUE on Email, and basic column constraints.
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.HasIndex(d => d.Email).IsUnique();
            entity.Property(d => d.Email).HasMaxLength(256).IsRequired();
            entity.Property(d => d.PasswordHash).IsRequired();
            entity.Property(d => d.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(d => d.LastName).HasMaxLength(100).IsRequired();
            entity.Property(d => d.Specialization).HasMaxLength(150).IsRequired();
            entity.Property(d => d.Bio);
            entity.Property(d => d.IsActive).IsRequired();
        });

        // FavoriteDoctors: composite PK for the many-to-many join table and CASCADE on the join rows.
        modelBuilder.Entity<FavoriteDoctor>(entity =>
        {
            entity.HasKey(fd => new { fd.PatientId, fd.DoctorId });

            entity.HasOne(fd => fd.Patient)
                .WithMany(p => p.FavoriteDoctors)
                .HasForeignKey(fd => fd.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(fd => fd.Doctor)
                .WithMany(d => d.FavoriteDoctors)
                .HasForeignKey(fd => fd.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // DoctorAvailabilities: PK, doctor FK, and a check that the slot ends after it starts.
        modelBuilder.Entity<DoctorAvailability>(entity =>
        {
            entity.HasKey(da => da.Id);

            entity.HasOne(da => da.Doctor)
                .WithMany(d => d.Availabilities)
                .HasForeignKey(da => da.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(da => da.StartTime).HasColumnType("datetime");
            entity.Property(da => da.EndTime).HasColumnType("datetime");
            entity.ToTable(tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("CK_DoctorAvailabilities_TimeRange", "EndTime > StartTime");
            });
        });

        // Appointments: PK, patient/doctor FKs, and validation of time range and status values.
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(a => a.StartTime).HasColumnType("datetime");
            entity.Property(a => a.EndTime).HasColumnType("datetime");
            entity.ToTable(tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("CK_Appointments_TimeRange", "EndTime > StartTime");
                tableBuilder.HasCheckConstraint("CK_Appointments_Status", "Status IN (0, 1, 2, 3)");
            });
        });

        // ChatSessions: PK, optional 1:1 with Appointment via unique FK, and conservative no-action links.
        modelBuilder.Entity<ChatSession>(entity =>
        {
            entity.HasKey(cs => cs.Id);
            entity.HasIndex(cs => cs.AppointmentId).IsUnique();

            entity.HasOne(cs => cs.Patient)
                .WithMany(p => p.ChatSessions)
                .HasForeignKey(cs => cs.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(cs => cs.Doctor)
                .WithMany(d => d.ChatSessions)
                .HasForeignKey(cs => cs.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(cs => cs.Appointment)
                .WithOne(a => a.ChatSession)
                .HasForeignKey<ChatSession>(cs => cs.AppointmentId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.Property(cs => cs.CreatedAt).HasColumnType("datetime");
            entity.Property(cs => cs.ClosedAt).HasColumnType("datetime");
            entity.ToTable(tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("CK_ChatSessions_Status", "Status IN (0, 1)");
            });
        });

        // Messages: PK, chat FK with CASCADE, and sender/status validation.
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.HasOne(m => m.ChatSession)
                .WithMany(cs => cs.Messages)
                .HasForeignKey(m => m.ChatSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(m => m.SentAt).HasColumnType("datetime");
            entity.ToTable(tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("CK_Messages_SenderType", "SenderType IN (0, 1)");
            });
        });

        // Attachments: PK and CASCADE from message to keep chat storage consistent.
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.HasOne(a => a.Message)
                .WithMany(m => m.Attachments)
                .HasForeignKey(a => a.MessageId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}