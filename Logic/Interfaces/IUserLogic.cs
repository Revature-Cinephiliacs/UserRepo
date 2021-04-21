using System.Collections.Generic;
using System.Threading.Tasks;

using Repository;
using GlobalModels;

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
        public Task<bool> UpdateUser(string username, User user);

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

        // public Task<bool> DeleteUser(string uid);

        // /// <summary>
        // /// add admin role to user
        // /// pass user id 
        // /// return true if successful
        // /// </summary>
        // /// 
        // public Task<bool> AddAsAdmin(string uid);

        /// <summary>
        /// to check if a user exists
        /// pass user id 
        /// return true if successful
        /// </summary>
        /// 
        public Task<bool> IsUserExist(string uid);
      
    }
}