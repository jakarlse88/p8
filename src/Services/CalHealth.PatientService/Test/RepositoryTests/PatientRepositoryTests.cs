using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalHealth.PatientService.Data;
using CalHealth.PatientService.Models;
using CalHealth.PatientService.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace CalHealth.PatientService.Test.RepositoryTests
{
    public class PatientRepositoryTests
    {
        [Fact]
        public async Task TestGetAllAsyncEagerLoading()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PatientContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;

            IEnumerable<Patient> results;

            await using (var context = new PatientContext(options))
            {
                await context.Database.EnsureCreatedAsync();
                
                var repository = new Repository<Patient>(context);
                
                // Act
                results = await repository.GetAllAsync(eager: true);

                await context.Database.EnsureDeletedAsync();
            }

            // Assert
            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<Patient>>(results);
            Assert.Equal(10, results.Count());
            Assert.NotNull(results.First().Gender);
        }

        
        [Fact]
        public async Task TestGetByConditionEagerLoading()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PatientContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;

            IEnumerable<Patient> results;

            await using (var context = new PatientContext(options))
            {
                await context.Database.EnsureCreatedAsync();
                
                var repository = new Repository<Patient>(context);
                
                // Act
                results = await repository.GetByConditionAsync(_ => true, eager: true);

                await context.Database.EnsureDeletedAsync();
            }

            // Assert
            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<Patient>>(results);
            Assert.Equal(10, results.Count());
            Assert.NotNull(results.First().Gender);
        }

        
        [Fact]
        public async Task TestGetByConditionPredicateNull()
        {
            // Arrange
            var repository = new Repository<Patient>(null);

            // Act
            async Task TestAction() => await repository.GetByConditionAsync(null);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
            Assert.Equal("predicate", ex.ParamName);
        }

        [Fact]
        public async Task TestGetByCondition()
        {
            // Arrange
            var patients = GeneratePatients();
            var mockDbSet = patients.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<PatientContext>();
            mockContext
                .Setup(x => x.Set<Patient>())
                .Returns(mockDbSet.Object);

            var repository = new Repository<Patient>(mockContext.Object);

            // Act
            var result = await repository.GetByConditionAsync(p => p.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.IsAssignableFrom<IEnumerable<Patient>>(result);
        }

        
        [Fact]
        public async Task TestGetAllNoResults()
        {
            // Arrange
            var patients = new List<Patient>();
            var mockDbSet = patients.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<PatientContext>();
            mockContext
                .Setup(x => x.Set<Patient>())
                .Returns(mockDbSet.Object);

            var repository = new Repository<Patient>(mockContext.Object);

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
            var patients = GeneratePatients();
            var mockDbSet = patients.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<PatientContext>();
            mockContext
                .Setup(x => x.Set<Patient>())
                .Returns(mockDbSet.Object);

            var repository = new Repository<Patient>(mockContext.Object);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task TestGetAllInclude()
        {
            // Arrange
            var patients = GeneratePatients();
            var mockDbSet = patients.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<PatientContext>();
            mockContext
                .Setup(x => x.Set<Patient>())
                .Returns(mockDbSet.Object);

            var repository = new Repository<Patient>(mockContext.Object);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void TestInsertEntityNull()
        {
            // Arrange
            var repository = new Repository<Patient>(null);

            // Act
            void TestAction() => repository.Insert(null);

            // Assert
            var ex = Assert.Throws<ArgumentNullException>(TestAction);
            Assert.Equal("entity", ex.ParamName);
        }

        [Fact]
        public void TestUpdateEntityNull()
        {
            // Arrange
            var repository = new Repository<Patient>(null);

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
            var mockContext = new Mock<PatientContext>();
            mockContext
                .Setup(x => x.Set<Patient>().Add(It.IsAny<Patient>()))
                .Verifiable();

            var repository = new Repository<Patient>(mockContext.Object);
            
            var entity = new Patient();
            
            // Act
            repository.Insert(entity);

            // Assert
            mockContext
                .Verify(x => x.Set<Patient>().Add(It.IsAny<Patient>()), Times.Once);
        }

        [Fact]
        public void TestUpdate()
        {
            // Arrange
            var mockContext = new Mock<PatientContext>();
            mockContext
                .Setup(x => x.Set<Patient>().Update(It.IsAny<Patient>()))
                .Verifiable();

            var repository = new Repository<Patient>(mockContext.Object);
            
            var entity = new Patient();
            
            // Act
            repository.Update(entity);

            // Assert
            mockContext
                .Verify(x => x.Set<Patient>().Update(It.IsAny<Patient>()), Times.Once);
        }

        /**
         * ============================
         * Internal helper methods
         *  ===========================
         */
        
        private static IEnumerable<Patient> GeneratePatients()
        {
            var patients = new List<Patient>
            {
                new Patient
                {
                    Id = 1
                    
                },
                new Patient
                {
                    Id = 2
                },
                new Patient
                {
                    Id = 3
                }
            };

            return patients;
        }
    }
}