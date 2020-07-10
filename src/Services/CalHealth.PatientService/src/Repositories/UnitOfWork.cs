using System.Threading.Tasks;
using CalHealth.PatientService.Data;

namespace CalHealth.PatientService.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PatientContext _context;
        private IAllergyRepository _allergyRepository;
        private IReligionRepository _religionRepository;
        private IGenderRepository _genderRepository;

        public UnitOfWork(PatientContext context)
        {
            _context = context;
        }

        public IAllergyRepository AllergyRepository =>
            _allergyRepository ??= new AllergyRepository(_context);

        public IReligionRepository ReligionRepository =>
            _religionRepository ??= new ReligionRepository(_context);

        public IGenderRepository GenderRepository =>
            _genderRepository ??= new GenderRepository(_context);
        
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