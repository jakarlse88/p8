using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CalHealth.BookingService.Models;
using CalHealth.BookingService.Repositories;
using Serilog;

namespace CalHealth.BookingService.Services
{
    public class ConsultantService : IConsultantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ConsultantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ConsultantDTO>> GetAllAsDTOAsync()
        {
            var result = await _unitOfWork.ConsultantRepository.GetAllAsync(
                c => c.Specialty,
                c => c.Gender);

            Log.Information("Result: {@consultant}", result.First());
            var mappedResult = _mapper.Map<IEnumerable<ConsultantDTO>>(result);

            return mappedResult;
        }
    }
}