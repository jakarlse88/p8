using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CalHealth.BookingService.Models;
using CalHealth.BookingService.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace CalHealth.BookingService.Test.ServiceTests
{
    public class ExternalPatientApiServiceTests
    {
        [Fact]
        public async Task TestPatientExistsArgumentNull()
        {
            // Arrange
            var service = new ExternalPatientApiService(null, null);

            // Act
            async Task TestAction() => await service.PatientExists(null);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
        }

        [Theory]
        [InlineData("Test", "")]
        [InlineData("", "Test")]
        public async Task TestPatientExistsArgumentPropertyNull(string firstName, string lastName)
        {
            // Arrange
            var patient = new PatientDTO
            {
                FirstName = firstName,
                LastName = lastName
            };

            var service = new ExternalPatientApiService(null, null);

            // Act
            async Task TestAction() => await service.PatientExists(null);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
        }

        [Fact]
        public async Task TestPatientExists()
        {
            // Arrange
            var testPatient = new PatientDTO
            {
                FirstName = "Test",
                LastName = "McTest",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            var cache = new MemoryCache(new MemoryCacheOptions());
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(true))
                })
                .Verifiable();

            
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:8083")
            };

            var expectedUri = new Uri($"https://localhost:8083/api/patient/exists?firstName={testPatient.FirstName}&lastName={testPatient.LastName}&dateOfBirth={testPatient.DateOfBirth.ToShortDateString()}");
            
            var service = new ExternalPatientApiService(httpClient, cache);
            
            // Act
            var result = await service.PatientExists(testPatient);

            // Assert
            Assert.True(result);
            
            mockHandler
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get
                        && req.RequestUri == expectedUri
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
        }

        [Fact]
        public async Task TestPatientExistsNotFound()
        {
            // Arrange
            var testPatient = new PatientDTO
            {
                FirstName = "Test",
                LastName = "McTest",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            var cache = new MemoryCache(new MemoryCacheOptions());
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(false))
                })
                .Verifiable();

            
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:8083")
            };

            var expectedUri = new Uri($"https://localhost:8083/api/patient/exists?firstName={testPatient.FirstName}&lastName={testPatient.LastName}&dateOfBirth={testPatient.DateOfBirth.ToShortDateString()}");
            
            var service = new ExternalPatientApiService(httpClient, cache);
            
            // Act
            var result = await service.PatientExists(testPatient);

            // Assert
            Assert.False(result);
            
            mockHandler
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get
                        && req.RequestUri == expectedUri
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
        }

    }
}