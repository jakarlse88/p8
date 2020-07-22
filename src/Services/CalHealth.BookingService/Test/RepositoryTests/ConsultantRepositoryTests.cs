using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalHealth.BookingService.Data;
using CalHealth.BookingService.Models;
using CalHealth.BookingService.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace CalHealth.BookingService.Test.RepositoryTests
{
    public class ConsultantRepositoryTests
    {
        [Fact]
        public async Task TestGetAllAsyncEagerLoading()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BookingContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;

            IEnumerable<Consultant> results;

            await using (var context = new BookingContext(options))
            {
                await context.Database.EnsureCreatedAsync();
                
                var repository = new Repository<Consultant>(context);
                
                // Act
                results = await repository.GetAllAsync(eager: true);

                await context.Database.EnsureDeletedAsync();
            }

            // Assert
            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<Consultant>>(results);
            Assert.Equal(6, results.Count());
            Assert.NotNull(results.First().Gender);
        }

        [Fact]
        public async Task TestGetByIdEager()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BookingContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;

            Consultant result;

            await using (var context = new BookingContext(options))
            {
                await context.Database.EnsureCreatedAsync();
                
                var repository = new Repository<Consultant>(context);
                
                // Act
                result = await repository.GetByIdAsync(1, eager: true);

                await context.Database.EnsureDeletedAsync();
            }

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Consultant>(result);
            Assert.Equal(1, result.Id);
            Assert.NotNull(result.Gender);
            Assert.NotNull(result.Specialty);
            Assert.NotNull(result.Appointment);
        }

        
        [Fact]
        public async Task TestGetByConditionEagerLoading()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BookingContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;

            IEnumerable<Consultant> results;

            await using (var context = new BookingContext(options))
            {
                await context.Database.EnsureCreatedAsync();
                
                var repository = new Repository<Consultant>(context);
                
                // Act
                results = await repository.GetByConditionAsync(_ => true, eager: true);

                await context.Database.EnsureDeletedAsync();
            }

            // Assert
            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<Consultant>>(results);
            Assert.Equal(6, results.Count());
            Assert.NotNull(results.First().Gender);
        }
        
        [Fact]
        public async Task TestGetAllNoResults()
        {
            // Arrange
            var consultants = new List<Consultant>();
            var mockDbSet = consultants.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<BookingContext>();
            mockContext
                .Setup(x => x.Set<Consultant>())
                .Returns(mockDbSet.Object);

            var repository = new Repository<Consultant>(mockContext.Object);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task TestGetAll()
        {
            // Arrange
            var consultants = GenerateConsultants();
            var mockDbSet = consultants.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<BookingContext>();
            mockContext
                .Setup(x => x.Set<Consultant>())
                .Returns(mockDbSet.Object);

            var repository = new Repository<Consultant>(mockContext.Object);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count());
        }

        [Fact]
        public async Task TestGetAllInclude()
        {
            // Arrange
            var consultants = GenerateConsultants();
            var mockDbSet = consultants.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<BookingContext>();
            mockContext
                .Setup(x => x.Set<Consultant>())
                .Returns(mockDbSet.Object);

            var repository = new Repository<Consultant>(mockContext.Object);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count());
            Assert.NotNull(result.First().Specialty);
        }

        [Fact]
        public void TestInsertEntityNull()
        {
            // Arrange
            var repository = new Repository<Consultant>(null);

            // Act
            void TestAction() => repository.Add(null);

            // Assert
            var ex = Assert.Throws<ArgumentNullException>(TestAction);
            Assert.Equal("entity", ex.ParamName);
        }

        [Fact]
        public void TestUpdateEntityNull()
        {
            // Arrange
            var repository = new Repository<Consultant>(null);

            // Act
            void TestAction() => repository.Update(null);

            // Assert
            var ex = Assert.Throws<ArgumentNullException>(TestAction);
            Assert.Equal("entity", ex.ParamName);
        }

        [Fact]
        public void TestInsert()
        {
            // Arrange
            var mockContext = new Mock<BookingContext>();
            mockContext
                .Setup(x => x.Set<Consultant>().Add(It.IsAny<Consultant>()))
                .Verifiable();

            var repository = new Repository<Consultant>(mockContext.Object);
            
            var entity = new Consultant();
            
            // Act
            repository.Add(entity);

            // Assert
            mockContext
                .Verify(x => x.Set<Consultant>().Add(It.IsAny<Consultant>()), Times.Once);
        }

        [Fact]
        public void TestUpdate()
        {
            // Arrange
            var mockContext = new Mock<BookingContext>();
            mockContext
                .Setup(x => x.Set<Consultant>().Update(It.IsAny<Consultant>()))
                .Verifiable();

            var repository = new Repository<Consultant>(mockContext.Object);
            
            var entity = new Consultant();
            
            // Act
            repository.Update(entity);

            // Assert
            mockContext
                .Verify(x => x.Set<Consultant>().Update(It.IsAny<Consultant>()), Times.Once);
        }

        /**
         * ============================
         * Internal helper methods
         *  ===========================
         */
        
        private static IEnumerable<Consultant> GenerateConsultants()
        {
            var consultants = new List<Consultant>
            {
                new Consultant
                {
                    Id = 1,
                    GenderId = 2,
                    SpecialtyId = 2,
                    FirstName = "Sophie",
                    LastName = "Harrington",
                    DateOfBirth = new DateTime(1985, 5, 24),
                    Specialty = new Specialty
                    {
                        Id = 1,
                        Type = "Test"
                    }
                },
                new Consultant
                {
                    Id = 2,
                    GenderId = 1,
                    SpecialtyId = 5,
                    FirstName = "Kilian",
                    LastName = "Lopez",
                    DateOfBirth = new DateTime(1967, 2, 5),
                    Specialty = new Specialty
                    {
                        Id = 1,
                        Type = "Test"
                    }
                },
                new Consultant
                {
                    Id = 3,
                    GenderId = 2,
                    SpecialtyId = 1,
                    FirstName = "Aya",
                    LastName = "Ahmed",
                    DateOfBirth = new DateTime(1990, 1, 9),
                    Specialty = new Specialty
                    {
                        Id = 1,
                        Type = "Test"
                    }
                },
                new Consultant
                {
                    Id = 4,
                    GenderId = 2,
                    SpecialtyId = 2,
                    FirstName = "Hyeo-jin",
                    LastName = "Lim",
                    DateOfBirth = new DateTime(1980, 2, 29),
                    Specialty = new Specialty
                    {
                        Id = 1,
                        Type = "Test"
                    }
                },
                new Consultant
                {
                    Id = 5,
                    GenderId = 1,
                    SpecialtyId = 7,
                    FirstName = "Lasse",
                    LastName = "Hansson",
                    DateOfBirth = new DateTime(1977, 12, 7),
                    Specialty = new Specialty
                    {
                        Id = 1,
                        Type = "Test"
                    }
                },
                new Consultant
                {
                    Id = 6,
                    GenderId = 1,
                    SpecialtyId = 4,
                    FirstName = "Abe",
                    LastName = "Shiraishi",
                    DateOfBirth = new DateTime(1973, 9, 5),
                    Specialty = new Specialty
                    {
                        Id = 1,
                        Type = "Test"
                    }
                }
            };

            return consultants;
        }
    }
}