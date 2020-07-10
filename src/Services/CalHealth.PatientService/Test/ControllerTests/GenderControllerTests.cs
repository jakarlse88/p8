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
    public class GenderControllerTests
    {
        [Fact]
        public async Task TestGet()
        {
            // Arrange
            var models = GenerateGenders();

            var mockService = new Mock<IGenderService>();
            mockService
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(models);

            var controller = new GenderController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var modelResult = Assert.IsAssignableFrom<IEnumerable<GenderDTO>>(actionResult.Value);
            Assert.Equal(6, modelResult.Count());
        }

        [Fact]
        public async Task TestGetNoResults()
        {
            // Arrange
            var models = new List<GenderDTO>();

            var mockService = new Mock<IGenderService>();
            mockService
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(models);

            var controller = new GenderController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var modelResult = Assert.IsAssignableFrom<IEnumerable<GenderDTO>>(actionResult.Value);
            Assert.Empty(modelResult);
        }

        /**
         * ============================
         * Internal helper methods
         *  ===========================
         */
        private static IEnumerable<GenderDTO> GenerateGenders()
        {
            var consultants = new List<GenderDTO>
            {
                new GenderDTO
                {
                    Id = 1,
                },
                new GenderDTO
                {
                    Id = 2,
                },
                new GenderDTO
                {
                    Id = 3,
                },
                new GenderDTO
                {
                    Id = 4,
                },
                new GenderDTO
                {
                    Id = 5,
                },
                new GenderDTO
                {
                    Id = 6,
                },
            };

            return consultants;
        }
    }
}