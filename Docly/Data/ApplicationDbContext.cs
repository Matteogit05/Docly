using Microsoft.EntityFrameworkCore;
using Docly.Data.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Doctor> Doctors { get; set; } = null!;
    public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;
    public DbSet<FavoriteDoctor> FavoriteDoctors { get; set; } = null!;
    public DbSet<Chat> Chats { get; set; } = null!;
    public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
    public DbSet<ChatFile> ChatFiles { get; set; } = null!;
    public DbSet<Rating> Ratings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // USERS: chiave primaria e vincolo UNIQUE sull'email.
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // DOCTORS: estensione 1:1 di Users e vincolo UNIQUE sulla licenza.
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId);
            entity.HasIndex(e => e.LicenseNumber).IsUnique();
            entity.HasIndex(e => e.Specialization);
            entity.HasIndex(e => e.IsVerified);

            entity.HasOne(e => e.User)
                .WithOne(e => e.DoctorProfile)
                .HasForeignKey<Doctor>(e => e.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // DOCTOR AVAILABILITY: disponibilità settimanali collegate al medico.
        modelBuilder.Entity<DoctorAvailability>(entity =>
        {
            entity.HasKey(e => e.AvailabilityId);

            entity.HasOne(e => e.Doctor)
                .WithMany(e => e.Availabilities)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // APPOINTMENTS: prenotazioni paziente-medico con vincolo anti-conflitto orario.
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId);
            entity.HasIndex(e => new { e.DoctorId, e.AppointmentDateTime }).IsUnique();
            entity.HasIndex(e => e.PatientId);
            entity.HasIndex(e => e.DoctorId);
            entity.HasIndex(e => e.AppointmentDateTime);

            entity.HasOne(e => e.Patient)
                .WithMany(e => e.PatientAppointments)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Doctor)
                .WithMany(e => e.Appointments)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.CancelledByUser)
                .WithMany(e => e.CancelledAppointments)
                .HasForeignKey(e => e.CancelledBy)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // FAVORITE DOCTORS: tabella ponte con anti-duplicato paziente-medico.
        modelBuilder.Entity<FavoriteDoctor>(entity =>
        {
            entity.HasKey(e => e.FavoriteDoctorId);
            entity.HasIndex(e => new { e.PatientId, e.DoctorId }).IsUnique();
            entity.HasIndex(e => e.PatientId);

            entity.HasOne(e => e.Patient)
                .WithMany(e => e.FavoriteDoctors)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Doctor)
                .WithMany(e => e.FavoriteDoctors)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // CHATS: conversazioni private con vincolo 1:1 opzionale verso Appointment.
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.ChatId);
            entity.HasIndex(e => e.PatientId);
            entity.HasIndex(e => e.DoctorId);
            entity.HasIndex(e => e.AppointmentId).IsUnique().HasFilter("[AppointmentId] IS NOT NULL");

            entity.HasOne(e => e.Patient)
                .WithMany(e => e.PatientChats)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Doctor)
                .WithMany(e => e.Chats)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Appointment)
                .WithOne(e => e.Chat)
                .HasForeignKey<Chat>(e => e.AppointmentId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // CHAT MESSAGES: messaggi appartenenti a una chat e a un mittente.
        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId);
            entity.HasIndex(e => e.ChatId);
            entity.HasIndex(e => e.SenderId);

            entity.HasOne(e => e.Chat)
                .WithMany(e => e.Messages)
                .HasForeignKey(e => e.ChatId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Sender)
                .WithMany(e => e.SentMessages)
                .HasForeignKey(e => e.SenderId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // CHAT FILES: file della chat, opzionalmente collegati a un messaggio.
        modelBuilder.Entity<ChatFile>(entity =>
        {
            entity.HasKey(e => e.FileId);
            entity.HasIndex(e => e.ChatId);
            entity.HasIndex(e => e.MessageId);
            entity.HasIndex(e => e.UploadedBy);

            entity.HasOne(e => e.Chat)
                .WithMany(e => e.Files)
                .HasForeignKey(e => e.ChatId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Message)
                .WithMany(e => e.Files)
                .HasForeignKey(e => e.MessageId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.UploadedByUser)
                .WithMany(e => e.UploadedFiles)
                .HasForeignKey(e => e.UploadedBy)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // RATINGS: una sola valutazione per appuntamento e legami verso medico/paziente.
        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId);
            entity.HasIndex(e => e.AppointmentId).IsUnique();
            entity.HasIndex(e => e.DoctorId);
            entity.HasIndex(e => e.PatientId);

            entity.HasOne(e => e.Appointment)
                .WithOne(e => e.Rating)
                .HasForeignKey<Rating>(e => e.AppointmentId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Doctor)
                .WithMany(e => e.Ratings)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Patient)
                .WithMany(e => e.RatingsGiven)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}