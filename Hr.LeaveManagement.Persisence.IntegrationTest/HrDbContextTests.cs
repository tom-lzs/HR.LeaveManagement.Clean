using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shouldly;

namespace Hr.LeaveManagement.Persisence.IntegrationTest
{
    public class HrDbContextTests

    {
        private HrDatabaseContext _hrDbContext;

        public HrDbContextTests()
        {
            var dbOptions = new DbContextOptionsBuilder<HrDatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;


            _hrDbContext = new HrDatabaseContext(dbOptions);
        }

        [Fact]  
        public async Task Save_SetDateCreatedValueAsync()
        {
            //arrange
            var leaveType = new LeaveType
            {
                Id = 1,
                DefaultDays = 10,
                Name = "Test Vacation",
            };

            //Act
            await _hrDbContext.LeaveTypes.AddAsync(leaveType);
            await _hrDbContext.SaveChangesAsync();

            //Assert
            leaveType.DateCreated.ShouldNotBeNull();
        }

        [Fact]
        public async void Save_SetDateModifiedValue()
        {
            //arrange
            var leaveType = new LeaveType
            {
                Id = 1,
                DefaultDays = 10,
                Name = "Test Vacation",
            };

            //Act
            await _hrDbContext.LeaveTypes.AddAsync(leaveType);
            await _hrDbContext.SaveChangesAsync();

            //Assert
            leaveType.DateModified.ShouldNotBeNull();

        }
    }
}