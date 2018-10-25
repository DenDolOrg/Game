using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyGame.BLL.DTO;
using MyGame.BLL.Services;
using MyGame.DAL.Entities;
using MyGame.DAL.EntityFramework;
using MyGame.Tests.MockManagers;
using MyGame.Tests.MockStores;
using MyGame.Tests.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.BLL.Services.Tests
{
    [TestClass()]
    public class UserServiceTests
    {
        MockUnitOfWork Work;
        MockUserManager userManager;
        MockRoleManager roleManager;
        MockPlayerManager playerManager;

        [TestInitialize]
        public void Init()
        {
            Work = new MockUnitOfWork();
            ServiceDataToUse.SetData();
        }

        #region AUTHENTICATE
        [TestMethod()]
        public async Task AuthenticateTest()
        {
            //Arrange
            userManager = new MockUserManager(new MockUserStore().Object)
                .MockFindByEmailAsync()
                .MockCheckPasswordAsync()
                .MockCreateIdentityAsync();

            Work.SetManagers(userManager);

            UserDTO user_good = new UserDTO { Email = ServiceDataToUse.User.Email, Password = ServiceDataToUse.User.PasswordHash };
            UserDTO user_bad = new UserDTO { Email = "email_bad@gmail.com", Password = "somePassword" };

            //Act
            UserService service = new UserService(Work.Object);

            var result_good = await service.Authenticate(user_good);
            var result_bad = await service.Authenticate(user_bad);

            //Assert

            Assert.IsNotNull(result_good, "Did not authenticate user with goog login data.");
            Assert.IsNull(result_bad, "Authenticate user with bad login data.");
        }
        #endregion

        #region DELETE
        [TestMethod()]
        public async Task DeleteTest()
        {
            //Arrange
            var newUser = new Mock<ApplicationUser>();
            newUser.Setup(m => m.Logins).Returns(new List<UserLogin> { new UserLogin { UserId = ServiceDataToUse.User.Id, LoginProvider = "provider", ProviderKey = "key" } });
            newUser.Setup(m => m.Id).Returns(ServiceDataToUse.User.Id);
            newUser.Setup(m => m.PlayerProfile).Returns(ServiceDataToUse.User.PlayerProfile);

            ServiceDataToUse.User = newUser.Object;

            MockUserManager userManager = new MockUserManager(new MockUserStore().Object)
                .MockFindByIdAsync()
                .MockRemoveLoginAsync()
                .MockRemoveFromRoleAsync()
                .MockGetRolesAsync()
                .MockDeleteAsync();

            MockPlayerManager playerManager = new MockPlayerManager()
                .MockDeleteAsync();

            Work.SetManagers(userManager,null, null, null, null, playerManager);

            UserDTO user_good = new UserDTO { Id = ServiceDataToUse.User.Id };
            UserDTO user_bad = new UserDTO { Id = 123 };

            //Act
            UserService service = new UserService(Work.Object);

            var result_good = await service.Delete(user_good);
            var result_bad = await service.Delete(user_bad);

            //Assert

            Assert.IsTrue(result_good.Succedeed, "Failed while deleting user with valid data.");
            Assert.IsFalse(result_bad.Succedeed, "Deleted user with invalid data.");
        }
        #endregion

        #region CREATE
        [TestMethod()]
        public async Task CreateTest()
        {
            //Arrange
            var user_good = new UserDTO { Id = 2, Email = "new_email@gmail.com", UserName = "new_username" };
            var user_bad = new UserDTO { Id = ServiceDataToUse.User.Id, Email = ServiceDataToUse.User.Email, UserName = ServiceDataToUse.User.UserName };

            userManager = new MockUserManager(new MockUserStore().Object)
                .MockFindByEmailAsync()
                .MockFindByNameAsync()
                .MockCreateAsync()
                .MockAddToRoleAsync();

            playerManager = new MockPlayerManager()
                .MockCreateAsync();

            Work.SetManagers(userManager, null, null, null, null, playerManager);

            //Act
            var service = new UserService(Work.Object);

            var result_good = await service.Create(user_good);
            var result_bad = await service.Create(user_bad);

            //Assert
            Assert.IsTrue(result_good.Succedeed, "Failed while creating user with valid data.");
            Assert.IsFalse(result_bad.Succedeed, "Created user with invalid data.");
        }
        #endregion

        #region SET_INITIAL_DATA
        [TestMethod()]
        public async Task SetInitialDataTest()
        {
            //Arrange
            List<ApplicationRole> roles = new List<ApplicationRole>
            {
               new ApplicationRole{ Id = 1, Name = "user"},
               new ApplicationRole{ Id = 2, Name = "admin"}
            };

            var newUser = new Mock<ApplicationUser>();
            newUser.Setup(m => m.Email).Returns(ServiceDataToUse.User.Email);
            newUser.Setup(m => m.Id).Returns(ServiceDataToUse.User.Id);
            newUser.Setup(m => m.Roles).Returns(new List<UserRole> { new UserRole { RoleId = 1, UserId = ServiceDataToUse.User.Id} });

            ServiceDataToUse.User = newUser.Object;

            var userDTO = new UserDTO { Id = ServiceDataToUse.User.Id, Email = ServiceDataToUse.User.Email };

            userManager = new MockUserManager(new MockUserStore().Object)
                .MockFindByEmailAsync()
                .MockAddToRoleAsync();

            roleManager = new MockRoleManager(new RoleStore(new ApplicationContext("some")))
                .MockCreateAsync()
                .MockFindByNameAsync()
                .MockRoles(roles);

            Work.SetManagers(userManager,null, null, null, roleManager);

            UserService service = new UserService(Work.Object);

            try
            {
                //Act
                await service.SetInitialData(userDTO, (from r in roles
                                                       select r.Name).ToList());
            }
            catch
            {
                //Assert
                Assert.Fail("Error while setting data.");
            }

        }
        #endregion

        #region GET_USER
        [TestMethod()]
        public async Task GetUserTest()
        {
            //Arrange
            userManager = new MockUserManager(new MockUserStore().Object)
                .MockFindByNameAsync();

            Work.SetManagers(userManager);

            UserDTO user_good = new UserDTO { UserName = ServiceDataToUse.User.UserName };
            UserDTO user_bad = new UserDTO { UserName = "bad_username" };

            //Act
            UserService service = new UserService(Work.Object);

            var result_good = await service.GetUser(user_good);
            var result_bad = await service.GetUser(user_bad);

            //Assert
            Assert.IsNotNull(result_good, "Failed while takin valid user.");
            Assert.IsNull(result_bad, "Success while taking invalid user");
        }
        #endregion


        [TestMethod()]
        public async Task GetAllUsersTest()
        {
            //Arrange
            userManager = new MockUserManager(new MockUserStore().Object)
                .MockUsers();

            Work.SetManagers(userManager);

            //Act
            UserService service = new UserService(Work.Object);

            var result_good = await service.GetAllUsers();

            //Assert
            Assert.IsNotNull(result_good, "Failed while takin users.");
        }
    }
}