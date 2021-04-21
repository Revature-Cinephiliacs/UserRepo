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
            if (UserExists(repoUser.Username))
            {
                Console.WriteLine("RepoLogic.AddUser() was called for a user that already exists.");
                return false;
            }
            await _dbContext.Users.AddAsync(repoUser);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Updates the information of the User identified by the username argument
        /// to the information in the User object. Returns true iff successful.
        /// Returns false if the user doesn't exist.
        /// </summary>
        /// <param name="repoUser"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUser(string username, User updatedUser)
        {
            User existingUser = _dbContext.Users.Where(u => u.Username == username).FirstOrDefault<User>();
            if (existingUser == null)
            {
                Console.WriteLine("RepoLogic.UpdateUser() was called for a user that doesn't exist.");
                return false;
            }
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Email = updatedUser.Email;

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
        /// Returns a list of all FollowingMovie objects from the database that match the username
        /// specified in the argument. Returns null if the user doesn't exist.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<List<FollowingMovie>> GetFollowingMovies(string username)
        {
            var userExists = UserExists(username);
            if(!userExists)
            {
                Console.WriteLine("RepoLogic.GetFollowingMovies() was called for a user that doesn't exist.");
                return null;
            }
            string userId = GetUserId(username);
            if(userId == null)
            {
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
            string username = GetUsername(followingMovie.UserId);
            if(username == null)
            {
                return false;
            }
            var userExists = UserExists(username);
            if(!userExists)
            {
                Console.WriteLine("RepoLogic.FollowMovie() was called for a user that doesn't exist.");
                return false;
            }
// MS API CALL: Make sure movie exists

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
        /// Returns true iff the username, specified in the argument, exists in the database's Users table.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private bool UserExists(string username)
        {
            return (_dbContext.Users.Where(u => u.Username == username).FirstOrDefault<User>() != null);
        }

        private string GetUserId(string username)
        {
            User user = _dbContext.Users.Where(u => u.Username == username).FirstOrDefault<User>();
            if(user == null)
            {
                return null;
            }
            return user.UserId;
        }

        private string GetUsername(string userId)
        {
            User user = _dbContext.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();
            if(user == null)
            {
                return null;
            }
            return user.Username;
        }
    }
}
