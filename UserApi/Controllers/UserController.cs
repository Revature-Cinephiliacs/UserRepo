using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic.Interfaces;
using Repository;
using GlobalModels;
using Microsoft.Extensions.Logging;

namespace CineAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserLogic _userLogic;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserLogic userLogic, ILogger<UserController> logger)
        {
            _userLogic = userLogic;
            _logger = logger;
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
                _logger.LogWarning("UserController.CreateUser() was called with invalid body data.");
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
        /// Returns a user based on their user id
        /// Returns 404 if user could not be found with the user id
        /// Returns 200 if the user was found
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet("{userid}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUserById(string userid)
        {
            User findUser = await _userLogic.GetUserById(userid);
            if (findUser == null)
            {
                return StatusCode(404);
            }
            StatusCode(200);
            return findUser;
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
                _logger.LogWarning("UserController.UpdateUser() was called with invalid body data.");
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
        /// Delete the User based on userid.
        /// If modelbidning fails, return 400
        /// If deleting user fails(couldn't find user), return 404
        /// If successfully deleted, return 400
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpDelete("delete/{userid}")]
        public async Task<ActionResult> DeleteUser(string userid)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("UserController.UpdateUser() was called with invalid body data.");
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
        /// If updating permissions fails, return 404
        /// If updating is successful, return 202
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
        /// If we couldn't get their age(invalid user/no dob) return 404
        /// If successful, return 200 and the age
        /// ----------------------------------
        /// Return type as a double instead of an int to allow for future implementation for more accurate age
        /// ----------------------------------
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
        /// Adds a new follower->followee relationship.
        /// Adds a new FollowingUser Object into the database.
        /// Returns true if successful.
        /// Returns false if failed.
        /// </summary>
        /// <param name="follower"></param>
        /// <param name="followee"></param>
        /// <returns></returns>
        [HttpPost("follow/{follower}/{followee}")]
        public async Task<ActionResult<bool>> FollowUser(string follower, string followee)
        {
            bool followNewUser = await _userLogic.FollowUser(follower, followee);
            if (followNewUser)
            {
                return StatusCode(201);
            }
            else
            {
                return StatusCode(404);
            }
        }

        /// <summary>
        /// Returns a list of all the users someone is following from their userid
        /// Returns 404 if unable to find original user
        /// Returns 204 if able to find user, but has no follows
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet("followlist/{userid}")]
        public async Task<ActionResult<List<User>>> GetFollowedUserList(string userid)
        {
            List<User> allUsers = await _userLogic.GetFollowingUsers(userid);
            if (allUsers == null)
            {
                return StatusCode(404);
            }
            if (allUsers.Count == 0)
            {
                return StatusCode(204);
            }
            StatusCode(200);
            return allUsers;
        }
    }
}
