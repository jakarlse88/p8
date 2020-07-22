using AutoMapper;

namespace CalHealth.PatientService.Models.MappingProfiles
{
    public class ReligionMappingProfile : Profile
    {
        public ReligionMappingProfile()
        {
            CreateMap<Religion, ReligionDTO>();
        }
    }
}