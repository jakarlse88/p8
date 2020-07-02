using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalHealth.CalendarService.Repositories.Interfaces
{
    public interface IRepositoryBase<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
    }
}