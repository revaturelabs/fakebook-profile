﻿using System;
using System.Threading.Tasks;

using Fakebook.Profile.DataAccess.EntityModel;
using Fakebook.Profile.Domain;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using Xunit;

namespace Fakebook.Profile.UnitTests.DataAccessTests.RepositoryTests
{
    public class UpdateTest
    {
        [Theory]
        [ClassData(typeof(TestData.ProfileTestData.Update.Valid))]
        public async void UpdatingRealProfileWorks(DomainProfile orig, DomainProfile updated)
        {
            // Arrange
            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ProfileDbContext>()
                .UseSqlite(connection)
                .Options;

            // Act
            using (var actingContext = new ProfileDbContext(options))
            {
                actingContext.Database.EnsureCreated();

                var repo = new ProfileRepository(actingContext);

                // Create the user data
                await repo.UpdateProfileAsync(email: orig.Email, updated);
            }

            // Assert
            using (var assertionContext = new ProfileDbContext(options))
            {
                var repo = new ProfileRepository(assertionContext);

                await repo.UpdateProfileAsync(orig.Email, updated);

                //Assert.True(updateResult, "Unable to update the user.");
                var alteredUser = await repo.GetProfileAsync(orig.Email);

                Assert.NotEqual(orig.FirstName, alteredUser.FirstName);
                Assert.NotEqual(orig.LastName, alteredUser.LastName);
                Assert.NotEqual(orig.Status, alteredUser.Status);
            }
        }

        [Theory]
        [ClassData(typeof(TestData.ProfileTestData.Update.Invalid))]
        public async void Profile_Update_Invalid(DomainProfile original, DomainProfile update)
        {
            // Arrange
            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ProfileDbContext>()
                .UseSqlite(connection)
                .Options;

            // Act
            using (var actingContext = new ProfileDbContext(options))
            {
                actingContext.Database.EnsureCreated();

                var repo = new ProfileRepository(actingContext);

                // Create the user data
                await repo.UpdateProfileAsync(email: original.Email, update);
            }

            // Assert
            using (var assertionContext = new ProfileDbContext(options))
            {
                var repo = new ProfileRepository(assertionContext);

                await Assert.ThrowsAsync<ArgumentException>(() => repo.UpdateProfileAsync(original.Email, update));

                //Assert.True(updateResult, "Unable to update the user.");
                var profileActual = await repo.GetProfileAsync(original.Email);

                Assert.Equal(original, profileActual);
            }
        }
    }
}
