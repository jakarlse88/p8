using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalHealth.CalendarService.Controllers;
using CalHealth.CalendarService.Models.DTOs;
using CalHealth.CalendarService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CalHealth.CalendarService.Test.ControllerTests
{
    public class ConsultantControllerTests
    {
        [Fact]
        public async Task TestGet()
        {
            // Arrange
            var models = GenerateConsultants();
            
            var mockService = new Mock<IConsultantService>();
            mockService
                .Setup(x => x.GetAllAsDTOAsync())
                .ReturnsAsync(models);
            
            var controller = new ConsultantController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var modelResult = Assert.IsAssignableFrom<IEnumerable<ConsultantDTO>>(actionResult.Value);
            Assert.Equal(6, modelResult.Count());
        }
        
        [Fact]
        public async Task TestGetNoResults()
        {
            // Arrange
            var models = new List<ConsultantDTO>();
            
            var mockService = new Mock<IConsultantService>();
            mockService
                .Setup(x => x.GetAllAsDTOAsync())
                .ReturnsAsync(models);
            
            var controller = new ConsultantController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var modelResult = Assert.IsAssignableFrom<IEnumerable<ConsultantDTO>>(actionResult.Value);
            Assert.Empty(modelResult);
        }
        
        /**
         * ============================
         * Internal helper methods
         *  ===========================
         */

        private static IEnumerable<ConsultantDTO> GenerateConsultants()
        {
            var consultants = new List<ConsultantDTO>
            {
                new ConsultantDTO
                {
                    Id = 1,
                    GenderId = 2,
                    FirstName = "Sophie",
                    LastName = "Harrington",
                    DateOfBirth = new DateTime(1985, 5, 24)
                },
                new ConsultantDTO
                {
                    Id = 2,
                    GenderId = 1,
                    FirstName = "Kilian",
                    LastName = "Lopez",
                    DateOfBirth = new DateTime(1967, 2, 5)
                },
                new ConsultantDTO
                {
                    Id = 3,
                    GenderId = 2,
                    FirstName = "Aya",
                    LastName = "Ahmed",
                    DateOfBirth = new DateTime(1990, 1, 9)
                },
                new ConsultantDTO
                {
                    Id = 4,
                    GenderId = 2,
                    FirstName = "Hyeo-jin",
                    LastName = "Lim",
                    DateOfBirth = new DateTime(1980, 2, 29)
                },
                new ConsultantDTO
                {
                    Id = 5,
                    GenderId = 1,
                    FirstName = "Lasse",
                    LastName = "Hansson",
                    DateOfBirth = new DateTime(1977, 12, 7)
                },
                new ConsultantDTO
                {
                    Id = 6,
                    GenderId = 1,
                    FirstName = "Abe",
                    LastName = "Shiraishi",
                    DateOfBirth = new DateTime(1973, 9, 5)
                }
            };

            return consultants;
        }
    }
}