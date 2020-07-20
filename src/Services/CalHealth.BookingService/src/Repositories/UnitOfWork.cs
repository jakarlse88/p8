using CalHealth.BookingService.Data;
using System.Threading.Tasks;
using CalHealth.BookingService.Models;

namespace CalHealth.BookingService.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookingContext _context;
        private IRepository<TimeSlot> _timeSlotRepository;
        private IRepository<Consultant> _consultantRepository;
        private IRepository<Appointment> _appointmentRepository;
        private IRepository<Day> _dayRepository;
        private IRepository<Week> _weekRepository;

        public UnitOfWork(BookingContext context)
        {
            _context = context;
        }

        public IRepository<TimeSlot> TimeSlotRepository =>
            _timeSlotRepository ??= new Repository<TimeSlot>(_context);

        public IRepository<Consultant> ConsultantRepository =>
            _consultantRepository ??= new Repository<Consultant>(_context);

        public IRepository<Appointment> AppointmentRepository =>
            _appointmentRepository ??= new Repository<Appointment>(_context);

        public IRepository<Day> DayRepository =>
            _dayRepository ??= new Repository<Day>(_context);

        public IRepository<Week> WeekRepository =>
            _weekRepository ??= new Repository<Week>(_context);

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RollbackAsync()
        {
            await _context.DisposeAsync();
        }
    }
}