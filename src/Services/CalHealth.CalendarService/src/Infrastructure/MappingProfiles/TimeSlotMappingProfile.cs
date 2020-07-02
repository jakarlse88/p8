using AutoMapper;
using CalHealth.CalendarService.Models;
using CalHealth.CalendarService.Models.DTOs;

namespace CalHealth.CalendarService.Infrastructure.MappingProfiles
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