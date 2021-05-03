using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repository;
using GlobalModels;
using Microsoft.Extensions.Logging;
using System.Linq;

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
            if (repoUser == null)
            {
                return null;
            }
            return await Task.Run(() => Mapper.RepoUserToUser(repoUser));
        }

        public async Task<string> GetUserNameById(string userid)
        {
            Repository.Models.User repoUser = await Task.Run(() => _repo.GetUserByUserId(userid));
            if (repoUser == null)
            {
                return null;
            }
            return repoUser.Username;
        }

        public User GetUser(string useremail)
        {
            var repoUser = _repo.GetUserByEmail(useremail);
            if (repoUser == null)
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
            if (repoAllUsers == null)
            {
                return null;
            }
            List<User> allUsers = new List<User>();
            List<Task<User>> tasks = new List<Task<User>>();
            foreach (Repository.Models.FollowingUser user in repoAllUsers)
            {
                tasks.Add(Task.Run(() => Mapper.RepoUserToUser(user.FolloweeUser)));
            }
            var results = await Task.WhenAll(tasks);
            foreach (var item in results)
            {
                allUsers.Add(item);
            }
            return allUsers;
        }

        public double? GetUserAge(string userid)
        {
            Repository.Models.User repoUser = _repo.GetUserByUserId(userid);
            if (repoUser == null || repoUser.DateOfBirth == null)
            {
                return null;
            }
            DateTime dateOfBirth = repoUser.DateOfBirth ?? DateTime.Now;
            DateTime now = DateTime.Now;

            int years = DateTime.Now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month)
            {
                years -= 1;
            }
            else if (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)
            {
                years -= 1;
            }
            return years;
        }

        public async Task CreateNotifications(ModelNotification modelNotifications, string type)
        {
            List<Repository.Models.Notification> newNotifications = new List<Repository.Models.Notification>();
            List<Task<Repository.Models.Notification>> tasks = new List<Task<Repository.Models.Notification>>();
            Task<List<Repository.Models.FollowingUser>> followeeTask = Task.Run(() => _repo.GetUserFollowees(modelNotifications.UserId));

            foreach (string userid in modelNotifications.Followers)
            {
                tasks.Add(Task.Run(() => Mapper.ModelNotifToRepoNotif(userid, modelNotifications.OtherId, type, null)));
            }
            var results = await Task.WhenAll(tasks);
            foreach (var item in results)
            {
                newNotifications.Add(item);
            }
            List<Task> repoTasks = new List<Task>();
            foreach (var notification in newNotifications)
            {
                repoTasks.Add(Task.Run(() => _repo.AddNotification(notification)));
            }
            followeeTask.Wait();
            List<Repository.Models.FollowingUser> allFollowees = followeeTask.Result;
            List<Repository.Models.Notification> otherNotifications = await Task.Run(() => FolloweeNotifications(modelNotifications.OtherId, type, allFollowees, newNotifications, modelNotifications.UserId));
            foreach (var notification in otherNotifications)
            {
                repoTasks.Add(Task.Run(() => _repo.AddNotification(notification)));
            }
            await Task.WhenAll(repoTasks);
        }

        public async Task<List<NotificationDTO>> GetNotifications(string userid)
        {
            List<Repository.Models.Notification> repoNotifications = await _repo.GetNotifications(userid);
            if (repoNotifications == null)
            {
                return null;
            }
            var userIdList = repoNotifications.Select(n => n.CreatorId).Where(u => u != null).ToList();
            var users = await _repo.GetReportedUsers(userIdList);
            List<NotificationDTO> newNotifications = new List<NotificationDTO>();
            List<Task<NotificationDTO>> tasks = new List<Task<NotificationDTO>>();
            foreach (var repoN in repoNotifications)
            {
                tasks.Add(Task.Run(() => Mapper.RepoNotifToNotifDTO(repoN)));

            }
            var results = await Task.WhenAll(tasks);
            foreach (var item in results)
            {
                if (item.CreatorId != null)
                    item.CreatorUsername = users.Where(u => u.UserId == item.CreatorId).FirstOrDefault().Username;
                newNotifications.Add(item);
            }
            return newNotifications;
        }

        public async Task<bool> DeleteSingleNotification(Guid notificationid)
        {
            return await _repo.DeleteNotification(notificationid.ToString());
        }

        public async Task<bool> DeleteNotifications(string userid)
        {
            return await _repo.DeleteAllNotifications(userid);
        }

        /// <summary>
        /// Returns a list of new notifications that were not created based on a user following another user
        /// Goes through the list of newly created notifications and checks if the followeeid + otherid combination is there
        /// If not, will make a new notification to be returned
        /// </summary>
        /// <param name="followeeeid"></param>
        /// <param name="otherid"></param>
        /// <param name="curNotif"></param>
        /// <returns></returns>
        private async Task<List<Repository.Models.Notification>> FolloweeNotifications(Guid otherid, string type, List<Repository.Models.FollowingUser> followees, List<Repository.Models.Notification> curNotif, string creatorid)
        {
            List<Repository.Models.Notification> newNotifications = new List<Repository.Models.Notification>();
            List<Task<Repository.Models.Notification>> tasks = new List<Task<Repository.Models.Notification>>();
            foreach (Repository.Models.FollowingUser followee in followees)
            {
                if (!curNotif.Exists(x => x.UserId == followee.FolloweeUserId))
                {
                    tasks.Add(Task.Run(() => Mapper.ModelNotifToRepoNotif(followee.FolloweeUserId, otherid, type, creatorid)));
                }
            }
            var results = await Task.WhenAll(tasks);
            foreach (var item in results)
            {
                newNotifications.Add(item);
            }
            return newNotifications;
        }
    }
}
