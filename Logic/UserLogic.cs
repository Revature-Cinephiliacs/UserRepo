using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repository;
using Repository.Models;

namespace BusinessLogic
{
    public class UserLogic : Interfaces.IUserLogic
    {
        private readonly RepoLogic _repo;
        
        public UserLogic(RepoLogic repo)
        {
            _repo = repo;
        }

        public async Task<bool> CreateUser(User user)
        {
            var repoUser = Mapper.UserToRepoUser(user);
            return await _repo.AddUser(repoUser);
        }

        public async Task<bool> UpdateUser(string username, User user)
        {
            var repoUser = Mapper.UserToRepoUser(user);
            return await _repo.UpdateUser(username, repoUser);
        }

        public User GetUser(string username)
        {
            var repoUser = _repo.GetUser(username);
            if(repoUser == null)
            {
                Console.WriteLine("UserLogic.GetUser() was called with a nonexistant username.");
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

        public async Task<bool> DeleteUser(string uid)
        {
            return await _repo.DeleteUser(uid);
        }

        public async Task<bool> AddAsAdmin(string uid)
        {
            return await _repo.AddAsAdmin(uid);
        }
    }
}
