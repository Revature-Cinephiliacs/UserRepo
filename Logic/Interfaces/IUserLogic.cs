using System.Collections.Generic;
using System.Threading.Tasks;

using Repository;
using GlobalModels;
using System;

namespace BusinessLogic.Interfaces
{
    public interface IUserLogic
    {
        /// <summary>
        /// Adds a new User Object to storage.
        /// Returns true if sucessful; false otherwise.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> CreateUser(User user);

        /// <summary>
        /// Updates a User Object in storage.
        /// Returns true if sucessful; false otherwise.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> UpdateUser(Guid userid, User user);

        /// <summary>
        /// Returns the User object whose Username is equal to the username argument.
        /// Returns null if the username is not found.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User GetUser(string username);

        /// <summary>
        /// Returns a list of every User object.
        /// </summary>
        /// <returns></returns>
        public Task<List<User>> GetUsers();

        /// <summary>
        /// Delete the user
        /// Return true or false
        /// </summary>
        public Task<bool> DeleteUser(Guid userid);

        /// <summary>
        /// add admin role to user
        /// pass user id 
        /// return true if successful
        /// </summary>
        // public Task<bool> AddAsAdmin(string uid);

        /// <summary>
        /// to check if a user exists
        /// pass user id 
        /// return true if successful
        /// </summary>
        public Task<bool> IsUserExist(Guid userid);

        /// <summary>
        /// Updates the permission level of user
        /// 1 = user, 2 = moderator, 3 = admin
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="permissionLevel"></param>
        /// <returns></returns>
        Task<bool> UpdatePermissions(Guid userid, int permissionLevel);
      
    }
}