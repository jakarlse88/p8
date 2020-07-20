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
    public class ReligionServiceTests
    {
        private readonly IMapper _mapper;

        public ReligionServiceTests()
        {
            var config = new MapperConfiguration(opt => { opt.AddProfile(new ReligionMappingProfile()); });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task TestGetAll()
        {
            // Arrange
            var religions = GenerateReligions();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.ReligionRepository.GetAllAsync(It.IsAny<bool>()))
                .ReturnsAsync(religions);

            var service = new ReligionService(mockUnitOfWork.Object, _mapper);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.IsAssignableFrom<IEnumerable<ReligionDTO>>(result);
        }

        [Fact]
        public async Task TestGetAllNoResults()
        {
            // Arrange
            var religions = new List<Religion>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.ReligionRepository.GetAllAsync(It.IsAny<bool>()))
                .ReturnsAsync(religions);

            var service = new ReligionService(mockUnitOfWork.Object, _mapper);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsAssignableFrom<IEnumerable<ReligionDTO>>(result);
        }

        /**
         * ============================
         * Internal helper methods
         *  ===========================
         */
        private static IEnumerable<Religion> GenerateReligions()
        {
            var religions = new List<Religion>
            {
                new Religion
                {
                    Id = 1
                },
                new Religion
                {
                    Id = 2
                }
            };

            return religions;
        }
    }
}