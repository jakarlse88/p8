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
    public class AllergyControllerTests
    {
        [Fact]
        public async Task TestGet()
        {
            // Arrange
            var models = GenerateAllergys();

            var mockService = new Mock<IAllergyService>();
            mockService
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(models);

            var controller = new AllergyController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var modelResult = Assert.IsAssignableFrom<IEnumerable<AllergyDTO>>(actionResult.Value);
            Assert.Equal(6, modelResult.Count());
        }

        [Fact]
        public async Task TestGetNoResults()
        {
            // Arrange
            var models = new List<AllergyDTO>();

            var mockService = new Mock<IAllergyService>();
            mockService
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(models);

            var controller = new AllergyController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var modelResult = Assert.IsAssignableFrom<IEnumerable<AllergyDTO>>(actionResult.Value);
            Assert.Empty(modelResult);
        }

        /**
         * ============================
         * Internal helper methods
         *  ===========================
         */
        private static IEnumerable<AllergyDTO> GenerateAllergys()
        {
            var consultants = new List<AllergyDTO>
            {
                new AllergyDTO
                {
                    Id = 1,
                },
                new AllergyDTO
                {
                    Id = 2,
                },
                new AllergyDTO
                {
                    Id = 3,
                },
                new AllergyDTO
                {
                    Id = 4,
                },
                new AllergyDTO
                {
                    Id = 5,
                },
                new AllergyDTO
                {
                    Id = 6,
                },
            };

            return consultants;
        }
    }
}