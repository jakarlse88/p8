using System.Threading.Tasks;
using CalHealth.PatientService.Models;

namespace CalHealth.PatientService.Repositories
{
    public interface IUnitOfWork
    {
        public IRepository<Allergy> AllergyRepository { get; }
        public IRepository<Religion> ReligionRepository { get; }
        public IRepository<Gender> GenderRepository { get; }
        public IRepository<Patient> PatientRepository { get; }

        Task CommitAsync();
        Task RollbackAsync();
    }
}
