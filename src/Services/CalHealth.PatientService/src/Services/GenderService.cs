using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CalHealth.PatientService.Models;
using CalHealth.PatientService.Repositories;

namespace CalHealth.PatientService.Services
{
    public class GenderService : IGenderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenderService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GenderDTO>> GetAllAsync()
        {
            var result = await _unitOfWork
                .GenderRepository
                .GetAllAsync();

            var mappedResult = _mapper.Map<IEnumerable<GenderDTO>>(result);

            return mappedResult;
        }
    }
}