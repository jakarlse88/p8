using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalHealth.CalendarService.Models;
using CalHealth.CalendarService.Repositories;
using CalHealth.CalendarService.Repositories.Interfaces;
using CalHealth.CalendarService.Services;
using Moq;
using Xunit;

namespace CalHealth.CalendarService.Test.ServiceTests
{
    public class TimeSlotServiceTests
    {
        [Fact]
        public async Task TestGetAll()
        {
            // Arrange
            var timeSlots = GenerateTimeSlots();
            
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.TimeSlotRepository.GetAll())
                .ReturnsAsync(timeSlots);

            var service = new TimeSlotService(mockUnitOfWork.Object, null);
            
            // Act
            var result = await service.GetAllAsDTOAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count());
        }
        
        [Fact]
        public async Task TestGetAllNoResults()
        {
            // Arrange
            var timeSlots = new List<TimeSlot>();
            
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.TimeSlotRepository.GetAll())
                .ReturnsAsync(timeSlots);

            var service = new TimeSlotService(mockUnitOfWork.Object, null);
            
            // Act
            var result = await service.GetAllAsDTOAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /**
         * ============================
         * Internal helper methods
         *  ===========================
         */

        private IEnumerable<TimeSlot> GenerateTimeSlots()
        {
            const int openingTime = 8;
            const int closingTime = 9;
            const int minutesPerHour = 60;
            const int sessionLength = 10;

            var timeSlots = new List<TimeSlot>();

            for (int hour = openingTime, index = 1; hour < closingTime; hour++)
            {
                for (int minute = 0; minute < (minutesPerHour / sessionLength); minute++)
                {
                    timeSlots.Add(
                        new TimeSlot
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