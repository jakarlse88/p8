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
    public class PatientControllerTests
    {
        [Fact]
        public async Task TestGet()
        {
            // Arrange
            var models = GeneratePatients();

            var mockService = new Mock<IPatientService>();
            mockService
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(models);

            var controller = new PatientController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var modelResult = Assert.IsAssignableFrom<IEnumerable<PatientDTO>>(actionResult.Value);
            Assert.Equal(6, modelResult.Count());
        }

        [Fact]
        public async Task TestGetNoResults()
        {
            // Arrange
            var models = new List<PatientDTO>();

            var mockService = new Mock<IPatientService>();
            mockService
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(models);

            var controller = new PatientController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var modelResult = Assert.IsAssignableFrom<IEnumerable<PatientDTO>>(actionResult.Value);
            Assert.Empty(modelResult);
        }

        /**
         * ============================
         * Internal helper methods
         *  ===========================
         */
        private static IEnumerable<PatientDTO> GeneratePatients()
        {
            var consultants = new List<PatientDTO>
            {
                new PatientDTO
                {
                    Id = 1,
                },
                new PatientDTO
                {
                    Id = 2,
                },
                new PatientDTO
                {
                    Id = 3,
                },
                new PatientDTO
                {
                    Id = 4,
                },
                new PatientDTO
                {
                    Id = 5,
                },
                new PatientDTO
                {
                    Id = 6,
                },
            };

            return consultants;
        }
    }
}