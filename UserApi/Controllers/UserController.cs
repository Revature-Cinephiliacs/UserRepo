﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Repository;
using GlobalModels;

namespace CineAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserLogic _userLogic;
        public UserController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        /// <summary>
        /// Returns a list of User objects that includes every User.
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public async Task<ActionResult<List<User>>> Get()
        {
            return await _userLogic.GetUsers();
        }

        /// <summary>
        /// Adds a new User based on the information provided.
        /// Returns a 400 status code if creation fails.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("UserController.CreateUser() was called with invalid body data.");
                return StatusCode(400);
            }

            if (await _userLogic.CreateUser(user))
            {
                return StatusCode(201);
            }
            else
            {
                return StatusCode(200);
            }
        }

        /// <summary>
        /// Updates User information based on the information provided.
        /// Returns a 400 status code if the incoming data is invalid.
        /// Returns a 404 status code if the userid does not already exist.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("update/{userid}")]
        public async Task<ActionResult> UpdateUser(string userid, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("UserController.UpdateUser() was called with invalid body data.");
                return StatusCode(400);
            }

            if (await _userLogic.UpdateUser(userid, user))
            {
                return StatusCode(202);
            }
            else
            {
                return StatusCode(404);
            }
        }

        /// <summary>
        /// Returns the User information associated with the provided user email.
        /// </summary>
        /// <param name="useremail"></param>
        /// <returns></returns>
        [HttpGet("{useremail}")]
        public ActionResult<User> GetUser(string useremail)
        {
            User user = _userLogic.GetUser(useremail);

            if (user == null)
            {
                return StatusCode(404);
            }
            StatusCode(200);
            return user;
        }
        
        /// <summary>
        /// Delete the User based on userid.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpDelete("delete/{userid}")]
        public async Task<ActionResult> DeleteUser(string userid)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("UserController.UpdateUser() was called with invalid body data.");
                return StatusCode(400);
            }

            if (await _userLogic.DeleteUser(userid))
            {
                return StatusCode(202);
            }
            else
            {
                return StatusCode(404);
            }
        }

        /// <summary>
        /// Changes a user's permissionlevel up to an admin level's (3)
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpPost("addadmin/{userid}")]
        public async Task<ActionResult> AddAsAdmin(string userid)
        {
            if (await _userLogic.UpdatePermissions(userid, 3))
            {
                return StatusCode(202);
            }
            else
            {
                return StatusCode(404);
            }
        }

        /// <summary>
        /// Returns the age of the user associated with the provided userid.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet("age/{userid}")]
        public ActionResult<double> GetUserAge(string userid)
        {
            double? nullableAge = _userLogic.GetUserAge(userid);
            double age = 0;

            if (nullableAge == null)
            {
                return StatusCode(404);
            }
            else
            {
                age = nullableAge ?? 0;
            }
            StatusCode(200);
            return age;
        }
        
        /// <summary>
        /// Returns a list containing all of the movie IDs for the Movies that
        /// the User with the provided username is following.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("movies/{userid}")]
        public async Task<ActionResult<List<string>>> GetFollowedMovies(string userid)
        {
            List<string> movieids = await _userLogic.GetFollowingMovies(userid);
            
            if(movieids == null)
            {
                return StatusCode(404);
            }
            if(movieids.Count == 0)
            {
                return StatusCode(204);
            }
            StatusCode(200);
            return movieids;
        }

        /// <summary>
        /// Adds the Movie with the provided movie ID to the provided User's 
        /// list of Movies they are following.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="movieid"></param>
        /// <returns></returns>
        [HttpPost("movie/{userid}/{movieid}")]
        public async Task<ActionResult> FollowMovie(string userid, string movieid)
        {
            var result = await _userLogic.FollowMovie(userid, movieid);
            if(result)
            {
                return StatusCode(201);
            }
            else
            {
                return StatusCode(400);
            }
        }
    }
}
