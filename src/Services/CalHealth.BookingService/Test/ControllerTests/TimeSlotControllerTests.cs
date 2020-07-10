using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalHealth.BookingService.Controllers;
using CalHealth.BookingService.Models;
using CalHealth.BookingService.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CalHealth.BookingService.Test.ControllerTests
{
    public class TimeSlotControllerTests
    {
        [Fact]
        public async Task TestGet()
        {
            // Arrange
            var models = GenerateTimeSlotDTOs();
            
            var mockService = new Mock<ITimeSlotService>();
            mockService
                .Setup(x => x.GetAllAsDTOAsync())
                .ReturnsAsync(models);
            
            var controller = new TimeSlotController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var modelResult = Assert.IsAssignableFrom<IEnumerable<TimeSlotDTO>>(actionResult.Value);
            Assert.Equal(6, modelResult.Count());
        }
        
        [Fact]
        public async Task TestGetNoResults()
        {
            // Arrange
            var models = new List<TimeSlotDTO>();
            
            var mockService = new Mock<ITimeSlotService>();
            mockService
                .Setup(x => x.GetAllAsDTOAsync())
                .ReturnsAsync(models);
            
            var controller = new TimeSlotController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            var modelResult = Assert.IsAssignableFrom<IEnumerable<TimeSlotDTO>>(actionResult.Value);
            Assert.Empty(modelResult);
        }

        /**
         * ============================
         * Internal helper methods
         *  ===========================
         */

        private static IEnumerable<TimeSlotDTO> GenerateTimeSlotDTOs()
        {
            const int openingTime = 8;
            const int closingTime = 9;
            const int minutesPerHour = 60;
            const int sessionLength = 10;

            var timeSlots = new List<TimeSlotDTO>();

            for (int hour = openingTime, index = 1; hour < closingTime; hour++)
            {
                for (int minute = 0; minute < (minutesPerHour / sessionLength); minute++)
                {
                    timeSlots.Add(
                        new TimeSlotDTO
                        {
                            Id = index,
                            StartTime = new DateTime(2020, 1, 1, hour, (minute * 10), 0),
                            EndTime =
                                minute == 5
                                    ? new DateTime(2020, 1, 1, (hour + 1), 0, 0)
                                    : new DateTime(2020, 1, 1, hour, ((minute + 1) * 10), 0)
                        }
                    );

                    index++;
                }
            }

            return timeSlots;
        }
    }
}