using System;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using GlobalModels;
using Xunit;
using Repository;
using BusinessLogic;
using System.Threading.Tasks;
using System.Linq;

namespace Testing
{
    public class UserLogicTest
    {
        readonly DbContextOptions<Cinephiliacs_UserContext> options = new DbContextOptionsBuilder<Cinephiliacs_UserContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;

        [Fact]
        public async Task CreateUser_True_Test()
        {
            GlobalModels.User newUser = new GlobalModels.User();
            newUser.Firstname = "Test";
            newUser.Lastname = "Testing";
            newUser.Email = "Testing@email.com";
            newUser.Permissions = 1;
            newUser.Username = "TestingTestington";
            newUser.Dateofbirth = "1990-05-20";

            bool returnedValue;
            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                RepoLogic userRepo = new RepoLogic(context);
                UserLogic test = new UserLogic(userRepo);
                returnedValue = await Task.Run(() => test.CreateUser(newUser));
            }

            Repository.Models.User dbUser;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                dbUser = context1.Users.FirstOrDefault(x => x.UserId == newUser.Userid.ToString());
            }

            Assert.Equal(newUser.Username, dbUser.Username);
        }

        [Fact]
        public async Task CreateUser_FalseByID_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "TestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";
            GlobalModels.User newUser = new GlobalModels.User();
            newUser.Userid = Guid.Parse(oldUser.UserId);
            newUser.Firstname = "Test";
            newUser.Lastname = "Testing";
            newUser.Email = "Testing@email.com";
            newUser.Permissions = 1;
            newUser.Username = "TestingTestington";
            newUser.Dateofbirth = "1990-05-20";

            bool returnedValue;
            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                
                context.Add<Repository.Models.User>(oldUser);
                context.SaveChanges();
            }

            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);

                returnedValue = await Task.Run(() => test.CreateUser(newUser));
            }

            Assert.False(returnedValue);
        }

        [Fact]
        public async Task CreateUser_FalseByEmail_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "TestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";
            GlobalModels.User newUser = new GlobalModels.User();
            newUser.Firstname = "Test";
            newUser.Lastname = "Testing";
            newUser.Email = oldUser.Email;
            newUser.Permissions = 1;
            newUser.Username = "TestingTestington";
            newUser.Dateofbirth = "1990-05-20";

            bool returnedValue;
            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                
                context.Add<Repository.Models.User>(oldUser);
                context.SaveChanges();
            }

            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);

                returnedValue = await Task.Run(() => test.CreateUser(newUser));
            }

            Assert.False(returnedValue);
        }

        [Fact]
        public async Task CreateUser_FalseByUsername_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "TestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";
            GlobalModels.User newUser = new GlobalModels.User();
            newUser.Firstname = "Test";
            newUser.Lastname = "Testing";
            newUser.Email = "Testing@email.com";
            newUser.Permissions = 1;
            newUser.Username = oldUser.Username;
            newUser.Dateofbirth = "1990-05-20";

            bool returnedValue;
            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                
                context.Add<Repository.Models.User>(oldUser);
                context.SaveChanges();
            }

            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);

                returnedValue = await Task.Run(() => test.CreateUser(newUser));
            }

            Assert.False(returnedValue);
        }

        [Fact]
        public async Task UpdateUser_True_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "OldTestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";
            GlobalModels.User newUser = new GlobalModels.User();
            newUser.Firstname = "Test";
            newUser.Lastname = "Testing";
            newUser.Email = "Testing@email.com";
            newUser.Permissions = 1;
            newUser.Username = "TestingTestington";
            newUser.Dateofbirth = "1990-05-20";

            bool returnedValue;
            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.SaveChanges();
            }

            Repository.Models.User dbUser;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                returnedValue = await Task.Run(() => test.UpdateUser(Guid.Parse(oldUser.UserId), newUser));
                dbUser = context1.Users.FirstOrDefault(x => x.UserId == oldUser.UserId);
            }

            Assert.Equal(newUser.Username, dbUser.Username);
        }

        [Fact]
        public async Task UpdateUser_FalseByUserNull_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "OldTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";
            GlobalModels.User newUser = new GlobalModels.User();
            newUser.Firstname = "Test";
            newUser.Lastname = "Testing";
            newUser.Email = "Testing@email.com";
            newUser.Permissions = 1;
            newUser.Username = "TestingTestington";
            newUser.Dateofbirth = "1990-05-20";

            bool returnedValue;
            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.SaveChanges();
            }

            Repository.Models.User dbUser;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                returnedValue = await Task.Run(() => test.UpdateUser(newUser.Userid, newUser));
                dbUser = context1.Users.FirstOrDefault(x => x.UserId == oldUser.UserId);
            }

            Assert.False(returnedValue);
        }

        [Fact]
        public async Task UpdateUser_FalseByUsernameExist_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "OldTestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";
            Repository.Models.User oldUser1 = new Repository.Models.User();
            oldUser1.UserId = Guid.NewGuid().ToString();
            oldUser1.Username = "OldTestingTestington1";
            oldUser1.FirstName = "OGTest1";
            oldUser1.LastName = "OriginalTest1";
            oldUser1.Permissions = 1;
            oldUser1.Email = "oldtest1@gmail.com";
            GlobalModels.User newUser = new GlobalModels.User();
            newUser.Firstname = "Test";
            newUser.Lastname = "Testing";
            newUser.Email = "Testing@email.com";
            newUser.Permissions = 1;
            newUser.Username = oldUser1.Username;
            newUser.Dateofbirth = "1990-05-20";

            bool returnedValue;
            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.Add<Repository.Models.User>(oldUser1);
                context.SaveChanges();
            }

            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                returnedValue = await Task.Run(() => test.UpdateUser(newUser.Userid, newUser));
            }

            Assert.False(returnedValue);
        }
    }
}
