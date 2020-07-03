using AutoMapper;
using CalHealth.CalendarService.Models.DTOs;

namespace CalHealth.CalendarService.Models.MappingProfiles
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