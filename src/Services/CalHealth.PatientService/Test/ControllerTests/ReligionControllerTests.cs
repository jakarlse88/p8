using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalHealth.PatientService.Controllers;
using CalHealth.PatientService.Models;
using CalHealth.PatientService.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CalHealth.PatientService.Test.ControllerTests
{
    public class ReligionControllerTests
    {
        [Fact]
        public async Task TestGet()
        {
            // Arrange
            var models = GenerateReligions();

            var mockService = new Mock<IReligionService>();
            mockService
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(models);

            var controller = new ReligionController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var modelResult = Assert.IsAssignableFrom<IEnumerable<ReligionDTO>>(actionResult.Value);
            Assert.Equal(6, modelResult.Count());
        }

        [Fact]
        public async Task TestGetNoResults()
        {
            // Arrange
            var models = new List<ReligionDTO>();

            var mockService = new Mock<IReligionService>();
            mockService
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(models);

            var controller = new ReligionController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var modelResult = Assert.IsAssignableFrom<IEnumerable<ReligionDTO>>(actionResult.Value);
            Assert.Empty(modelResult);
        }

        /**
         * ============================
         * Internal helper methods
         *  ===========================
         */
        private static IEnumerable<ReligionDTO> GenerateReligions()
        {
            var consultants = new List<ReligionDTO>
            {
                new ReligionDTO
                {
                    Id = 1,
                },
                new ReligionDTO
                {
                    Id = 2,
                },
                new ReligionDTO
                {
                    Id = 3,
                },
                new ReligionDTO
                {
                    Id = 4,
                },
                new ReligionDTO
                {
                    Id = 5,
                },
                new ReligionDTO
                {
                    Id = 6,
                },
            };

            return consultants;
        }
    }
}