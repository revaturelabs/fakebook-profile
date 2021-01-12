﻿using Fakebook.Profile.Domain.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fakebook.Profile.Domain
{
    /// <summary>
    /// A profile for the domain logic and Api to use.
    /// </summary>
    public class DomainProfile
    {
        //[anything]@[anything].[anything]
        private string _email; 
        /// <summary>
        /// The user's email.
        /// </summary>
        /// <remarks>
        /// Cannot be null, and must match the Regex for email characters.
        /// </remarks>
        public string Email
        {
            get => _email; 
            set
            {
                if(value is null)
                {
                    throw new ArgumentNullException("Email cannot be null.");
                }
                else if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"Invalid Email, {value}");
                }

                Regex emailRegex = new Regex(RegularExpressions.EmailCharacters);
                Match m = emailRegex.Match(value);
                if (!m.Success)
                    throw new ArgumentException();
                _email = value;
            }
        }

        //should be a url, defualts to an image.
        /// <summary>
        /// A url that is expected to be an image. Technically though, no checks are done to enforce that.
        /// </summary>
        public Uri ProfilePictureUrl { get; set; } = new Uri("https://publicdomainvectors.org/photos/defaultprofile.png");

        /// <summary>
        /// Shortcut to get a user's full name. 
        /// </summary>
        public string Name => $"{FirstName} {LastName}";

        private string _firstName;
        /// <summary>
        /// The user's first name.
        /// </summary>
        /// <remarks>
        /// A-Z, ', ., - are the only valid characters for first and last name.
        /// </remarks>
        public string FirstName
        {
            get => _firstName;
            set
            {
                if(value is null)
                {
                    throw new ArgumentNullException("First name cannot be null.");
                }
                 else if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"Invalid first name, {value}");
                }

                Regex nameRegex = new Regex(RegularExpressions.NameCharacters);
                // throw null exception if value is null
                Match m = nameRegex.Match(value);
                if (!m.Success)
                    throw new ArgumentException();
                _firstName = value;
            }
        }

        private string _lastName;
        /// <summary>
        /// The user's last name.
        /// </summary>
        /// <remarks>
        /// A-Z, ', ., - are the only valid characters for first and last name.
        /// </remarks>
        public string LastName
        {
            get => _lastName;
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Last name cannot be null.");
                }
                else if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"Invalid last name, {value}");
                }

                Regex nameRegex = new Regex(RegularExpressions.NameCharacters);
                Match m = nameRegex.Match(value);
                if (!m.Success)
                    throw new ArgumentException();
                _lastName = value;
            }
        }

        private string _phoneNumber;
        /// <summary>
        /// The user's phone number.
        /// </summary>
        /// <remarks>
        /// any up to 15 digits, can have - or () spaces optional, or be null
        /// </remarks>
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {  
                if(value is not null)
                {
                    Regex phoneRegex = new Regex(RegularExpressions.PhoneNumberCharacters);
                    Match m = phoneRegex.Match(value);
                    if (!m.Success)
                        throw new ArgumentException();
                }
                
                _phoneNumber = value;

            }
        }

        private DateTime _birthDate;
        /// <summary>
        /// The user's birthdate. 
        /// </summary>
        /// <remarks>
        /// Can't be null, but will default to all zeros.
        /// </remarks>
        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (value.Date > DateTime.Now.Date) 
                    throw new ArgumentException($"Invalid Date, {value.Date}, is in the future.");
                _birthDate = value;
            }
        }

        string _status;
        /// <summary>
        /// The user's current status message.
        /// </summary>
        /// <remarks>
        /// can be null, or reasonable text (sanitized so they don't get funky)
        /// </remarks>
        public string Status { 
            get => _status;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _status = null;
                }
                else
                {
                    //todo: check for funk, like js, html, and sql injection. Packages can help
                    _status = value;
                }
            }
        }

        public DomainProfile(string email, string firstname, string lastname, DateTime birthdate, Uri pfpuri = null, string phonenumer = null, string status = null )
        {
            this.Email = email;
            if (email == null)
            {
                throw new ArgumentNullException();            
            }

            this.FirstName = firstname;
            this.LastName = lastname;  
            
            if(pfpuri is null)
            {
                this.ProfilePictureUrl = new Uri("https://publicdomainvectors.org/photos/defaultprofile.png");
            }

            this.BirthDate = birthdate;

            this.PhoneNumber = phonenumer;

            this.Status = status;
        }

        /// <summary>
        /// Create a new user with a phone number filled in.
        /// </summary>
        /// <param name="email">The user's email. Must be uniqeu.</param>
        /// <param name="phoneNumber">The user's phone number/</param>
        /// <param name="firstname">The user's first name.</param>
        /// <param name="lastname">The user's last name.</param>
        public DomainProfile(string email, string phoneNumber, string firstname, string lastname)
        {
            this.Email = email;
            this.PhoneNumber = phoneNumber;

            this.FirstName = firstname;
            this.LastName = lastname;
        }



        /// <summary>
        /// Create a basic profile
        /// </summary>
        /// <param name="email">The user's email. Must be Unique.</param>
        /// <param name="firstname">The user's firstname.</param>
        /// <param name="lastname">The user's lastname.</param>
        public DomainProfile(string email, string firstname, string lastname)
        {
            this.Email = email;
            if (email == null)
            {
                throw new ArgumentNullException();
            }

            this.FirstName = firstname;
            this.LastName = lastname;
        }
    }
}
