using System;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using GlobalModels;
using Xunit;
using Repository;
using BusinessLogic;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

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
            newUser.Userid = oldUser.UserId;
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
                
                returnedValue = await Task.Run(() => test.UpdateUser(oldUser.UserId, newUser));
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

            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                returnedValue = await Task.Run(() => test.UpdateUser(newUser.Userid, newUser));
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

        [Fact]
        public async Task UpdateUser_FalseByEmailExist_Test()
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
            newUser.Email = oldUser1.Email;
            newUser.Permissions = 1;
            newUser.Username = "TestingTestington";
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

        [Fact]
        public async Task GetUser_True_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "OldTestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";

            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.SaveChanges();
            }

            GlobalModels.User returnUser;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                returnUser = await Task.Run(() => test.GetUser("oldtest@gmail.com"));
            }

            Assert.Equal(oldUser.UserId, returnUser.Userid);
        }

        [Fact]
        public async Task GetUser_UserNull_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "OldTestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";

            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.SaveChanges();
            }

            GlobalModels.User returnUser;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                returnUser = await Task.Run(() => test.GetUser("failtest@gmail.com"));
            }

            Assert.Null(returnUser);
        }

        [Fact]
        public async Task GetUsers_True_Test()
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

            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.Add<Repository.Models.User>(oldUser1);
                context.SaveChanges();
            }

            List<GlobalModels.User> returnUser;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                returnUser = await Task.Run(() => test.GetUsers());
            }

            Assert.Equal(2, returnUser.Count);
        }

        [Fact]
        public async Task DeleteUsers_True_Test()
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

            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.Add<Repository.Models.User>(oldUser1);
                context.SaveChanges();
            }

            bool successDelete;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                successDelete = await Task.Run(() => test.DeleteUser(oldUser.UserId));
            }

            Assert.True(successDelete);
        }

        [Fact]
        public async Task DeleteUsers_False_Test()
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

            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.Add<Repository.Models.User>(oldUser1);
                context.SaveChanges();
            }

            bool successDelete;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                successDelete = await Task.Run(() => test.DeleteUser(Guid.NewGuid().ToString()));
            }

            Assert.False(successDelete);
        }

        [Fact]
        public async Task UpdatePermissions_True_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "OldTestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";

            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.SaveChanges();
            }

            bool successUpdate;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                successUpdate = await Task.Run(() => test.UpdatePermissions(oldUser.UserId, 3));
            }

            Assert.True(successUpdate);
        }

        [Fact]
        public async Task UpdatePermissions_FalseByNullUser_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "OldTestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";

            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.SaveChanges();
            }

            bool successUpdate;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                successUpdate = await Task.Run(() => test.UpdatePermissions(Guid.NewGuid().ToString(), 3));
            }

            Assert.False(successUpdate);
        }

        [Fact]
        public async Task UpdatePermissions_FalseByInvalidPermissionLevelNegavtive_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "OldTestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";

            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.SaveChanges();
            }

            bool successUpdate;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                successUpdate = await Task.Run(() => test.UpdatePermissions(oldUser.UserId, -1));
            }

            Assert.False(successUpdate);
        }

        [Fact]
        public async Task GetUserAge_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "OldTestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";
            oldUser.DateOfBirth = DateTime.Parse("1994-05-20");

            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.SaveChanges();
            }

            double? age;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                age = await Task.Run(() => test.GetUserAge(oldUser.UserId));
            }

            Assert.Equal(26, age);
        }

        [Fact]
        public async Task GetUserAge_Null_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "OldTestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";
            oldUser.DateOfBirth = DateTime.Parse("1994-05-20");

            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.SaveChanges();
            }

            double? age;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                age = await Task.Run(() => test.GetUserAge(Guid.NewGuid().ToString()));
            }

            Assert.Null(age);
        }

        [Fact]
        public async Task FollowMovie_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "OldTestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";
            oldUser.DateOfBirth = DateTime.Parse("1994-05-20");

            string randomMovieId = Guid.NewGuid().ToString();

            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.SaveChanges();
            }

            bool successFollow;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                successFollow = await Task.Run(() => test.FollowMovie(oldUser.UserId, randomMovieId));
            }

            Assert.True(successFollow);
        }

        [Fact]
        public async Task FollowMovie_FalseByInvalidUser_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "OldTestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";
            oldUser.DateOfBirth = DateTime.Parse("1994-05-20");

            string randomMovieId = Guid.NewGuid().ToString();

            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.SaveChanges();
            }

            bool successFollow;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                successFollow = await Task.Run(() => test.FollowMovie(Guid.NewGuid().ToString(), randomMovieId));
            }

            Assert.False(successFollow);
        }

        [Fact]
        public async Task FollowMovie_FalseByAlreadyFollow_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "OldTestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";
            oldUser.DateOfBirth = DateTime.Parse("1994-05-20");

            string randomMovieId = Guid.NewGuid().ToString();

            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.SaveChanges();
            }

            bool successFollow;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                successFollow = await Task.Run(() => test.FollowMovie(oldUser.UserId, randomMovieId));
                successFollow = await Task.Run(() => test.FollowMovie(oldUser.UserId, randomMovieId));
            }

            Assert.False(successFollow);
        }

        [Fact]
        public async Task GetFollowingMovies_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "OldTestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";
            oldUser.DateOfBirth = DateTime.Parse("1994-05-20");

            Repository.Models.FollowingMovie repoFollowingMovie = new Repository.Models.FollowingMovie();
            repoFollowingMovie.UserId = oldUser.UserId;
            repoFollowingMovie.MovieId = Guid.NewGuid().ToString();
            Repository.Models.FollowingMovie repoFollowingMovie1 = new Repository.Models.FollowingMovie();
            repoFollowingMovie1.UserId = oldUser.UserId;
            repoFollowingMovie1.MovieId = Guid.NewGuid().ToString();

            string randomMovieId = Guid.NewGuid().ToString();

            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.Add<FollowingMovie>(repoFollowingMovie);
                context.Add<FollowingMovie>(repoFollowingMovie1);

                context.SaveChanges();
            }

            List<string> movieList;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                movieList = await Task.Run(() => test.GetFollowingMovies(oldUser.UserId));
            }

            Assert.Equal(2, movieList.Count);
        }

        [Fact]
        public async Task GetFollowingMovies_NullByInvalidUser_Test()
        {
            Repository.Models.User oldUser = new Repository.Models.User();
            oldUser.UserId = Guid.NewGuid().ToString();
            oldUser.Username = "OldTestingTestington";
            oldUser.FirstName = "OGTest";
            oldUser.LastName = "OriginalTest";
            oldUser.Permissions = 1;
            oldUser.Email = "oldtest@gmail.com";
            oldUser.DateOfBirth = DateTime.Parse("1994-05-20");

            Repository.Models.FollowingMovie repoFollowingMovie = new Repository.Models.FollowingMovie();
            repoFollowingMovie.UserId = oldUser.UserId;
            repoFollowingMovie.MovieId = Guid.NewGuid().ToString();
            Repository.Models.FollowingMovie repoFollowingMovie1 = new Repository.Models.FollowingMovie();
            repoFollowingMovie1.UserId = oldUser.UserId;
            repoFollowingMovie1.MovieId = Guid.NewGuid().ToString();

            string randomMovieId = Guid.NewGuid().ToString();

            using(var context = new Cinephiliacs_UserContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add<Repository.Models.User>(oldUser);
                context.Add<FollowingMovie>(repoFollowingMovie);
                context.Add<FollowingMovie>(repoFollowingMovie1);

                context.SaveChanges();
            }

            List<string> movieList;
            using(var context1 = new Cinephiliacs_UserContext(options))
            {
                context1.Database.EnsureCreated();
                
                RepoLogic userRepo = new RepoLogic(context1);
                UserLogic test = new UserLogic(userRepo);
                
                movieList = await Task.Run(() => test.GetFollowingMovies(Guid.NewGuid().ToString()));
            }

            Assert.Null(movieList);
        }
    }
}
