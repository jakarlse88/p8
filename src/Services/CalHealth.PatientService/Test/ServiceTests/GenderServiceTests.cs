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
    public class GenderServiceTests
    {
        private readonly IMapper _mapper;

        public GenderServiceTests()
        {
            var config = new MapperConfiguration(opt => { opt.AddProfile(new GenderMappingProfile()); });

            _mapper = config.CreateMapper();
        }
        
        [Fact]
        public async Task TestGetAll()
        {
            // Arrange
            var consultants = GenerateGenders();
            
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.GenderRepository.GetAllAsync())
                .ReturnsAsync(consultants);

            var service = new GenderService(_mapper, mockUnitOfWork.Object);
            
            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.IsAssignableFrom<IEnumerable<GenderDTO>>(result);
        }
        
        [Fact]
        public async Task TestGetAllNoResults()
        {
            // Arrange
            var timeSlots = new List<Gender>();
            
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.GenderRepository.GetAllAsync())
                .ReturnsAsync(timeSlots);

            var service = new GenderService(_mapper, mockUnitOfWork.Object);
            
            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsAssignableFrom<IEnumerable<GenderDTO>>(result);
        }

        /**
         * ============================
         * Internal helper methods
         *  ===========================
         */

        private static IEnumerable<Gender> GenerateGenders()
        {
            var genders = new List<Gender>
            {
                new Gender
                {
                    Id = 1
                },
                new Gender
                {
                    Id = 2
                }
            };

            return genders;
        }
    }
}