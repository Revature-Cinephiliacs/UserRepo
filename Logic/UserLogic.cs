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

        public async Task<User> GetUserById(string userid)
        {
            var repoUser = await Task.Run(() => _repo.GetUserByUserId(userid));
            if(repoUser == null)
            {
                return null;
            }
            return await Task.Run(() => Mapper.RepoUserToUser(repoUser));
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

        public async Task<bool> FollowUser(string followerid, string followeeid)
        {
            Repository.Models.FollowingUser newFollow = new Repository.Models.FollowingUser();
            newFollow.FollowerUserId = followerid;
            newFollow.FolloweeUserId = followeeid;
            return await _repo.FollowUser(newFollow);
        }

        public async Task<List<User>> GetFollowingUsers(string userid)
        {
            List<Repository.Models.FollowingUser> repoAllUsers = await _repo.GetFollowingUsers(userid);
            if(repoAllUsers == null)
            {
                return null;
            }
            List<User> allUsers = new List<User>();
            List<Task<User>> tasks = new List<Task<User>>();
            foreach(Repository.Models.FollowingUser user in repoAllUsers)
            {
                tasks.Add(Task.Run(() => Mapper.RepoUserToUser(user.FolloweeUser)));
            }
            var results = await Task.WhenAll(tasks);
            foreach(var item in results)
            {
                allUsers.Add(item);
            }
            return allUsers;
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
