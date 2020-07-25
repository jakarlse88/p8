using System;
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

        [Theory]
        [InlineData("Test", "", "2000.01.01")]
        [InlineData("", "Test", "2000.01.01")]
        [InlineData("", "", "")]
        public async Task TestPatientExistsNullArg(string firstName, string lastName, string dateOfBirth)
        {
            // Arrange
            var controller = new PatientController(null);

            // Act
            var result = await controller.PatientExists(firstName, lastName, dateOfBirth);

            // Assert
            var actionResult = Assert.IsAssignableFrom<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task TestPatientExistsValidArgs()
        {
            // Arrange
            const string firstName = "Test";
            const string lastName = "McTest";
            const string dateOfBirth = "01.01.2000";
            
            var models = GeneratePatients();

            var mockService = new Mock<IPatientService>();
            mockService
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(models);

            var controller = new PatientController(mockService.Object);
            
            // Act
            var response = await controller.PatientExists(firstName, lastName, dateOfBirth);
            
            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            var model = Assert.IsAssignableFrom<bool>(actionResult.Value);
            Assert.True(model);
        }

        [Fact]
        public async Task TestPatientExistsInvalidArgs()
        {
            // Arrange
            const string firstName = "Untest";
            const string lastName = "McTest";
            const string dateOfBirth = "01.01.2000";
            
            var models = GeneratePatients();

            var mockService = new Mock<IPatientService>();
            mockService
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(models);

            var controller = new PatientController(mockService.Object);
            
            // Act
            var response = await controller.PatientExists(firstName, lastName, dateOfBirth);
            
            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            var model = Assert.IsAssignableFrom<bool>(actionResult.Value);
            Assert.False(model);
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
                    FirstName = "Test",
                    LastName = "McTest",
                    DateOfBirth = new DateTime(2000,1,1)
                },
                new PatientDTO
                {
                    Id = 2,
                    FirstName = "Test",
                    LastName = "McTest",
                    DateOfBirth = new DateTime(2000,1,1)
                },
                new PatientDTO
                {
                    Id = 3,
                    FirstName = "Test",
                    LastName = "McTest",
                    DateOfBirth = new DateTime(2000,1,1)
                },
                new PatientDTO
                {
                    Id = 4,
                    FirstName = "Test",
                    LastName = "McTest",
                    DateOfBirth = new DateTime(2000,1,1)
                },
                new PatientDTO
                {
                    Id = 5,
                    FirstName = "Test",
                    LastName = "McTest",
                    DateOfBirth = new DateTime(2000,1,1)
                },
                new PatientDTO
                {
                    Id = 6,
                    FirstName = "Test",
                    LastName = "McTest",
                    DateOfBirth = new DateTime(2000,1,1)
                },
            };

            return consultants;
        }
    }
}