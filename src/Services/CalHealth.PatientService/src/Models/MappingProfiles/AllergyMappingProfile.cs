using AutoMapper;

namespace CalHealth.PatientService.Models.MappingProfiles
{
    public class AllergyMappingProfile : Profile
    {
        public AllergyMappingProfile()
        {
            CreateMap<Allergy, AllergyDTO>();
        }
    }
}