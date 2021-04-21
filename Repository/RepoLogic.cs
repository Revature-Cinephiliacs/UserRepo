using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace Repository
{
    public class RepoLogic
    {
        private readonly Cinephiliacs_UserContext _dbContext;

        public RepoLogic(Cinephiliacs_UserContext dbContext)
        {
            _dbContext = dbContext;
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
                Console.WriteLine("RepoLogic.AddUser() was called for a userid that already exists.");
                return false;
            }
            if (UserExistsByEmail(repoUser.Email))
            {
                Console.WriteLine("RepoLogic.AddUser() was called for a useremail that already exists.");
                return false;
            }
            if (UserExistsByUsername(repoUser.Username))
            {
                Console.WriteLine("RepoLogic.AddUser() was called for a username that already exists.");
                return false;
            }
            await _dbContext.Users.AddAsync(repoUser);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Updates the information of the User identified by the userId argument
        /// to the information in the updatedUser object. Returns true iff successful.
        /// Returns false if the user doesn't exist, if the updated username already exists,
        /// or if the updated email already exists.
        /// </summary>
        /// <param name="repoUser"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUser(string userId, User updatedUser)
        {
            User existingUser = _dbContext.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();
            if (existingUser == null)
            {
                Console.WriteLine("RepoLogic.UpdateUser() was called for a user that doesn't exist.");
                return false;
            }
            if(existingUser.Username != updatedUser.Username)
            {
                if(_dbContext.Users.Where(u => u.Username == updatedUser.Username).FirstOrDefault<User>() == null)
                {
                    existingUser.Username = updatedUser.Username;
                }
                else
                {
                    Console.WriteLine("RepoLogic.UpdateUser() was called with a updated username that already exists.");
                    return false;
                }
            }
            if(existingUser.Email != updatedUser.Email)
            {
                if(_dbContext.Users.Where(u => u.Email == updatedUser.Email).FirstOrDefault<User>() == null)
                {
                    existingUser.Email = updatedUser.Email;
                }
                else
                {
                    Console.WriteLine("RepoLogic.UpdateUser() was called with a updated email that already exists.");
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
        /// <param name="username"></param>
        /// <returns></returns>
        public User GetUser(string email)
        {
            return _dbContext.Users.Where(u => u.Email == email).FirstOrDefault<User>();
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
                Console.WriteLine("RepoLogic.DeleteUser() was called for a user that doesn't exist.");
                return false;
            }

            _dbContext.Users.Remove(existingUser);

            await _dbContext.SaveChangesAsync();
            return true;
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
                Console.WriteLine("RepoLogic.UpdatePermissions() was called for a user that doesn't exist.");
                return false;
            }

            if(permissionsLevel > 255 || permissionsLevel < 0)
            {
                Console.WriteLine("RepoLogic.UpdatePermissions() was called with a permissionsLevel that is " +
                    "outside the range of the database type (Byte).");
                return false;
            }
            existingUser.Permissions = (byte)permissionsLevel;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Returns a list of all FollowingMovie objects from the database that match the userId
        /// specified in the argument. Returns null if the user doesn't exist.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<List<FollowingMovie>> GetFollowingMovies(string userId)
        {
            if(!UserExistsById(userId))
            {
                Console.WriteLine("RepoLogic.GetFollowingMovies() was called for a user that doesn't exist.");
                return null;
            }
            return await _dbContext.FollowingMovies.Where(f => f.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Adds the FollowingMovie specified in the argument to the database.
        /// Returns true iff successful.
        /// Returns false if the username or movieid referenced in the FollowingMovie object
        /// do not already exist in their respective database tables.
        /// </summary>
        /// <param name="followingMovie"></param>
        /// <returns></returns>
        public async Task<bool> FollowMovie(FollowingMovie followingMovie)
        {
            if(!UserExistsById(followingMovie.UserId))
            {
                Console.WriteLine("RepoLogic.FollowMovie() was called for a user that doesn't exist.");
                return false;
            }
            // MS API CALL: Make sure movie exists //

            // Ensure the User is not already Following this Movie
            if((await _dbContext.FollowingMovies.Where(fm => 
                    fm.UserId == followingMovie.UserId 
                    && fm.MovieId == followingMovie.MovieId
                ).FirstOrDefaultAsync<FollowingMovie>()) != null)
            {
                Console.WriteLine("RepoLogic.FollowMovie() was called for a movie that the user is " +
                    "already following.");
                return false;
            }

            await _dbContext.FollowingMovies.AddAsync(followingMovie);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Returns true iff the userId, specified in the argument, exists in the database's Users table.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private bool UserExistsById(string userId)
        {
            return (_dbContext.Users.Where(u => u.UserId == userId).FirstOrDefault<User>() != null);
        }

        /// <summary>
        /// Returns true iff the user email exists in the database's Users table
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool UserExistsByEmail(string email)
        {
            return (_dbContext.Users.Where(u => u.Email == email).FirstOrDefault<User>() != null);
        }

        /// <summary>
        /// Returns true iff the user's username already exists in the database's Users table
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool UserExistsByUsername(string username)
        {
            return (_dbContext.Users.Where(u => u.Username == username).FirstOrDefault<User>() != null);
        }
    }
}
