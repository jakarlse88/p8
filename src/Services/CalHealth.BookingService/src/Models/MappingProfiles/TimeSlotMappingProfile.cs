using AutoMapper;

namespace CalHealth.BookingService.Models.MappingProfiles
{
    public class TimeSlotMappingProfile : Profile
    {
        public TimeSlotMappingProfile()
        {
            CreateMap<TimeSlot, TimeSlotDTO>();
            CreateMap<TimeSlotDTO, TimeSlot>();
        }
    }
}