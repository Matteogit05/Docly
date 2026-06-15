using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<FavoriteDoctor> FavoriteDoctors { get; set; }
    public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<ChatSession> ChatSessions { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Attachment> Attachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("AspNetUsers");
            entity.HasKey(user => user.Id);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable("Patients");
            entity.HasKey(patient => patient.Id);
            entity.HasIndex(patient => patient.IdentityUserId).IsUnique();
            entity.HasIndex(patient => patient.TaxId).IsUnique();

            entity.Property(patient => patient.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(patient => patient.LastName).IsRequired().HasMaxLength(100);
            entity.Property(patient => patient.IdentityUserId).IsRequired().HasMaxLength(450);
            entity.Property(patient => patient.TaxId).IsRequired().HasMaxLength(16);

            entity.HasOne(patient => patient.ApplicationUser)
                .WithOne()
                .HasForeignKey<Patient>(patient => patient.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict); // Keep the domain profile if the Identity account is removed.
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable("Doctors");
            entity.HasKey(doctor => doctor.Id);
            entity.HasIndex(doctor => doctor.IdentityUserId).IsUnique();

            entity.Property(doctor => doctor.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(doctor => doctor.LastName).IsRequired().HasMaxLength(100);
            entity.Property(doctor => doctor.IdentityUserId).IsRequired().HasMaxLength(450);
            entity.Property(doctor => doctor.Specialization).IsRequired().HasMaxLength(150);
            entity.Property(doctor => doctor.Bio).HasColumnType("TEXT");

            entity.HasOne(doctor => doctor.ApplicationUser)
                .WithOne()
                .HasForeignKey<Doctor>(doctor => doctor.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict); // Same reasoning as Patient: preserve domain history.
        });

        modelBuilder.Entity<FavoriteDoctor>(entity =>
        {
            entity.ToTable("FavoriteDoctors");
            entity.HasKey(favoriteDoctor => new { favoriteDoctor.PatientId, favoriteDoctor.DoctorId });

            entity.HasOne(favoriteDoctor => favoriteDoctor.Patient)
                .WithMany(patient => patient.FavoriteDoctors)
                .HasForeignKey(favoriteDoctor => favoriteDoctor.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(favoriteDoctor => favoriteDoctor.Doctor)
                .WithMany(doctor => doctor.FavoriteDoctors)
                .HasForeignKey(favoriteDoctor => favoriteDoctor.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DoctorAvailability>(entity =>
        {
            entity.ToTable("DoctorAvailabilities", tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("CK_DoctorAvailabilities_TimeRange", "EndTime > StartTime");
            });
            entity.HasKey(availability => availability.Id);

            entity.Property(availability => availability.StartTime).IsRequired();
            entity.Property(availability => availability.EndTime).IsRequired();

            entity.HasOne(availability => availability.Doctor)
                .WithMany(doctor => doctor.DoctorAvailabilities)
                .HasForeignKey(availability => availability.DoctorId)
                .OnDelete(DeleteBehavior.Restrict); // Availability rows are historical scheduling data.
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.ToTable("Appointments", tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("CK_Appointments_TimeRange", "EndTime > StartTime");
                tableBuilder.HasCheckConstraint("CK_Appointments_Status", "Status IN (0, 1, 2, 3)");
            });
            entity.HasKey(appointment => appointment.Id);

            entity.Property(appointment => appointment.SymptomNotes).HasColumnType("TEXT");
            entity.Property(appointment => appointment.StartTime).IsRequired();
            entity.Property(appointment => appointment.EndTime).IsRequired();

            entity.HasOne(appointment => appointment.Patient)
                .WithMany(patient => patient.Appointments)
                .HasForeignKey(appointment => appointment.PatientId)
                .OnDelete(DeleteBehavior.Restrict); // Preserve appointment history.

            entity.HasOne(appointment => appointment.Doctor)
                .WithMany(doctor => doctor.Appointments)
                .HasForeignKey(appointment => appointment.DoctorId)
                .OnDelete(DeleteBehavior.Restrict); // Preserve appointment history.

            entity.HasOne(appointment => appointment.ChatSession)
                .WithOne(chatSession => chatSession.Appointment)
                .HasForeignKey<ChatSession>(chatSession => chatSession.AppointmentId)
                .OnDelete(DeleteBehavior.SetNull); // Optional link; keep chat if appointment is removed.
        });

        modelBuilder.Entity<ChatSession>(entity =>
        {
            entity.ToTable("ChatSessions", tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("CK_ChatSessions_Status", "Status IN (0, 1)");
            });
            entity.HasKey(chatSession => chatSession.Id);

            entity.HasOne(chatSession => chatSession.Patient)
                .WithMany(patient => patient.ChatSessions)
                .HasForeignKey(chatSession => chatSession.PatientId)
                .OnDelete(DeleteBehavior.Restrict); // Chat history should survive profile cleanup.

            entity.HasOne(chatSession => chatSession.Doctor)
                .WithMany(doctor => doctor.ChatSessions)
                .HasForeignKey(chatSession => chatSession.DoctorId)
                .OnDelete(DeleteBehavior.Restrict); // Chat history should survive profile cleanup.
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("Messages", tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("CK_Messages_SenderType", "SenderType IN (0, 1)");
            });
            entity.HasKey(message => message.Id);

            entity.Property(message => message.Content).HasColumnType("TEXT");

            entity.HasOne(message => message.ChatSession)
                .WithMany(chatSession => chatSession.Messages)
                .HasForeignKey(message => message.ChatSessionId)
                .OnDelete(DeleteBehavior.Cascade); // Removing a chat removes its messages by design.
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.ToTable("Attachments");
            entity.HasKey(attachment => attachment.Id);

            entity.Property(attachment => attachment.FileName).IsRequired().HasMaxLength(255);
            entity.Property(attachment => attachment.FileExtension).IsRequired().HasMaxLength(10);
            entity.Property(attachment => attachment.StoragePath).IsRequired().HasMaxLength(500);

            entity.HasOne(attachment => attachment.Message)
                .WithMany(message => message.Attachments)
                .HasForeignKey(attachment => attachment.MessageId)
                .OnDelete(DeleteBehavior.Cascade); // Attachments are dependent data for the message.
        });
    }
}