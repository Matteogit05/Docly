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
        bool isAbsent = await _context.DoctorAbsences
            .AnyAsync(a => a.DoctorId == doctorId 
                        && requestedDate.Date >= a.StartDate.Date 
                        && requestedDate.Date <= a.EndDate.Date);

        if (isAbsent)
        {
            return new List<TimeSpan>(); 
        }
        
        var dayOfWeek = requestedDate.DayOfWeek;

        var schedules = await _context.DoctorSchedules
            .Where(s => s.DoctorId == doctorId && s.DayOfWeek == dayOfWeek)
            .ToListAsync();

        var existingAppointments = await _context.Appointments
            .Where(a => a.DoctorId == doctorId 
                     && a.StartTime.Date == requestedDate.Date 
                     && a.Status != 2) 
            .ToListAsync();

        var availableSlots = new List<TimeSpan>();
        var now = DateTime.Now;

        foreach (var schedule in schedules)
        {
            var currentSlotStart = schedule.StartTime;
            var slotDuration = TimeSpan.FromMinutes(schedule.SlotDurationMinutes);

            while (currentSlotStart.Add(slotDuration) <= schedule.EndTime)
            {
                var slotStartDateTime = requestedDate.Date.Add(currentSlotStart);
                var slotEndDateTime = slotStartDateTime.Add(slotDuration);

                bool isOverlapping = existingAppointments.Any(a =>
                    (slotStartDateTime >= a.StartTime && slotStartDateTime < a.EndTime) ||
                    (slotEndDateTime > a.StartTime && slotEndDateTime <= a.EndTime) ||
                    (slotStartDateTime <= a.StartTime && slotEndDateTime >= a.EndTime)
                );

                if (!isOverlapping && slotStartDateTime > now)
                {
                    availableSlots.Add(currentSlotStart);
                }

                currentSlotStart = currentSlotStart.Add(slotDuration);
            }
        }

        return availableSlots.OrderBy(s => s).ToList();
    }
}