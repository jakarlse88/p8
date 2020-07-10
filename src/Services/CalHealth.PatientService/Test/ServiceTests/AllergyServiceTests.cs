using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CalHealth.PatientService.Models;
using CalHealth.PatientService.Models.MappingProfiles;
using CalHealth.PatientService.Repositories;
using CalHealth.PatientService.Services;
using Moq;
using Xunit;

namespace CalHealth.PatientService.Test.ServiceTests
{
    public class AllergyServiceTests
    {
        private readonly IMapper _mapper;

        public AllergyServiceTests()
        {
            var config = new MapperConfiguration(opt => { opt.AddProfile(new AllergyMappingProfile()); });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task TestGetAll()
        {
            // Arrange
            var allergies = GenerateAllergies();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.AllergyRepository.GetAllAsync())
                .ReturnsAsync(allergies);

            var service = new AllergyService(mockUnitOfWork.Object, _mapper);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.IsAssignableFrom<IEnumerable<AllergyDTO>>(result);
        }

        [Fact]
        public async Task TestGetAllNoResults()
        {
            // Arrange
            var allergies = new List<Allergy>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.AllergyRepository.GetAllAsync())
                .ReturnsAsync(allergies);

            var service = new AllergyService(mockUnitOfWork.Object, _mapper);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsAssignableFrom<IEnumerable<AllergyDTO>>(result);
        }

        /**
         * ============================
         * Internal helper methods
         *  ===========================
         */
        private static IEnumerable<Allergy> GenerateAllergies()
        {
            var allergies = new List<Allergy>
            {
                new Allergy
                {
                    Id = 1
                },
                new Allergy
                {
                    Id = 2
                }
            };

            return allergies;
        }
    }
}