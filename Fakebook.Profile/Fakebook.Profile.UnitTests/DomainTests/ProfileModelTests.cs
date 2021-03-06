﻿using System;

using Fakebook.Profile.Domain;

using Xunit;

namespace Fakebook.Profile.UnitTests.DomainTests
{
    public class ProfileModelTests
    {
        #region EmailTests
        /// <summary>
        /// Check that emails can't be null.
        /// </summary>
        [Fact]
        public void EmailCantBeNull()
        {
            //arrange
            string email = null;
            //act
            //assert
            Assert.ThrowsAny<ArgumentNullException>(() => new DomainProfile(email, "First", "Last"));
        }

        /// <summary>
        /// Check that invalid emails throw an exception.
        /// </summary>
        /// <param name="email">The email to test. Should be invalid.</param>
        [Theory]
        [InlineData("test1@test")]
        [InlineData("test2.test2")]
        [InlineData("test3.test3@test3")]
        [InlineData("test4")]
        public void InvalidEmailThrowsException(string email)
        {
            //arrange
            //act
            //assert
            Assert.ThrowsAny<ArgumentException>(() => new DomainProfile(email, "First", "Last"));
        }

        /// <summary>
        /// Test that valid emails work.
        /// </summary>
        /// <param name="email">The email to test.</param>
        [Theory]
        [InlineData("Simple@email.com")]
        [InlineData("Other@email.test")]
        [InlineData("subdomianEmail@puppies.test.mail")]
        [InlineData("notcom@puppies.supplies")]
        public void ValidEmailWorks(string email)
        {
            //arrange
            //act
            DomainProfile profile = new DomainProfile(email, "First", "Last");

            //assert
            Assert.NotNull(profile.Email);
            Assert.Equal(email, profile.Email);
        }
        #endregion


        #region PfPTests
        /// <summary>
        /// Insure Uri's can be null.
        /// </summary>
        [Fact]
        public void UrlCanBeNull()
        {
            //arrange
            Uri url = null;
            DomainProfile profile = new DomainProfile("test@test.test", "First", "Last");

            //act
            profile.ProfilePictureUrl = url;

            //assert
            Assert.Null(profile.ProfilePictureUrl);
        }

        /// <summary>
        /// Check that a valid url is accepted.
        /// </summary>
        /// <param name="host">The host of the uri to test. Should be a domain</param>
        /// <param name="path">The path to the resource in the url</param>
        [Theory]
        //https://i.imgur.com/BCeyxdR.jpg
        [InlineData("i.imgur.com", "BCeyxdR.jpg")]
        //https://i.imgur.com/Lf5S5Sa.jpg
        [InlineData("i.imgur.com", "Lf5S5Sa.jpg")]
        //https://i.imgur.com/DX5KAnQ.jpg
        [InlineData("i.imgur.com", "DX5KAnQ.jpg")]
        public void ValidUrlWorks(string host, string path)
        {
            //arrange
            DomainProfile profile = new DomainProfile("test@test.com", "First", "Last");
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Host = host;
            uriBuilder.Path = path;

            //act
            Uri uri = uriBuilder.Uri;
            profile.ProfilePictureUrl = uri;

            //assert
            Assert.NotNull(profile.ProfilePictureUrl);
        }
        #endregion


        #region NameTests
        /// <summary>
        /// Check that the name property can format names correctly
        /// </summary>
        /// <param name="first">The first name</param>
        /// <param name="last">The last name</param>
        [Theory]
        [InlineData("first", "last")]
        [InlineData("Something", "Name")]
        public void NameReturnsProperFormat(string first, string last)
        {
            //arrange
            DomainProfile profile = new DomainProfile("example@email.com", "First", "Last");

            profile.FirstName = first;
            profile.LastName = last;

            //act 
            string fullname = profile.Name;

            //assert
            Assert.NotNull(fullname);
            Assert.Equal(first + ' ' + last, fullname);
        }



        /// <summary>
        /// Insure Valid characters are accepted
        /// </summary>
        /// <param name="name"> The name to test.</param>
        [Theory]
        [InlineData("Name")]
        [InlineData("name")]
        [InlineData("hypen-name")]
        [InlineData("Na,me")]
        [InlineData("Mr.Name")]
        [InlineData("X Æ A-12")]
        [InlineData("l33t")]
        public void ValidNameAccepted(string name)
        {
            //arrange
            DomainProfile profile = new DomainProfile("email@example.com", "First", "Last");

            //act
            profile.FirstName = name;
            profile.LastName = name;

            //assert
            Assert.Equal(name, profile.FirstName);
            Assert.Equal(name, profile.LastName);
        }


        /// <summary>
        /// Test that invalid names are rejected.
        /// </summary>
        /// <param name="name">The name to test</param>
        [Theory]
        [InlineData("1234567")]
        [InlineData("!@#$%^&*()_+")]
        public void InvalidNameRejected(string name)
        {
            //arrange
            DomainProfile profile = new DomainProfile("email@example.com", "First", "Last");

            //act
            //assert
            Assert.ThrowsAny<ArgumentException>(() => profile.FirstName = name);
            Assert.ThrowsAny<ArgumentException>(() => profile.LastName = name);
        }
        #endregion


        #region PhoneNumberTests
        /// <summary>
        /// Insure valid phone number inputs work.
        /// </summary>
        /// <param name="phoneNumber">Phone number string</param>
        [Theory]
        [InlineData("1234567899")]
        [InlineData("570-386-1123")]
        [InlineData("(267)-424-2243")]
        [InlineData("(267) 424-2243")]
        [InlineData("267 424 2243")]
        public void ValidPhoneNumberWorks(string phoneNumber)
        {
            // arrange
            DomainProfile profile = new DomainProfile("testemail@email.com", "First", "Last");

            // act
            profile.PhoneNumber = phoneNumber;

            // assert
            Assert.True(profile.PhoneNumber.Length <= 14 || profile.PhoneNumber.Length >= 10);
        }

        /// <summary>
        /// Invalid phone numbers don't work.
        /// </summary>
        /// <param name="phoneNumber">The phone number to test.</param>
        [Theory]
        [InlineData("345")]
        [InlineData("77777777777777777777")]
        [InlineData("dw#oi3456")]
        public void InvalidPhoneNumberThrowsException(string phoneNumber)
        {
            // arrange
            DomainProfile profile = new DomainProfile("testemail@email.com", "First", "Last");

            // act
            // assert
            Assert.ThrowsAny<ArgumentException>(() => profile.PhoneNumber = phoneNumber);
        }
        #endregion


        #region BirthdayTests
        /// <summary>
        /// Check if a valid birthdate is in the past
        /// </summary>
        /// <param name="birthDate"> a birthdate string</param>
        [Theory]
        [InlineData("8/18/1880")]
        [InlineData("9/19/1999")]
        [InlineData("12/29/2020")]
        public void ValidBirthDataShouldBeInThePast(string birthDate)
        {
            // arrange
            DomainProfile profile = new DomainProfile("testemail@email.com", "First", "Last");
            DateTime bDate = DateTime.Parse(birthDate);

            // act
            profile.BirthDate = bDate;

            // assert
            Assert.True(profile.BirthDate < DateTime.Now);
        }

        /// <summary>
        /// Check if an invalid birthdate throws an argument exception
        /// </summary>
        /// <param name="birthDate"> a birthdate string</param>
        [Theory]
        [InlineData("5/11/2021")]
        [InlineData("2/28/2022")]
        [InlineData("12/31/2023")]
        public void InvalidBirthdateShouldThrowAnException(string birthDate)
        {
            // arrange 
            DomainProfile profile = new DomainProfile("testemail@email.com", "First", "Last");
            DateTime bDate = DateTime.Parse(birthDate);

            // act + assert        
            Assert.ThrowsAny<ArgumentException>(() => profile.BirthDate = bDate);
        }

        /// <summary>
        /// Check that one milisecond ago is valid because it's in the past.
        /// </summary>
        [Fact]
        public void EarlierTodayIsValid()
        {
            // arrange 
            DomainProfile profile = new DomainProfile("testemail@email.com", "First", "Last");
            DateTime earlyerToday = DateTime.Now;
            earlyerToday = earlyerToday.AddMilliseconds(-1);

            // act
            profile.BirthDate = earlyerToday;
            // assert
            Assert.Equal(earlyerToday, profile.BirthDate);
        }
        #endregion
    }
}