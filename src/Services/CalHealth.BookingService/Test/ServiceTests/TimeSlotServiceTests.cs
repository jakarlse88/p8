using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CalHealth.BookingService.Models;
using CalHealth.BookingService.Models.MappingProfiles;
using CalHealth.BookingService.Repositories;
using CalHealth.BookingService.Services;
using Moq;
using Xunit;

namespace CalHealth.BookingService.Test.ServiceTests
{
    public class TimeSlotServiceTests
    {
        private readonly IMapper _mapper;

        public TimeSlotServiceTests()
        {
            var config = new MapperConfiguration(opt => { opt.AddProfile(new TimeSlotMappingProfile()); });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task TestGetAll()
        {
            // Arrange
            var timeSlots = GenerateTimeSlots();
            
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.TimeSlotRepository.GetAllAsync(It.IsAny<bool>()))
                .ReturnsAsync(timeSlots);

            var service = new TimeSlotService(mockUnitOfWork.Object, _mapper);
            
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
                .Setup(x => x.TimeSlotRepository.GetAllAsync(It.IsAny<bool>()))
                .ReturnsAsync(timeSlots);

            var service = new TimeSlotService(mockUnitOfWork.Object, _mapper);
            
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