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
        /// Returns a 404 status code if the username does not already exist.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("update/{username}")]
        public async Task<ActionResult> UpdateUser(string username, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("UserController.UpdateUser() was called with invalid body data.");
                return StatusCode(400);
            }

            if (await _userLogic.UpdateUser(username, user))
            {
                return StatusCode(202);
            }
            else
            {
                return StatusCode(404);
            }
        }

        /// <summary>
        /// Returns the User information associated with the provided username.
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
        // [HttpDelete("delete/{userid}")]
        // public async Task<ActionResult> DeleteUser(string userid, [FromBody] User user)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         Console.WriteLine("UserController.UpdateUser() was called with invalid body data.");
        //         return StatusCode(400);
        //     }

        //     if (await _userLogic.DeleteUser(userid))
        //     {
        //         return StatusCode(202);
        //     }
        //     else
        //     {
        //         return StatusCode(404);
        //     }
        // }

        [HttpPost("addadmin/{userid}")]
        public async Task<ActionResult> AddAsAdmin(string userid)
        {
            if (!userid)
            {
                Console.WriteLine("UserController.UpdateUser() was called with invalid user id.");
                return StatusCode(400);
            }

            if (await _userLogic.UpdateUser(userid))
            {
                return StatusCode(202);
            }
            else
            {
                return StatusCode(404);
            }
        }


    }
}