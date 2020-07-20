using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CalHealth.PatientService.Models;
using CalHealth.PatientService.Repositories;

namespace CalHealth.PatientService.Services
{
    public class ReligionService : IReligionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReligionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReligionDTO>> GetAllAsync()
        {
            var result = await _unitOfWork
                .ReligionRepository
                .GetAllAsync();

            var mappedResult = _mapper.Map<IEnumerable<ReligionDTO>>(result);

            return mappedResult;
        }
    }
}