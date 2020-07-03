using AutoMapper;
using CalHealth.CalendarService.Models.DTOs;

namespace CalHealth.CalendarService.Models.MappingProfiles
{
    public class ConsultantMappingProfile : Profile
    {
        public ConsultantMappingProfile()
        {
            CreateMap<Consultant, ConsultantDTO>()
                .ForMember(dto => dto.Specialty, 
                    action => action.MapFrom(
                        (entity, dto) => entity.Specialty.Type));

            CreateMap<ConsultantDTO, Consultant>();
        }
    }
}