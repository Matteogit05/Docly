using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        await SeedApplicationUsersAsync(db);
        await SeedDoctorsAsync(db);
        await SeedPatientsAsync(db);
        await SeedDoctorAvailabilitiesAsync(db);
    }

    private static async Task SeedApplicationUsersAsync(ApplicationDbContext db)
    {
        var passwordHasher = new PasswordHasher<ApplicationUser>();

        var seedUsers = new[]
        {
            new ApplicationUser
            {
                Id = "2d9a8d58-2ec7-4e02-bd5f-7b1d8b7c1001",
                UserName = "doctor1@docly.test",
                NormalizedUserName = "DOCTOR1@DOCLY.TEST",
                Email = "doctor1@docly.test",
                NormalizedEmail = "DOCTOR1@DOCLY.TEST",
                EmailConfirmed = true
            },
            new ApplicationUser
            {
                Id = "2d9a8d58-2ec7-4e02-bd5f-7b1d8b7c1002",
                UserName = "doctor2@docly.test",
                NormalizedUserName = "DOCTOR2@DOCLY.TEST",
                Email = "doctor2@docly.test",
                NormalizedEmail = "DOCTOR2@DOCLY.TEST",
                EmailConfirmed = true
            },
            new ApplicationUser
            {
                Id = "2d9a8d58-2ec7-4e02-bd5f-7b1d8b7c1003",
                UserName = "doctor3@docly.test",
                NormalizedUserName = "DOCTOR3@DOCLY.TEST",
                Email = "doctor3@docly.test",
                NormalizedEmail = "DOCTOR3@DOCLY.TEST",
                EmailConfirmed = true
            },
            new ApplicationUser
            {
                Id = "2d9a8d58-2ec7-4e02-bd5f-7b1d8b7c1004",
                UserName = "doctor4@docly.test",
                NormalizedUserName = "DOCTOR4@DOCLY.TEST",
                Email = "doctor4@docly.test",
                NormalizedEmail = "DOCTOR4@DOCLY.TEST",
                EmailConfirmed = true
            },
            new ApplicationUser
            {
                Id = "2d9a8d58-2ec7-4e02-bd5f-7b1d8b7c1005",
                UserName = "doctor5@docly.test",
                NormalizedUserName = "DOCTOR5@DOCLY.TEST",
                Email = "doctor5@docly.test",
                NormalizedEmail = "DOCTOR5@DOCLY.TEST",
                EmailConfirmed = true
            },
            new ApplicationUser
            {
                Id = "8fa65a79-1f0d-4f1e-86a7-8d7b5a2a2001",
                UserName = "patient1@docly.test",
                NormalizedUserName = "PATIENT1@DOCLY.TEST",
                Email = "patient1@docly.test",
                NormalizedEmail = "PATIENT1@DOCLY.TEST",
                EmailConfirmed = true
            },
            new ApplicationUser
            {
                Id = "8fa65a79-1f0d-4f1e-86a7-8d7b5a2a2002",
                UserName = "patient2@docly.test",
                NormalizedUserName = "PATIENT2@DOCLY.TEST",
                Email = "patient2@docly.test",
                NormalizedEmail = "PATIENT2@DOCLY.TEST",
                EmailConfirmed = true
            },
            new ApplicationUser
            {
                Id = "8fa65a79-1f0d-4f1e-86a7-8d7b5a2a2003",
                UserName = "patient3@docly.test",
                NormalizedUserName = "PATIENT3@DOCLY.TEST",
                Email = "patient3@docly.test",
                NormalizedEmail = "PATIENT3@DOCLY.TEST",
                EmailConfirmed = true
            },
            new ApplicationUser
            {
                Id = "8fa65a79-1f0d-4f1e-86a7-8d7b5a2a2004",
                UserName = "patient4@docly.test",
                NormalizedUserName = "PATIENT4@DOCLY.TEST",
                Email = "patient4@docly.test",
                NormalizedEmail = "PATIENT4@DOCLY.TEST",
                EmailConfirmed = true
            },
            new ApplicationUser
            {
                Id = "8fa65a79-1f0d-4f1e-86a7-8d7b5a2a2005",
                UserName = "patient5@docly.test",
                NormalizedUserName = "PATIENT5@DOCLY.TEST",
                Email = "patient5@docly.test",
                NormalizedEmail = "PATIENT5@DOCLY.TEST",
                EmailConfirmed = true
            }
        };

        var existingUserIds = await db.ApplicationUsers
            .Select(user => user.Id)
            .ToHashSetAsync();

        foreach (var seedUser in seedUsers)
        {
            if (existingUserIds.Contains(seedUser.Id))
            {
                continue;
            }

            seedUser.PasswordHash = passwordHasher.HashPassword(seedUser, "Password123!");
            db.ApplicationUsers.Add(seedUser);
        }

        await db.SaveChangesAsync();
    }

    private static async Task SeedDoctorsAsync(ApplicationDbContext db)
    {
        var seedDoctors = new[]
        {
            new Doctor
            {
                Id = 1,
                IdentityUserId = "2d9a8d58-2ec7-4e02-bd5f-7b1d8b7c1001",
                FirstName = "Marco",
                LastName = "Rinaldi",
                Specialization = "Cardiologia",
                Bio = "Specialista in prevenzione cardiovascolare e follow-up clinico.",
                IsActive = true
            },
            new Doctor
            {
                Id = 2,
                IdentityUserId = "2d9a8d58-2ec7-4e02-bd5f-7b1d8b7c1002",
                FirstName = "Giulia",
                LastName = "Ferri",
                Specialization = "Dermatologia",
                Bio = "Si occupa di patologie cutanee, controlli periodici e telemedicina.",
                IsActive = true
            },
            new Doctor
            {
                Id = 3,
                IdentityUserId = "2d9a8d58-2ec7-4e02-bd5f-7b1d8b7c1003",
                FirstName = "Luca",
                LastName = "Moretti",
                Specialization = "Ortopedia",
                Bio = "Gestione di traumi, dolori articolari e percorsi riabilitativi.",
                IsActive = true
            },
            new Doctor
            {
                Id = 4,
                IdentityUserId = "2d9a8d58-2ec7-4e02-bd5f-7b1d8b7c1004",
                FirstName = "Sara",
                LastName = "Conti",
                Specialization = "Pediatria",
                Bio = "Visite pediatriche, controlli di crescita e consulenza per famiglie.",
                IsActive = true
            },
            new Doctor
            {
                Id = 5,
                IdentityUserId = "2d9a8d58-2ec7-4e02-bd5f-7b1d8b7c1005",
                FirstName = "Andrea",
                LastName = "Bianchi",
                Specialization = "Neurologia",
                Bio = "Valutazione di cefalee, disturbi del sonno e sintomi neurologici.",
                IsActive = true
            }
        };

        var existingDoctorIds = await db.Doctors
            .Select(doctor => doctor.Id)
            .ToHashSetAsync();

        foreach (var seedDoctor in seedDoctors)
        {
            if (existingDoctorIds.Contains(seedDoctor.Id))
            {
                continue;
            }

            db.Doctors.Add(seedDoctor);
        }

        await db.SaveChangesAsync();
    }

    private static async Task SeedPatientsAsync(ApplicationDbContext db)
    {
        var seedPatients = new[]
        {
            new Patient
            {
                Id = 1,
                IdentityUserId = "8fa65a79-1f0d-4f1e-86a7-8d7b5a2a2001",
                FirstName = "Elena",
                LastName = "Galli",
                DateOfBirth = new DateTime(1990, 3, 14),
                TaxId = "GLLLNE90C54H501A"
            },
            new Patient
            {
                Id = 2,
                IdentityUserId = "8fa65a79-1f0d-4f1e-86a7-8d7b5a2a2002",
                FirstName = "Matteo",
                LastName = "Romano",
                DateOfBirth = new DateTime(1987, 11, 2),
                TaxId = "RMNMTT87S02F205B"
            },
            new Patient
            {
                Id = 3,
                IdentityUserId = "8fa65a79-1f0d-4f1e-86a7-8d7b5a2a2003",
                FirstName = "Chiara",
                LastName = "Vitale",
                DateOfBirth = new DateTime(1995, 6, 27),
                TaxId = "VTLCHR95H67L219C"
            },
            new Patient
            {
                Id = 4,
                IdentityUserId = "8fa65a79-1f0d-4f1e-86a7-8d7b5a2a2004",
                FirstName = "Davide",
                LastName = "Greco",
                DateOfBirth = new DateTime(1979, 9, 8),
                TaxId = "GRCDVD79P08F839D"
            },
            new Patient
            {
                Id = 5,
                IdentityUserId = "8fa65a79-1f0d-4f1e-86a7-8d7b5a2a2005",
                FirstName = "Francesca",
                LastName = "Lombardi",
                DateOfBirth = new DateTime(2001, 1, 19),
                TaxId = "LMBFNC01A59D325E"
            }
        };

        var existingPatientIds = await db.Patients
            .Select(patient => patient.Id)
            .ToHashSetAsync();

        foreach (var seedPatient in seedPatients)
        {
            if (existingPatientIds.Contains(seedPatient.Id))
            {
                continue;
            }

            db.Patients.Add(seedPatient);
        }

        await db.SaveChangesAsync();
    }

    private static async Task SeedDoctorAvailabilitiesAsync(ApplicationDbContext db)
    {
        var seedAvailabilities = new[]
        {
            new DoctorAvailability
            {
                Id = 1,
                DoctorId = 1,
                StartTime = new DateTime(2026, 7, 1, 9, 0, 0),
                EndTime = new DateTime(2026, 7, 1, 11, 0, 0),
                IsBooked = false
            },
            new DoctorAvailability
            {
                Id = 2,
                DoctorId = 1,
                StartTime = new DateTime(2026, 7, 1, 14, 0, 0),
                EndTime = new DateTime(2026, 7, 1, 16, 0, 0),
                IsBooked = false
            },
            new DoctorAvailability
            {
                Id = 3,
                DoctorId = 2,
                StartTime = new DateTime(2026, 7, 2, 9, 0, 0),
                EndTime = new DateTime(2026, 7, 2, 12, 0, 0),
                IsBooked = false
            },
            new DoctorAvailability
            {
                Id = 4,
                DoctorId = 2,
                StartTime = new DateTime(2026, 7, 2, 15, 0, 0),
                EndTime = new DateTime(2026, 7, 2, 18, 0, 0),
                IsBooked = false
            },
            new DoctorAvailability
            {
                Id = 5,
                DoctorId = 3,
                StartTime = new DateTime(2026, 7, 3, 10, 0, 0),
                EndTime = new DateTime(2026, 7, 3, 13, 0, 0),
                IsBooked = false
            },
            new DoctorAvailability
            {
                Id = 6,
                DoctorId = 3,
                StartTime = new DateTime(2026, 7, 3, 16, 0, 0),
                EndTime = new DateTime(2026, 7, 3, 18, 0, 0),
                IsBooked = false
            },
            new DoctorAvailability
            {
                Id = 7,
                DoctorId = 4,
                StartTime = new DateTime(2026, 7, 4, 9, 30, 0),
                EndTime = new DateTime(2026, 7, 4, 12, 30, 0),
                IsBooked = false
            },
            new DoctorAvailability
            {
                Id = 8,
                DoctorId = 4,
                StartTime = new DateTime(2026, 7, 4, 14, 30, 0),
                EndTime = new DateTime(2026, 7, 4, 17, 0, 0),
                IsBooked = false
            },
            new DoctorAvailability
            {
                Id = 9,
                DoctorId = 5,
                StartTime = new DateTime(2026, 7, 5, 8, 30, 0),
                EndTime = new DateTime(2026, 7, 5, 11, 30, 0),
                IsBooked = false
            },
            new DoctorAvailability
            {
                Id = 10,
                DoctorId = 5,
                StartTime = new DateTime(2026, 7, 5, 13, 30, 0),
                EndTime = new DateTime(2026, 7, 5, 16, 30, 0),
                IsBooked = false
            }
        };

        var existingAvailabilityKeys = await db.DoctorAvailabilities
            .Select(availability => new
            {
                availability.DoctorId,
                availability.StartTime,
                availability.EndTime,
                availability.IsBooked
            })
            .ToListAsync();

        foreach (var seedAvailability in seedAvailabilities)
        {
            var alreadyExists = existingAvailabilityKeys.Any(existingAvailability =>
                existingAvailability.DoctorId == seedAvailability.DoctorId &&
                existingAvailability.StartTime == seedAvailability.StartTime &&
                existingAvailability.EndTime == seedAvailability.EndTime &&
                existingAvailability.IsBooked == seedAvailability.IsBooked);

            if (alreadyExists)
            {
                continue;
            }

            db.DoctorAvailabilities.Add(seedAvailability);
        }

        await db.SaveChangesAsync();
    }
}