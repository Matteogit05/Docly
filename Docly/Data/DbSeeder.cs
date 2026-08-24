using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var db = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        await db.Database.EnsureCreatedAsync(); // Assicurati che il db esista
        await SeedRolesAndAdminAsync(roleManager, userManager, configuration);

        await SeedApplicationUsersAsync(userManager);
        await SeedDoctorsAsync(db);
        await SeedPatientsAsync(db);
        /*
        await SeedDoctorSchedulesAsync(db);
        await SeedDoctorAbsencesAsync(db);
        */
    }

    private static async Task SeedRolesAndAdminAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        string[] roleNames = { "Admin", "Doctor", "Patient" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        string adminEmail = configuration["AdminSettings:Email"] ?? "admin@docly.it";
        string adminPassword = configuration["AdminSettings:Password"];

        if (string.IsNullOrEmpty(adminPassword))
        {
            throw new Exception("Attenzione: La password per l'Admin non è stata configurata in appsettings.json!");
        }

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword); 
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
    private static async Task SeedApplicationUsersAsync(UserManager<ApplicationUser> userManager)
    {
        var seedUsers = new[]
        {
            new ApplicationUser { Id = "2d9a8d58-2ec7-4e02-bd5f-7b1d8b7c1001", UserName = "doctor1@docly.test", Email = "doctor1@docly.test", EmailConfirmed = true },
            new ApplicationUser { Id = "2d9a8d58-2ec7-4e02-bd5f-7b1d8b7c1002", UserName = "doctor2@docly.test", Email = "doctor2@docly.test", EmailConfirmed = true },
            new ApplicationUser { Id = "2d9a8d58-2ec7-4e02-bd5f-7b1d8b7c1003", UserName = "doctor3@docly.test", Email = "doctor3@docly.test", EmailConfirmed = true },
            new ApplicationUser { Id = "2d9a8d58-2ec7-4e02-bd5f-7b1d8b7c1004", UserName = "doctor4@docly.test", Email = "doctor4@docly.test", EmailConfirmed = true },
            new ApplicationUser { Id = "2d9a8d58-2ec7-4e02-bd5f-7b1d8b7c1005", UserName = "doctor5@docly.test", Email = "doctor5@docly.test", EmailConfirmed = true },
            
            new ApplicationUser { Id = "8fa65a79-1f0d-4f1e-86a7-8d7b5a2a2001", UserName = "patient1@docly.test", Email = "patient1@docly.test", EmailConfirmed = true },
            new ApplicationUser { Id = "8fa65a79-1f0d-4f1e-86a7-8d7b5a2a2002", UserName = "patient2@docly.test", Email = "patient2@docly.test", EmailConfirmed = true },
            new ApplicationUser { Id = "8fa65a79-1f0d-4f1e-86a7-8d7b5a2a2003", UserName = "patient3@docly.test", Email = "patient3@docly.test", EmailConfirmed = true },
            new ApplicationUser { Id = "8fa65a79-1f0d-4f1e-86a7-8d7b5a2a2004", UserName = "patient4@docly.test", Email = "patient4@docly.test", EmailConfirmed = true },
            new ApplicationUser { Id = "8fa65a79-1f0d-4f1e-86a7-8d7b5a2a2005", UserName = "patient5@docly.test", Email = "patient5@docly.test", EmailConfirmed = true }
        };

        foreach (var user in seedUsers)
        {
            if (await userManager.FindByIdAsync(user.Id) == null)
            {
                var result = await userManager.CreateAsync(user, "Password123!");
                
                if (result.Succeeded)
                {
                    if (user.Email.Contains("doctor"))
                    {
                        await userManager.AddToRoleAsync(user, "Doctor");
                    }
                    else if (user.Email.Contains("patient"))
                    {
                        await userManager.AddToRoleAsync(user, "Patient");
                    }
                }
            }
        }
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
    /*
    private static async Task SeedDoctorSchedulesAsync(ApplicationDbContext db)
    {
        var seedSchedules = new List<DoctorSchedule>();
        int scheduleId = 1;

        // Creiamo un orario per il Dottor 1 (Marco Rinaldi): Lun-Ven, 09:00-13:00 e 14:00-18:00
        for (int i = 1; i <= 5; i++) // 1=Lunedì, 5=Venerdì
        {
            seedSchedules.Add(new DoctorSchedule
            {
                DoctorId = 1,
                DayOfWeek = (DayOfWeek)i,
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(13, 0, 0),
                SlotDurationMinutes = 15
            });

            seedSchedules.Add(new DoctorSchedule
            {
                DoctorId = 1,
                DayOfWeek = (DayOfWeek)i,
                StartTime = new TimeSpan(14, 0, 0),
                EndTime = new TimeSpan(18, 0, 0),
                SlotDurationMinutes = 15
            });

            seedSchedules.Add(new DoctorSchedule
            {
                DoctorId = 2,
                DayOfWeek = (DayOfWeek)i,
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(12, 0, 0),
                SlotDurationMinutes = 15
            });

            seedSchedules.Add(new DoctorSchedule
            {
                DoctorId = 2,
                DayOfWeek = (DayOfWeek)i,
                StartTime = new TimeSpan(14, 0, 0),
                EndTime = new TimeSpan(16, 0, 0),
                SlotDurationMinutes = 15
            });
        }
        if (await db.DoctorSchedules.AnyAsync())
        {
            db.DoctorSchedules.RemoveRange(db.DoctorSchedules);
            await db.SaveChangesAsync();
        }

        db.DoctorSchedules.AddRange(seedSchedules);
        await db.SaveChangesAsync();
    }

    private static async Task SeedDoctorAbsencesAsync(ApplicationDbContext db)
    {
        var seedAbsences = new[]
        {
            new DoctorAbsence
            {
                DoctorId = 1, // Marco Rinaldi
                StartDate = new DateTime(2026, 8, 10), // Assente dal 10 Agosto
                EndDate = new DateTime(2026, 8, 20),   // Fino al 20 Agosto compreso
                Reason = "Ferie estive"
            },

            new DoctorAbsence
            {
                DoctorId = 2,
                StartDate = new DateTime(2026, 8, 30),
                EndDate = new DateTime(2026, 9, 5),
                Reason = "Ferie"
            }
        };

        if (await db.DoctorAbsences.AnyAsync())
        {
            db.DoctorAbsences.RemoveRange(db.DoctorAbsences);
            await db.SaveChangesAsync();
        }

        db.DoctorAbsences.AddRange(seedAbsences);
        await db.SaveChangesAsync();
    }
    */
}