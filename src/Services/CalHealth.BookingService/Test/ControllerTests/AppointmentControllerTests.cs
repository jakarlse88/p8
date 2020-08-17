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
    public class AppointmentControllerTests
    {
        [Fact]
        public async Task TestGetAppointmentIdInvalid()
        {
            // Arrange
            var controller = new AppointmentController(null, null);

            // Act
            var response = await controller.Get(0);

            // Assert
            var actionResult = Assert.IsAssignableFrom<BadRequestObjectResult>(response.Result);
            Assert.Equal("appointmentId", actionResult.Value);
        }

        [Fact]
        public async Task TestGetResultNull()
        {
            // Arrange
            var mockService = new Mock<IAppointmentService>();
            mockService
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null as AppointmentDTO);

            var controller = new AppointmentController(mockService.Object, null);

            // Act
            var response = await controller.Get(1);

            // Assert
            var actionResult = Assert.IsAssignableFrom<NotFoundResult>(response.Result);
        }

        [Fact]
        public async Task TestGet()
        {
            // Arrange
            var dto = new AppointmentDTO { Id = 1 };

            var mockService = new Mock<IAppointmentService>();
            mockService
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(dto);

            var controller = new AppointmentController(mockService.Object, null);

            // Act
            var response = await controller.Get(1);

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            var modelResult = Assert.IsAssignableFrom<AppointmentDTO>(actionResult.Value);
            Assert.Equal(1, modelResult.Id);
        }

        [Fact]
        public async Task TestGetByConsultantIdInvalid()
        {
            // Arrange
            var controller = new AppointmentController(null, null);

            // Act
            var response = await controller.GetByConsultant(0);

            // Assert
            var actionResult = Assert.IsAssignableFrom<BadRequestObjectResult>(response.Result);
            Assert.Equal("consultantId", actionResult.Value);
        }

        [Fact]
        public async Task TestGetByConsultantId()
        {
            // Arrange
            var models = new List<AppointmentDTO> { new AppointmentDTO { Id = 1, ConsultantId = 1 } };

            var mockService = new Mock<IAppointmentService>();
            mockService
                .Setup(x => x.GetByConsultantAsync(1))
                .ReturnsAsync(models);

            var controller = new AppointmentController(mockService.Object, null);

            // Act
            var response = await controller.GetByConsultant(1);

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            var modelResult = Assert.IsAssignableFrom<IEnumerable<AppointmentDTO>>(actionResult.Value);
            Assert.Equal(1, modelResult.First().ConsultantId);
        }

        [Fact]
        public async Task TestGetByConsultantIdNoResults()
        {
            // Arrange
            var models = new List<AppointmentDTO>();

            var mockService = new Mock<IAppointmentService>();
            mockService
                .Setup(x => x.GetByConsultantAsync(1))
                .ReturnsAsync(models);

            var controller = new AppointmentController(mockService.Object, null);

            // Act
            var response = await controller.GetByConsultant(1);

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            var modelResult = Assert.IsAssignableFrom<IEnumerable<AppointmentDTO>>(actionResult.Value);
            Assert.Empty(modelResult);
        }

        [Fact]
        public async Task TestPostDtoNull()
        {
            // Arrange
            var controller = new AppointmentController(null, null);

            // Act
            var response = await controller.Post(null);

            // Assert
            var actionResult = Assert.IsAssignableFrom<BadRequestObjectResult>(response);
            Assert.Equal("The dto parameter cannot be null.", actionResult.Value);
        }

        [Fact]
        public async Task TestPostPatientNull()
        {
            // Arrange
            var controller = new AppointmentController(null, null);

            // Act
            var response = await controller.Post(new AppointmentDTO { Patient = null });

            // Assert
            var actionResult = Assert.IsAssignableFrom<BadRequestObjectResult>(response);
            Assert.Equal($"The Patient property of the {typeof(AppointmentDTO)} parameter cannot be null.", actionResult.Value);
        }


        [Fact]
        public async Task TestPost()
        {
            // Arrange
            var dto = new AppointmentDTO { Id = 1, Patient = new PatientDTO() };

            var mockAppointmentService = new Mock<IAppointmentService>();
            mockAppointmentService
                .Setup(x => x.CreateAsync(dto))
                .ReturnsAsync(dto);

            var mockExternalPatientApiService = new Mock<IExternalPatientApiService>();
            mockExternalPatientApiService
                .Setup(x => x.PatientExists(It.IsAny<PatientDTO>()))
                .ReturnsAsync(true);
            
            var controller = new AppointmentController(mockAppointmentService.Object, mockExternalPatientApiService.Object);

            // Act
            var response = await controller.Post(dto);

            // Assert
            var actionResult = Assert.IsAssignableFrom<CreatedAtActionResult>(response);
            Assert.Equal("Get", actionResult.ActionName);
            Assert.IsAssignableFrom<AppointmentDTO>(actionResult.Value);
        }

        [Fact]
        public async Task TestPostPatientNotFound()
        {
            // Arrange
            var dto = new AppointmentDTO { Id = 1, Patient = new PatientDTO() };

            var mockAppointmentService = new Mock<IAppointmentService>();
            mockAppointmentService
                .Setup(x => x.CreateAsync(dto))
                .ReturnsAsync(dto);

            var mockExternalPatientApiService = new Mock<IExternalPatientApiService>();
            mockExternalPatientApiService
                .Setup(x => x.PatientExists(It.IsAny<PatientDTO>()))
                .ReturnsAsync(false);
            
            var controller = new AppointmentController(mockAppointmentService.Object, mockExternalPatientApiService.Object);

            // Act
            var response = await controller.Post(dto);

            // Assert
            var actionResult = Assert.IsAssignableFrom<BadRequestObjectResult>(response);
            Assert.Equal("No patient entity matching the specified personal details was found.", actionResult.Value);
        }

    }
}