using Microsoft.EntityFrameworkCore;

public class BookingService
{
    private readonly ApplicationDbContext _context;

    public BookingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TimeSpan>> GetAvailableSlotsAsync(int doctorId, DateTime requestedDate)
    {
        // 0. CONTROLLO ASSENZE (Ferie, Malattia, ecc.)
        // Verifichiamo se la data richiesta cade all'interno di un periodo di assenza del medico
        bool isAbsent = await _context.DoctorAbsences
            .AnyAsync(a => a.DoctorId == doctorId 
                        && requestedDate.Date >= a.StartDate.Date 
                        && requestedDate.Date <= a.EndDate.Date);

        if (isAbsent)
        {
            // Il medico è assente per questa data, restituiamo una lista vuota (nessuno slot disponibile)
            return new List<TimeSpan>(); 
        }
        
        // 1. Troviamo il giorno della settimana (es. Lunedì)
        var dayOfWeek = requestedDate.DayOfWeek;

        // 2. Recuperiamo il turno del medico per quel giorno (potrebbe avere mattina e pomeriggio)
        var schedules = await _context.DoctorSchedules
            .Where(s => s.DoctorId == doctorId && s.DayOfWeek == dayOfWeek)
            .ToListAsync();

        // 3. Recuperiamo gli appuntamenti GIA' FISSATI per quel giorno
        // Escludiamo quelli cancellati (Status 2 come da tue note)
        var existingAppointments = await _context.Appointments
            .Where(a => a.DoctorId == doctorId 
                     && a.StartTime.Date == requestedDate.Date 
                     && a.Status != 2) 
            .ToListAsync();

        var availableSlots = new List<TimeSpan>();
        var now = DateTime.Now;

        // 4. Generiamo gli slot per ogni turno (es. prima mattina, poi pomeriggio)
        foreach (var schedule in schedules)
        {
            var currentSlotStart = schedule.StartTime;
            var slotDuration = TimeSpan.FromMinutes(schedule.SlotDurationMinutes);

            // Ciclo finché c'è spazio per un'intera visita prima della fine del turno
            while (currentSlotStart.Add(slotDuration) <= schedule.EndTime)
            {
                var slotStartDateTime = requestedDate.Date.Add(currentSlotStart);
                var slotEndDateTime = slotStartDateTime.Add(slotDuration);

                // Controllo sovrapposizioni con appuntamenti esistenti
                bool isOverlapping = existingAppointments.Any(a =>
                    (slotStartDateTime >= a.StartTime && slotStartDateTime < a.EndTime) ||
                    (slotEndDateTime > a.StartTime && slotEndDateTime <= a.EndTime) ||
                    (slotStartDateTime <= a.StartTime && slotEndDateTime >= a.EndTime)
                );

                // Aggiungiamo lo slot se NON si sovrappone ed è nel futuro (non nel passato)
                if (!isOverlapping && slotStartDateTime > now)
                {
                    availableSlots.Add(currentSlotStart);
                }

                // Avanziamo di 15 minuti (o della durata definita)
                currentSlotStart = currentSlotStart.Add(slotDuration);
            }
        }

        return availableSlots.OrderBy(s => s).ToList(); // Restituisce gli orari ordinati
    }
}