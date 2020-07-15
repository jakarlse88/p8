using AutoMapper;

namespace CalHealth.PatientService.Models.MappingProfiles
{
    public class PatientMappingProfile : Profile
    {
        public PatientMappingProfile()
        {
            CreateMap<Patient, PatientDTO>();
        }
    }
}