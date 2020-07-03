using AutoMapper;
using CalHealth.CalendarService.Models.DTOs;

namespace CalHealth.CalendarService.Models.MappingProfiles
{
    public class ConsultantMappingProfile : Profile
    {
        public ConsultantMappingProfile()
        {
            CreateMap<Consultant, ConsultantDTO>();
            CreateMap<ConsultantDTO, Consultant>();
        }
    }
}