using AutoMapper;

namespace CalHealth.PatientService.Models.MappingProfiles
{
    public class GenderMappingProfile : Profile
    {
        public GenderMappingProfile()
        {
            CreateMap<Gender, GenderDTO>();
        }
    }
}