using CalHealth.BookingService.Data;
using System.Threading.Tasks;

namespace CalHealth.BookingService.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookingContext _context;
        private ITimeSlotRepository _timeSlotRepository;
        private IConsultantRepository _consultantRepository;
        private IAppointmentRepository _appointmentRepository;

        public UnitOfWork(BookingContext context)
        {
            _context = context;
        }

        public ITimeSlotRepository TimeSlotRepository =>
            _timeSlotRepository ??= new TimeSlotRepository(_context);

        public IConsultantRepository ConsultantRepository =>
            _consultantRepository ??= new ConsultantRepository(_context);

        public IAppointmentRepository AppointmentRepository =>
            _appointmentRepository ??= new AppointmentRepository(_context);

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