﻿using Abode.Models; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Abode.AccountsRouting
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsRouting : ControllerBase
    {
        private readonly AbodeDbContext _dbContext;

        public AccountsRouting(AbodeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("profile")]
        public IActionResult CreateProfile([FromBody] Profile profileInput)
        {
            try
            {
                // Check if the user exists in dbo.Accounts
                var user = _dbContext.Accounts.FirstOrDefault(u => u.UserId == profileInput.UserId);
                if (user == null)
                {
                    return NotFound(new { message = $"User with ID {profileInput.UserId} not found." });
                }

                // Add the new profile to the database
                _dbContext.Profiles.Add(profileInput);
                _dbContext.SaveChanges();

                return Ok(new { message = "Profile created successfully.", profile = profileInput });
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { message = "Error occurred while saving profile to the database. NOTE YOU CAN NOT ADD THE SAME USER ID TO THIS TABLE TWICE", details = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        [HttpGet("profile/{userId}")]
        public IActionResult GetProfilesByUserId(int userId)
        {
            try
            {
                // Retrieve all profiles related to userId
                var profiles = _dbContext.Profiles
                    .Where(p => p.UserId == userId)
                    .ToList();

                if (profiles == null || !profiles.Any())
                {
                    return NotFound(new { message = $"No profiles found for user with ID {userId}." });
                }

                return Ok(profiles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("allUsernames")]
        public IActionResult GetAllUsernames()
        {
            try
            {
                var usernames = _dbContext.Accounts
                    .Select(a => a.Username)   
                    .ToList();

                return Ok(usernames);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("allEmails")]
        public IActionResult GetAllEmails()
        {
            try
            {
                var emails = _dbContext.Accounts
                .Select(a => a.Email)   
                .ToList();
                return Ok(emails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPut("profile/{userId}")]
        public IActionResult UpdateProfile(int userId, [FromBody] Profile profileInput){
            try
            {
                var existingProfile = _dbContext.Profiles.FirstOrDefault(p => p.UserId == userId);
                if (existingProfile == null)
                {
                    return NotFound(new { message = $"Profile for user with ID {userId} not found." });
                }
                existingProfile.AirConditioning = profileInput.AirConditioning;
                existingProfile.Deposit = profileInput.Deposit;
                existingProfile.PhoneNumber = profileInput.PhoneNumber;
                existingProfile.Address = profileInput.Address;
                existingProfile.Amenities = profileInput.Amenities;
                existingProfile.Bathrooms = profileInput.Bathrooms;
                existingProfile.Bedrooms = profileInput.Bedrooms;
                existingProfile.Bio = profileInput.Bio;
                existingProfile.Description = profileInput.Description;
                existingProfile.Laundry = profileInput.Laundry;
                existingProfile.LeaseTerms = profileInput.LeaseTerms;
                existingProfile.Name = profileInput.Name;
                existingProfile.Parking = profileInput.Parking;
                existingProfile.PhoneNumber = profileInput.PhoneNumber;
                existingProfile.Photo = profileInput.Photo;
                existingProfile.Price = profileInput.Price;
                existingProfile.LeaseTerms = profileInput.LeaseTerms;
                existingProfile.PublicEmail = profileInput.PublicEmail;
                existingProfile.Username = profileInput.Username;                
                _dbContext.SaveChanges();
                return Ok(new { message = "Profile updated successfully.", profile = existingProfile });
            }
            catch (DbUpdateException dbEx){
                return BadRequest(new { message = "Error occurred while updating profile in the database.", details = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}

