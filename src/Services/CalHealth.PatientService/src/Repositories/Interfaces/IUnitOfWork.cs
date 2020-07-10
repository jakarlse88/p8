using System.Threading.Tasks;

namespace CalHealth.PatientService.Repositories
{
    public interface IUnitOfWork
    {
        public IAllergyRepository AllergyRepository { get; }
        public IReligionRepository ReligionRepository { get; }
        public IGenderRepository GenderRepository { get; }

        Task CommitAsync();
        Task RollbackAsync();
    }
}
