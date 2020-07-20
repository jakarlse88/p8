using AutoMapper;

namespace CalHealth.BookingService.Models.MappingProfiles
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