using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CalHealth.BookingService.Models;
using CalHealth.BookingService.Repositories;

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

        /// <summary>
        /// Asynchronously get all <see cref="Consultant"/> entities.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ConsultantDTO>> GetAllAsDTOAsync()
        {
            var result = await _unitOfWork.ConsultantRepository.GetAllAsync(eager: true);

            var mappedResult = _mapper.Map<IEnumerable<ConsultantDTO>>(result);

            return mappedResult;
        }
    }
}