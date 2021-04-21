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
            return await _repo.UpdateUser(userid, repoUser);
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
            return await _repo.DeleteUser(userid);
        }

        /// <summary>
        /// Updates a user's permission to be an admin
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task<bool> AddAsAdmin(Guid userid)
        {
            return await _repo.AddAsAdmin(userid);
        }

        public async Task<bool> IsUserExist(Guid userid)
        {
            User isUser = await _repo.GetUser(userid);
            if(isUser == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdatePermissions(Guid userid, int permissionLevel)
        {
            return await _repo.UpdatePermissions(userid, permissionLevel);
        }
    }
}
