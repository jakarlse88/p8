using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CalHealth.BookingService.Models;
using CalHealth.BookingService.Repositories;

namespace CalHealth.BookingService.Services
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TimeSlotService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TimeSlotDTO>> GetAllAsDTOAsync()
        {
            var result = await _unitOfWork.TimeSlotRepository.GetAllAsync();

            var mappedResult = _mapper.Map<IEnumerable<TimeSlotDTO>>(result);
            
            return mappedResult;
        }
    }
}