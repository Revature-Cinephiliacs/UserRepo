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
        /// Returns the age of the user, in years with decimal places representing
        /// the days. Returns null if the userid does not exist.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public double? GetUserAge(string userid);

        /// <summary>
        /// Returns a User object using a userid
        /// Will return null if userid was not found in the database
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Task<User> GetUserById(string userid);

        /// <summary>
        /// Function for one user to follow another user
        /// Adds a new FollowingUser Object to database
        /// Returns true if successful
        /// Return false if failed
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

        /// <summary>
        /// Returns the username of a user based on their userid
        /// Returns null is unable to find the user
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Task<string> GetUserNameById(string userid);

        /// <summary>
        /// Creates notifications about a newly created object(reviews, discussion, comments)
        /// Type defines the microservice that we received the newly recreated object from
        /// c = comments, d = discussions, r = reviews
        /// </summary>
        /// <param name="commentNotification"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task CreateNotifications(ModelNotification modelNotifications, string type);

        /// <summary>
        /// Gets a list of notifications for a user based on userid
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Task<List<NotificationDTO>> GetNotifications(string userid);

        /// <summary>
        /// Deletes a single notification from the database
        /// </summary>
        /// <param name="notificationid"></param>
        /// <returns></returns>
        public Task<bool> DeleteSingleNotification(Guid notificationid);

        /// <summary>
        /// Deletes all notifications that belong to a user, based on userid
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Task<bool> DeleteNotifications(string userid);
    }
}