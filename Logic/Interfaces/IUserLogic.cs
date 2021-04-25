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
        public Task<bool> UpdateUser(string userid, User user);

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
        public Task<bool> DeleteUser(string userid);

        /// <summary>
        /// Updates the permission level of user
        /// 1 = user, 2 = moderator, 3 = admin
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="permissionLevel"></param>
        /// <returns></returns>
        Task<bool> UpdatePermissions(string userid, int permissionLevel);

        
        /// <summary>
        /// Returns a list of every FollowingMovie object that was created by the user
        /// specified by the username argument. Returns null if the username doesn't
        /// exist.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Task<List<string>> GetFollowingMovies(string userid);

        
        /// <summary>
        /// Adds a new FollowingMovie Object to storage.
        /// Returns true if successful; false otherwise.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="movieid"></param>
        /// <returns></returns>
        public Task<bool> FollowMovie(string userid, string movieid);

        /// <summary>
        /// Returns the age of the user, in years with decimal places representing
        /// the days. Returns null if the userid does not exist.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public double? GetUserAge(string userid);

        /// <summary>
        /// Function for one user to follow another user
        /// Adds a new FollowingUser Object to database
        /// Returns true is successful
        /// Return false is failed
        /// </summary>
        /// <param name="follower"></param>
        /// <param name="followee"></param>
        /// <returns></returns>
        public Task<bool> FollowUser(string follower, string followee);

        /// <summary>
        /// Returns a list of all the users a specific user is following
        /// Will return an empty list if specific user exists, but no follows
        /// Will return null if specific userid couldn't be found in database
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Task<List<User>> GetFollowingUsers(string userid);
    }
}