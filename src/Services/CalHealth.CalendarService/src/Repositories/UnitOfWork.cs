﻿using System.Threading.Tasks;
using CalHealth.CalendarService.Data;
using CalHealth.CalendarService.Repositories.Interfaces;

namespace CalHealth.CalendarService.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CalendarContext _context;
        private ITimeSlotRepository _timeSlotRepository;

        public UnitOfWork(CalendarContext context)
        {
            _context = context;
        }

        public ITimeSlotRepository TimeSlotRepository =>
            _timeSlotRepository ??= new TimeSlotRepository(_context);
        
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