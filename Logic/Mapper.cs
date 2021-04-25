using System;
using System.Globalization;
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
                Console.WriteLine("Mapper.UserToRepoUser() failed due to an invalid date");
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
    }
}
