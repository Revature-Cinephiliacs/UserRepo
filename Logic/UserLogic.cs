using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repository;
using GlobalModels;
using Microsoft.Extensions.Logging;

namespace BusinessLogic
{
    /// <summary>
    /// Comments in interface(IUserLogic.cs)
    /// </summary>
    public class UserLogic : Interfaces.IUserLogic
    {
        private readonly RepoLogic _repo;
        private readonly ILogger<UserLogic> _logger;

        public UserLogic(RepoLogic repo)
        {
            _repo = repo;
        }
        
        public UserLogic(RepoLogic repo, ILogger<UserLogic> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        
        public async Task<bool> CreateUser(User user)
        {
            var repoUser = Mapper.UserToRepoUser(user);
            return await _repo.AddUser(repoUser);
        }

        public async Task<bool> UpdateUser(string userid, User user)
        {
            var repoUser = Mapper.UserToRepoUser(user);
            return await _repo.UpdateUser(userid, repoUser);
        }

        public User GetUser(string useremail)
        {
            var repoUser = _repo.GetUserByEmail(useremail);
            if(repoUser == null)
            {
                return null;
            }
            return Mapper.RepoUserToUser(repoUser);
        }

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

        public async Task<bool> DeleteUser(string userid)
        {
            return await _repo.DeleteUser(userid);
        }

        public async Task<bool> UpdatePermissions(string userid, int permissionLevel)
        {
            return await _repo.UpdatePermissions(userid, permissionLevel);
        }

        public async Task<List<string>> GetFollowingMovies(string userid)
        {
            List<Repository.Models.FollowingMovie> repoFollowingMovies = await _repo.GetFollowingMovies(userid);
            if(repoFollowingMovies == null)
            {
                return null;
            }

            List<string> followingMovies = new List<string>();
            foreach (var repoFollowingMovie in repoFollowingMovies)
            {
                followingMovies.Add(repoFollowingMovie.MovieId);
            }
            return followingMovies;
        }

        public async Task<bool> FollowMovie(string userid, string movieid)
        {
            Repository.Models.FollowingMovie repoFollowingMovie = new Repository.Models.FollowingMovie();
            repoFollowingMovie.UserId = userid;
            repoFollowingMovie.MovieId = movieid;
            
            return await _repo.FollowMovie(repoFollowingMovie);
        }

        public double? GetUserAge(string userid)
        {
            Repository.Models.User repoUser = _repo.GetUserByUserId(userid);
            if(repoUser == null || repoUser.DateOfBirth == null)
            {
                return null;
            }
            DateTime dateOfBirth = repoUser.DateOfBirth ?? DateTime.Now;
            DateTime now = DateTime.Now;

            int years = DateTime.Now.Year  - dateOfBirth.Year;
            if(now.Month < dateOfBirth.Month)
            {
                years -= 1;
            }
            else if(now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)
            {
                years -= 1;
            }
            return years;
        }
    }
}
