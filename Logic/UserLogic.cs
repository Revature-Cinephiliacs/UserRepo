using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repository;
using GlobalModels;

namespace BusinessLogic
{
    public class UserLogic : Interfaces.IUserLogic
    {
        private readonly RepoLogic _repo;
        
        public UserLogic(RepoLogic repo)
        {
            _repo = repo;
        }
        /// <summary>
        /// Adds a new User Object to repository.
        /// Returns true if sucessful; false otherwise.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> CreateUser(User user)
        {
            var repoUser = Mapper.UserToRepoUser(user);
            return await _repo.AddUser(repoUser);
        }

        /// <summary>
        /// Updates a User Object in repository.
        /// Returns true if sucessful; false otherwise.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUser(Guid userid, User user)
        {
            var repoUser = Mapper.UserToRepoUser(user);
            return await _repo.UpdateUser(userid.ToString(), repoUser);
        }

         /// <summary>
        /// Returns the User object whose Username is equal to the username argument.
        /// Returns null if the username is not found.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>User</returns>
        public User GetUser(string useremail)
        {
            var repoUser = _repo.GetUser(useremail);
            if(repoUser == null)
            {
                Console.WriteLine("UserLogic.GetUser() was called with a nonexistant username.");
                return null;
            }
            return Mapper.RepoUserToUser(repoUser);
        }

        /// <summary>
        /// Returns a list of every User object.
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetUsers()
        {
            var repoUsers = await _repo.GetUsers();

            var users = new List<User>();
            foreach (var repoUser in repoUsers)
            {
                users.Add(Mapper.RepoUserToUser(repoUser));
            }
            return users;
        }

        /// <summary>
        /// Delete the user
        /// Return true if successful
        /// </summary>
        public async Task<bool> DeleteUser(Guid userid)
        {
            return await _repo.DeleteUser(userid.ToString());
        }

        /// <summary>
        /// Checks if a user already exists in the database by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsUserExistByEmail(string email)
        {
            var isUser = _repo.GetUser(email);
            if(isUser == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Updates the permission levels of a user in the database
        /// Returns true is successful, else return false.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="permissionLevel"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePermissions(Guid userid, int permissionLevel)
        {
            return await _repo.UpdatePermissions(userid.ToString(), permissionLevel);
        }

        /// <summary>
        /// Get a list of movies that a user is following using the user's id
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<List<string>> GetFollowingMovies(Guid userid)
        {
            List<Repository.Models.FollowingMovie> repoFollowingMovies = await _repo.GetFollowingMovies(userid.ToString());
            if(repoFollowingMovies == null)
            {
                Console.WriteLine("UserLogic.GetFollowingMovies() was called for a username that doesn't exist.");
                return null;
            }

            List<string> followingMovies = new List<string>();
            foreach (var repoFollowingMovie in repoFollowingMovies)
            {
                followingMovies.Add(repoFollowingMovie.MovieId);
            }
            return followingMovies;
        }

        /// <summary>
        /// Have a user start following a movie using the userid and movieid
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="movieid"></param>
        /// <returns></returns>
        public async Task<bool> FollowMovie(Guid userid, string movieid)
        {
            Repository.Models.FollowingMovie repoFollowingMovie = new Repository.Models.FollowingMovie();
            repoFollowingMovie.UserId = userid.ToString();
            repoFollowingMovie.MovieId = movieid;
            
            return await _repo.FollowMovie(repoFollowingMovie);
        }
    }
}
