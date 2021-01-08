﻿using Fakebook.Profile.DataAccess.EntityModel;
using Microsoft.EntityFrameworkCore;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fakebook.Profile.Domain
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ProfileDbContext _context;
        public ProfileRepository(ProfileDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Model mapping from entity to domain
        /// </summary>
        /// <param name="profile">entity model object used</param>
        /// <exception type="ArgumentNullException">If the profile, or the profile's email is null,
        /// this will be thrown </exception>
        /// <returns></returns>
        private static DomainProfile ToDomainProfile(EntityProfile profile)
        {
            if (profile == null || profile.Email == null || profile.FirstName == null || profile.LastName == null)
            {
                throw new ArgumentNullException("Must have a entity profile, with an email.");
            }

            DomainProfile convertedProfile = new DomainProfile(profile.Email)
            {
                ProfilePictureUrl = profile.ProfilePictureUrl,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                PhoneNumber = profile.PhoneNumber,
                BirthDate = profile.BirthDate,
                Status = profile.Status
            };

            return convertedProfile;
        }

        /// <summary>
        /// default model mapping from domain to entity
        /// </summary>
        /// <param name="profile">domain model object used</param>
        /// <returns></returns>
        private static EntityProfile ToEntityProfile(DomainProfile profile)
        {
            if (profile == null || profile.Email == null || profile.FirstName == null || profile.LastName == null)
            {
                throw new ArgumentNullException("Must have a domain profile, with an email.");
            }

            EntityProfile convertedProfile = new EntityProfile
            {
                Email = profile.Email,
                ProfilePictureUrl = profile.ProfilePictureUrl,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                PhoneNumber = profile.PhoneNumber,
                BirthDate = profile.BirthDate,
                Status = profile.Status
            };

            //will not fill in navigation properties currently 
            //if there's already an entry for this profile in the DB
            return convertedProfile;
        }

        /// <summary>
        /// Get all users' profiles at once
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DomainProfile>> GetAllProfilesAsync()
        {          
            var entity = await _context.EntityProfiles
                .ToListAsync();

            // model mapping
            var users = entity.Select(e => ToDomainProfile(e));
            return users;         
        }

        /// <summary>
        /// Get one specific user profile using his email
        /// </summary>
        /// <param name="email">email used to find the user</param>
        /// <returns></returns>
        public async Task<DomainProfile> GetProfileAsync(string email)
        {
            var entities = _context.EntityProfiles;

            if (!entities.Any())
            {
                throw new ArgumentException("Source is empty");
            }

            var profileFound = _context.EntityProfiles.FirstOrDefaultAsync(x => x.Email == email);
            if (profileFound == null)
            {
                throw new ArgumentNullException("Email not found"); 
            }

            var entity = await entities                
                .FirstOrDefaultAsync(e => e.Email == email);

            // model mapping
            var user = ToDomainProfile(entity);
            return user;
        }

        /// <summary>
        /// Get a group of user profiles using their emails
        /// </summary>
        /// <param name="emails">a collection of emails used</param>
        /// <returns></returns>
        public async Task<IEnumerable<DomainProfile>> GetProfilesByEmailAsync(IEnumerable<string> emails)
        {
            var userEntities = _context.EntityProfiles;
            var userEmails = userEntities
                .Select(u => u.Email)
                .ToList();

            if (!emails.All(e => userEmails.Contains(e)))
            {
                throw new ArgumentException("Not all emails requested are present.");
            }

            var users = await userEntities            
                .ToListAsync();

            if (!emails.Any() || !users.Any())
            {
                return new List<DomainProfile>();
            }

            return users
                .Where(u => emails.Contains(u.Email))
                .Select(u => ToDomainProfile(u))
                .ToList();
        }

        /// <summary>
        /// Take in a domain profile and create an entity profile
        /// </summary>
        /// <param name="profileData">domain profile used</param>
        /// <returns></returns>
        public async Task CreateProfileAsync(DomainProfile profileData)
        {

            // have to have this try catch block to prevent errors from data base
            try
            {
                var newUser = ToEntityProfile(profileData); // convert
                await _context.AddAsync(newUser);
                await _context.SaveChangesAsync();             
            }
            catch
            {
                throw new ArgumentException("Failed to create a profile");
            }
        }

        public async Task UpdateProfileAsync(string email, DomainProfile domainProfileData)
        {
            // have to have this try catch block to prevent errors from data base
            try
            {
                var userEntity = ToEntityProfile(domainProfileData);

                var entities = _context.EntityProfiles;
                if (!entities.Any())
                {
                    throw new ArgumentException("Source is empty");
                }

                var profileFound = _context.EntityProfiles.FirstOrDefaultAsync(x => x.Email == email);
                if (profileFound == null)
                {
                    throw new ArgumentNullException("Email not found");
                }

                var entity = await entities.FirstOrDefaultAsync(x => x.Email == email);
                // assign all the values
                {
                    entity.Email = userEntity.Email;
                    entity.ProfilePictureUrl = userEntity.ProfilePictureUrl;
                    entity.FirstName = userEntity.FirstName;
                    entity.LastName = userEntity.LastName;
                    entity.PhoneNumber = userEntity.PhoneNumber;
                    entity.BirthDate = userEntity.BirthDate;
                    entity.Status = userEntity.Status;
                }
                // save changes.
                _context.SaveChanges();
            }
            catch
            {
                throw new ArgumentException("Failed to update a profile");
            }
        }
    }

}

