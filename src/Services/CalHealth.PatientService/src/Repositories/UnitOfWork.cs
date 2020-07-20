using System.Threading.Tasks;
using CalHealth.PatientService.Data;
using CalHealth.PatientService.Models;

namespace CalHealth.PatientService.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PatientContext _context;
        private IRepository<Allergy> _allergyRepository;
        private IRepository<Religion> _religionRepository;
        private IRepository<Gender> _genderRepository;
        private IRepository<Patient> _patientRepository;

        public UnitOfWork(PatientContext context)
        {
            _context = context;
        }

        public IRepository<Allergy> AllergyRepository =>
            _allergyRepository ??= new Repository<Allergy>(_context);

        public IRepository<Religion> ReligionRepository =>
            _religionRepository ??= new Repository<Religion>(_context);

        public IRepository<Gender> GenderRepository =>
            _genderRepository ??= new Repository<Gender>(_context);

        public IRepository<Patient> PatientRepository =>
            _patientRepository ??= new Repository<Patient>(_context);

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