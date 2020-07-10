using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CalHealth.PatientService.Models;
using CalHealth.PatientService.Repositories;

namespace CalHealth.PatientService.Services
{
    public class AllergyService : IAllergyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AllergyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AllergyDTO>> GetAllAsync()
        {
            var result = await _unitOfWork
                .AllergyRepository
                .GetAllAsync();

            var mappedResult = _mapper.Map<IEnumerable<AllergyDTO>>(result);

            return mappedResult;
        }
    }
}