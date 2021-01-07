﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Fakebook.Profile.Domain;
using Fakebook.Profile.RestApi.ApiModel;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fakebook.Profile.RestApi.Controllers
{
    /// <summary>
    /// Controller that handles routes/actions relating to profiles
    /// </summary>
    [Route("api/profiles")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _repository;

        /// <summary>
        /// Contructor method for creating a Profile Controller
        /// </summary>
        /// <param name="repository">Instance of an IRepository interface that allows for the class to store through different mediums</param>
        public ProfileController(IProfileRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Helper method for getting the password of a currently authenticated user
        /// </summary>
        /// <returns>The email of the user, if they are logged in</returns>
        private string GetUserEmail()
        {
            try
            {
                return User.FindFirst(ct => ct.Type.Contains("nameidentifier")).Value;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Action method that handles getting multiple profiles via their emails;
        /// PUT: /api/profiles/selection/{emails}
        /// </summary>
        /// <param name="emails">A collection of emails as strings to get the profiles</param>
        /// <returns>A collection of profiles converted to API Models</returns>
        [Authorize]
        [HttpGet("selection/{emails}")]
        public async Task<ActionResult<IEnumerable<ProfileApiModel>>> SelectProfilesAsync([FromBody] IEnumerable<string> emails)
        {
            var results = await _repository.GetProfilesByEmailAsync(emails);
            // convert them to the ApiModel

            return results
                .Select(p => new ProfileApiModel(p))
                .ToList();
        }

        /// <summary>
        /// Action method that handles getting a single user by their email;
        /// GET: /api/profiles/{profileEmail}
        /// </summary>
        /// <param name="profileEmail">The email of the user being retrieved</param>
        /// <returns>If found, a profile API model version of the profile; if not, it returns a NotFound()</returns>
        [Authorize]
        [HttpGet("")]
        [HttpGet("{profileEmail}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProfileApiModel>> GetAsync(string profileEmail = null)
        {
            string email = profileEmail is not null ? profileEmail : GetUserEmail();

            if (email is null)
            {
                throw new ArgumentException("Could not find current user's email");
            }

            var result = await _repository.GetProfileAsync(email);
            return Ok(new ProfileApiModel(result));
        }

        /// <summary>
        /// Action method that is used for creating a new profile;
        /// POST: /api/profiles/
        /// </summary>
        /// <param name="apiModel">The data of the profile to be created</param>
        /// <returns>Created if the model was created successfully, otherwise a 400-based status code</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateAsync([FromBody] ProfileApiModel apiModel)
        {
            try
            {
                var domainProfile = apiModel.ToDomainProfile();
                await _repository.CreateProfileAsync(domainProfile);
                return this.CreatedAtAction(nameof(GetAsync), new { email = apiModel.Email });
            }
            catch
            {
                // return this because the profile could not be created.
                return BadRequest();
            }
        }

        /// <summary>
        /// Updates a user profile with a given email;
        /// PUT: /api/profiles/
        /// </summary>
        /// <param name="apiModel">The data to update the currect user with, if it exists</param>
        /// <returns>200 Ok if the process goes successfully; elsewise a 400-based status code</returns>
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateAsync([FromBody] ProfileApiModel apiModel)
        {
            try
            {
                string userEmail = GetUserEmail();
                if(userEmail is null)
                {           
                    throw new ArgumentException("Could not find current user's email");
                }

                await _repository.UpdateProfileAsync(userEmail, apiModel.ToDomainProfile());
                return Ok();
            }
            catch
            {
                //should be 404?
                return BadRequest();
            }
        }
    }
}
