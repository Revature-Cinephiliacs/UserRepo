using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Models;

namespace Repository
{
    public class RepoLogic
    {
        private readonly Cinephiliacs_UserContext _dbContext;
        private readonly ILogger<RepoLogic> _logger;

        public RepoLogic(Cinephiliacs_UserContext dbContext)
        {
            _dbContext = dbContext;
        }

        public RepoLogic(Cinephiliacs_UserContext dbContext, ILogger<RepoLogic> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Adds the User specified in the argument to the database. Returns true iff successful.
        /// Returns false if the user already exists.
        /// </summary>
        /// <param name="repoUser"></param>
        /// <returns></returns>
        public async Task<bool> AddUser(User repoUser)
        {
            if (UserExistsById(repoUser.UserId))
            {
                _logger.LogWarning($"RepoLogic.AddUser() was called for a userid that already exists {repoUser.UserId}.");
                return false;
            }
            if (UserExistsByEmail(repoUser.Email))
            {
                _logger.LogWarning($"RepoLogic.AddUser() was called for a useremail that already exists {repoUser.Email}.");
                return false;
            }
            if (UserExistsByUsername(repoUser.Username))
            {
                _logger.LogWarning($"RepoLogic.AddUser() was called for a username that already exists {repoUser.Username}.");
                return false;
            }
            await _dbContext.Users.AddAsync(repoUser);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Adds a new FollowingUser object into the database
        /// Checks if both the follower and followee exists, on id
        /// If either follower or followee does not exist, return true
        /// If successul in adding to database, return true
        /// </summary>
        /// <param name="newFollow"></param>
        /// <returns></returns>
        public async Task<bool> FollowUser(FollowingUser newFollow)
        {
            if (!UserExistsById(newFollow.FollowerUserId))
            {
                _logger.LogWarning($"RepoLogic.Follower() was called for a userid(follower) that does not exist {newFollow.FollowerUserId}.");
                return false;
            }
            if (!UserExistsById(newFollow.FolloweeUserId))
            {
                _logger.LogWarning($"RepoLogic.Follower() was called for a userid(follower) that does not exist {newFollow.FolloweeUserId}.");
                return false;
            }
            await _dbContext.AddAsync<FollowingUser>(newFollow);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Returns the entire list of users that a specific user is following
        /// Will return null if the specific userid does not exist
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<List<FollowingUser>> GetFollowingUsers(string userid)
        {
            if (!UserExistsById(userid))
            {
                _logger.LogWarning($"RepoLogic.GetFollowing was called for a userid that does not exist {userid}.");
                return null;
            }
            return await _dbContext.FollowingUsers.Include(x => x.FolloweeUser).Where(x => x.FollowerUserId == userid).ToListAsync();
        }

        /// <summary>
        /// Updates the information of the User identified by the userId argument
        /// to the information in the updatedUser object. Returns true iff successful.
        /// Returns false if the user doesn't exist, if the updated username already exists,
        /// or if the updated email already exists.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="updatedUser"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUser(string userId, User updatedUser)
        {
            User existingUser = _dbContext.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();
            if (existingUser == null)
            {
                _logger.LogWarning($"RepoLogic.UpdateUser() was called for a user that doesn't exist {userId}.");
                return false;
            }
            if (existingUser.Username != updatedUser.Username)
            {
                if (_dbContext.Users.Where(u => u.Username == updatedUser.Username).FirstOrDefault<User>() == null)
                {
                    existingUser.Username = updatedUser.Username;
                }
                else
                {
                    _logger.LogWarning($"RepoLogic.UpdateUser() was called with a updated username that already exists {updatedUser.Username}.");
                    return false;
                }
            }
            if (existingUser.Email != updatedUser.Email)
            {
                if (_dbContext.Users.Where(u => u.Email == updatedUser.Email).FirstOrDefault<User>() == null)
                {
                    existingUser.Email = updatedUser.Email;
                }
                else
                {
                    _logger.LogWarning($"RepoLogic.UpdateUser() was called with a updated email that already exists {updatedUser.Email}.");
                    return false;
                }
            }
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.DateOfBirth = updatedUser.DateOfBirth;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Returns the User object from the database that matches the email specified
        /// in the argument. Returns null if the email is not found.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User GetUserByEmail(string email)
        {
            return _dbContext.Users.Where(u => u.Email == email).FirstOrDefault<User>();
        }

        /// <summary>
        /// Returns the User object from the database that matches the userId specified
        /// in the argument. Returns null if the userId is not found.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUserByUserId(string userId)
        {
            return _dbContext.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();
        }

        /// <summary>
        /// Returns a list of all User objects in the database. If there are no users,
        /// the list will be empty.
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        /// <summary>
        /// Removes the user with the userId specified in the argument from the database.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUser(string userId)
        {
            User existingUser = _dbContext.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();
            if (existingUser == null)
            {
                _logger.LogWarning($"RepoLogic.DeleteUser() was called for a user that doesn't exist. {userId}");
                return false;
            }


            _dbContext.Users.Remove(existingUser);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetReportedUsers(List<string> ids)
        {
            return await _dbContext.Users.Where(u => ids.Contains(u.UserId)).ToListAsync();
        }

        /// <summary>
        /// Updates a user's permission to a specific level
        /// 1 = user, 2 = mod, 3 = admin
        /// Checks if user exists and if it's a valid permission level
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissionsLevel"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePermissions(string userId, int permissionsLevel)
        {
            User existingUser = _dbContext.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();
            if (existingUser == null)
            {
                _logger.LogWarning($"RepoLogic.UpdatePermissions() was called for a user that doesn't exist {userId}.");
                return false;
            }

            if (permissionsLevel > 255 || permissionsLevel < 0)
            {
                _logger.LogWarning($"RepoLogic.UpdatePermissions() was called with a permissionsLevel that is " +
                    $"outside the range of the database type (Byte) {permissionsLevel}.");
                return false;
            }
            existingUser.Permissions = (byte)permissionsLevel;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Returns true iff the userId, specified in the argument, exists in the database's Users table.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool UserExistsById(string userId)
        {
            return (_dbContext.Users.Where(u => u.UserId == userId).FirstOrDefault<User>() != null);
        }

        /// <summary>
        /// Returns true iff the user email exists in the database's Users table
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool UserExistsByEmail(string email)
        {
            return (_dbContext.Users.Where(u => u.Email == email).FirstOrDefault<User>() != null);
        }

        /// <summary>
        /// Returns true iff the user's username already exists in the database's Users table
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private bool UserExistsByUsername(string username)
        {
            return (_dbContext.Users.Where(u => u.Username == username).FirstOrDefault<User>() != null);
        }
    }
}
