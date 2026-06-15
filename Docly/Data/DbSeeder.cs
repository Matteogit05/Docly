using Microsoft.EntityFrameworkCore;
using Docly.Data.Entities;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        await SeedDoctorsAsync(db);
        await SeedPatientsAsync(db);
        await SeedDoctorAvailabilitiesAsync(db);
    }

    private static async Task SeedDoctorsAsync(ApplicationDbContext db)
    {
        var seedDoctors = new[]
        {
            new Doctor
            {
                Id = 1,
                Email = "mario.rossi@docly.it",
                PasswordHash = "HASHED_PASSWORD_DOCTOR_1",
                FirstName = "Mario",
                LastName = "Rossi",
                Specialization = "Cardiologia",
                Bio = "Cardiologo con esperienza in prevenzione cardiovascolare e controlli periodici.",
                IsActive = true
            },
            new Doctor
            {
                Id = 2,
                Email = "giulia.bianchi@docly.it",
                PasswordHash = "HASHED_PASSWORD_DOCTOR_2",
                FirstName = "Giulia",
                LastName = "Bianchi",
                Specialization = "Dermatologia",
                Bio = "Dermatologa focalizzata su prevenzione, acne, dermatiti e controllo nei.",
                IsActive = true
            },
            new Doctor
            {
                Id = 3,
                Email = "luca.verdi@docly.it",
                PasswordHash = "HASHED_PASSWORD_DOCTOR_3",
                FirstName = "Luca",
                LastName = "Verdi",
                Specialization = "Ortopedia",
                Bio = "Ortopedico orientato alla gestione conservativa e al recupero funzionale.",
                IsActive = true
            },
            new Doctor
            {
                Id = 4,
                Email = "elena.gallo@docly.it",
                PasswordHash = "HASHED_PASSWORD_DOCTOR_4",
                FirstName = "Elena",
                LastName = "Gallo",
                Specialization = "Pediatria",
                Bio = "Pediatra per visite di controllo, vaccinazioni e supporto alla crescita.",
                IsActive = true
            },
            new Doctor
            {
                Id = 5,
                Email = "andrea.ferrari@docly.it",
                PasswordHash = "HASHED_PASSWORD_DOCTOR_5",
                FirstName = "Andrea",
                LastName = "Ferrari",
                Specialization = "Oculistica",
                Bio = "Oculista per visite della vista, controllo pressione oculare e prevenzione.",
                IsActive = true
            }
        };

        var existingEmails = await db.Doctors
            .Select(doctor => doctor.Email)
            .ToListAsync();

        var doctorsToAdd = seedDoctors
            .Where(doctor => !existingEmails.Contains(doctor.Email))
            .ToList();

        if (doctorsToAdd.Count == 0)
            return;

        db.Doctors.AddRange(doctorsToAdd);
        await db.SaveChangesAsync();
    }

    private static async Task SeedPatientsAsync(ApplicationDbContext db)
    {
        var seedPatients = new[]
        {
            new Patient
            {
                Id = 1,
                Email = "anna.neri@docly.it",
                PasswordHash = "HASHED_PASSWORD_PATIENT_1",
                FirstName = "Anna",
                LastName = "Neri",
                DateOfBirth = new DateTime(1994, 3, 12),
                TaxId = "NRNANNA94C52H501Z"
            },
            new Patient
            {
                Id = 2,
                Email = "paolo.conti@docly.it",
                PasswordHash = "HASHED_PASSWORD_PATIENT_2",
                FirstName = "Paolo",
                LastName = "Conti",
                DateOfBirth = new DateTime(1988, 7, 4),
                TaxId = "CNTPLA88L04F205Q"
            },
            new Patient
            {
                Id = 3,
                Email = "sara.marini@docly.it",
                PasswordHash = "HASHED_PASSWORD_PATIENT_3",
                FirstName = "Sara",
                LastName = "Marini",
                DateOfBirth = new DateTime(2001, 11, 22),
                TaxId = "MRNSRA01S62D969D"
            },
            new Patient
            {
                Id = 4,
                Email = "matteo.greco@docly.it",
                PasswordHash = "HASHED_PASSWORD_PATIENT_4",
                FirstName = "Matteo",
                LastName = "Greco",
                DateOfBirth = new DateTime(1979, 1, 18),
                TaxId = "GRCMTT79A18H501R"
            },
            new Patient
            {
                Id = 5,
                Email = "chiara.riva@docly.it",
                PasswordHash = "HASHED_PASSWORD_PATIENT_5",
                FirstName = "Chiara",
                LastName = "Riva",
                DateOfBirth = new DateTime(1997, 9, 30),
                TaxId = "RVICHR97P70F205L"
            }
        };

        var existingEmails = await db.Patients
            .Select(patient => patient.Email)
            .ToListAsync();

        var existingTaxIds = await db.Patients
            .Select(patient => patient.TaxId)
            .ToListAsync();

        var patientsToAdd = seedPatients
            .Where(patient => !existingEmails.Contains(patient.Email) && !existingTaxIds.Contains(patient.TaxId))
            .ToList();

        if (patientsToAdd.Count == 0)
            return;

        db.Patients.AddRange(patientsToAdd);
        await db.SaveChangesAsync();
    }

    private static async Task SeedDoctorAvailabilitiesAsync(ApplicationDbContext db)
    {
        var seedAvailabilities = new[]
        {
            (DoctorEmail: "mario.rossi@docly.it", StartTime: new DateTime(2026, 6, 15, 9, 0, 0), EndTime: new DateTime(2026, 6, 15, 12, 0, 0), IsBooked: false),
            (DoctorEmail: "mario.rossi@docly.it", StartTime: new DateTime(2026, 6, 16, 14, 0, 0), EndTime: new DateTime(2026, 6, 16, 18, 0, 0), IsBooked: false),
            (DoctorEmail: "giulia.bianchi@docly.it", StartTime: new DateTime(2026, 6, 15, 10, 0, 0), EndTime: new DateTime(2026, 6, 15, 13, 0, 0), IsBooked: false),
            (DoctorEmail: "giulia.bianchi@docly.it", StartTime: new DateTime(2026, 6, 17, 15, 0, 0), EndTime: new DateTime(2026, 6, 17, 18, 0, 0), IsBooked: false),
            (DoctorEmail: "luca.verdi@docly.it", StartTime: new DateTime(2026, 6, 16, 9, 30, 0), EndTime: new DateTime(2026, 6, 16, 12, 30, 0), IsBooked: false),
            (DoctorEmail: "luca.verdi@docly.it", StartTime: new DateTime(2026, 6, 18, 14, 30, 0), EndTime: new DateTime(2026, 6, 18, 17, 30, 0), IsBooked: false),
            (DoctorEmail: "elena.gallo@docly.it", StartTime: new DateTime(2026, 6, 15, 8, 30, 0), EndTime: new DateTime(2026, 6, 15, 12, 30, 0), IsBooked: false),
            (DoctorEmail: "elena.gallo@docly.it", StartTime: new DateTime(2026, 6, 17, 9, 0, 0), EndTime: new DateTime(2026, 6, 17, 13, 0, 0), IsBooked: false),
            (DoctorEmail: "andrea.ferrari@docly.it", StartTime: new DateTime(2026, 6, 16, 11, 0, 0), EndTime: new DateTime(2026, 6, 16, 14, 0, 0), IsBooked: false),
            (DoctorEmail: "andrea.ferrari@docly.it", StartTime: new DateTime(2026, 6, 18, 15, 0, 0), EndTime: new DateTime(2026, 6, 18, 19, 0, 0), IsBooked: false)
        };

        var doctorEmails = seedAvailabilities
            .Select(availability => availability.DoctorEmail)
            .Distinct()
            .ToList();

        var doctorsByEmail = await db.Doctors
            .Where(doctor => doctorEmails.Contains(doctor.Email))
            .ToDictionaryAsync(doctor => doctor.Email);

        var doctorIds = doctorsByEmail.Values
            .Select(doctor => doctor.Id)
            .ToList();

        var existingSlots = await db.DoctorAvailabilities
            .Where(availability => doctorIds.Contains(availability.DoctorId))
            .Select(availability => new
            {
                availability.DoctorId,
                availability.StartTime,
                availability.EndTime,
                availability.IsBooked
            })
            .ToListAsync();

        var existingSlotSet = existingSlots
            .Select(slot => (slot.DoctorId, slot.StartTime, slot.EndTime, slot.IsBooked))
            .ToHashSet();

        var availabilitiesToAdd = new List<DoctorAvailability>();

        foreach (var seedAvailability in seedAvailabilities)
        {
            if (!doctorsByEmail.TryGetValue(seedAvailability.DoctorEmail, out var doctor))
                continue;

            var slotKey = (doctor.Id, seedAvailability.StartTime, seedAvailability.EndTime, seedAvailability.IsBooked);
            if (existingSlotSet.Contains(slotKey))
                continue;

            availabilitiesToAdd.Add(new DoctorAvailability
            {
                DoctorId = doctor.Id,
                StartTime = seedAvailability.StartTime,
                EndTime = seedAvailability.EndTime,
                IsBooked = seedAvailability.IsBooked
            });
        }

        if (availabilitiesToAdd.Count == 0)
            return;

        db.DoctorAvailabilities.AddRange(availabilitiesToAdd);
        await db.SaveChangesAsync();
    }
}