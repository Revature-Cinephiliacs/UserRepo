using System;
using System.Globalization;
using System.Threading.Tasks;
using GlobalModels;

namespace BusinessLogic
{
    public static class Mapper
    {
        /// <summary>
        /// Maps an instance of GlobalModels.User onto a new instance of
        /// Repository.Models.User. Returns null if the Date format was
        /// invalid.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Repository.Models.User UserToRepoUser(User user)
        {
            var repoUser = new Repository.Models.User();
            repoUser.UserId = user.Userid;
            repoUser.Username = user.Username;
            repoUser.FirstName = user.Firstname;
            repoUser.LastName = user.Lastname;
            repoUser.Email = user.Email;
            try {
                repoUser.DateOfBirth = DateTime.ParseExact(user.Dateofbirth, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            catch (FormatException) {
                return null;
            }
            repoUser.Permissions = 1;

            return repoUser;
        }

        /// <summary>
        /// Maps an instance of Repository.Models.User onto a new instance of
        /// GlobalModels.User
        /// </summary>
        /// <param name="repoUser"></param>
        /// <returns></returns>
        public static User RepoUserToUser(Repository.Models.User repoUser)
        {
            DateTime repoDate = repoUser.DateOfBirth ?? DateTime.Now;
            string dtoDate = repoDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var user = new User(repoUser.UserId, repoUser.Username, repoUser.FirstName, repoUser.LastName,
                repoUser.Email, dtoDate, repoUser.Permissions);
            return user;
        }

        /// <summary>
        /// Converts a comment notification to a notification to be stored in the database
        /// service: c = comments, d = disuccsion, r = review
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="commentId"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        internal static Repository.Models.Notification ModelNotifToRepoNotif(string userid, Guid commentId, string service, string creatorid)
        {
            Repository.Models.Notification newNotification = new Repository.Models.Notification();
            newNotification.NotificationId = Guid.NewGuid().ToString();
            newNotification.UserId = userid;
            newNotification.OtherId = commentId.ToString();
            newNotification.FromService = service;
            newNotification.CreatorId = creatorid;
            return newNotification;
        }

        /// <summary>
        /// Converts a repo notification to a notificationDTO for frontend to use
        /// </summary>
        /// <param name="repoN"></param>
        /// <returns></returns>
        internal static NotificationDTO RepoNotifToNotifDTO(Repository.Models.Notification repoN)
        {
            NotificationDTO newNotif = new NotificationDTO();
            newNotif.NotificationId = repoN.NotificationId;
            newNotif.OtherId = Guid.Parse(repoN.OtherId);
            newNotif.FromService = repoN.FromService;
            return newNotif;
        }
    }
}
