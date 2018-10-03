using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MyGame.BLL.DTO;
using MyGame.BLL.Infrastructure;
using MyGame.BLL.Interfaces;
using MyGame.DAL.Entities;
using MyGame.DAL.Interfaces;
namespace MyGame.BLL.Services
{
    /// <summary>
    /// Service class which contains communication methods between DAL and UI levels. 
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Instance of <see cref="IUnitOfWork"/> which contains managers of entities and DB context;
        /// </summary>
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="UserService"/>.
        /// </summary>
        /// <param name="db">Instance of <see cref="IUnitOfWork"/></param>
        public UserService(IUnitOfWork db)
        {
            Database = db;
        }

        #region AUTHENTICATION
        /// <summary>
        /// User authentication method.
        /// </summary>
        /// <param name="userDTO">Universal user data model. Contains user email and password.
        /// <seealso cref="UserDTO"/>
        /// </param>
        /// <returns>Returns claim of authenticated user.</returns>
        public async Task<ClaimsIdentity> Authenticate(UserDTO userDTO)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDTO.Email);
            if(user == null)
            {
                user = await Database.UserManager.FindAsync(userDTO.Email, userDTO.Password);
                if(user != null)
                    claim = await Database.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }
            else
            {
                if(await Database.UserManager.CheckPasswordAsync(user, userDTO.Password))
                {
                    claim = await Database.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                }
            }
            return claim;

        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete user method.
        /// </summary>
        /// <param name="userDTO">Universal user data model. Contains user email.
        /// <seealso cref="UserDTO"/>
        /// </param>
        /// <returns>Returns detetion operation status like <see cref="OperatingSystem"/> instance.</returns>
        public async Task<OperationDetails> Delete(UserDTO userDTO)
        {
            var user = await Database.UserManager.FindByEmailAsync(userDTO.Email);
            if (user != null)
            {
                var logins = user.Logins;
                var rolesForUser = await Database.UserManager.GetRolesAsync(user.Id);
                
                foreach(var login in logins.ToList())
                {
                    await Database.UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }

                if(rolesForUser.Count() > 0)
                {
                    foreach(string roleName in rolesForUser.ToList())
                    {
                        var result = await Database.UserManager.RemoveFromRoleAsync(user.Id, roleName);
                    }
                }
                await Database.PlayerManager.DeleteAsync(user.PlayerProfile);
                await Database.UserManager.DeleteAsync(user);
                return new OperationDetails(true, "Account" + userDTO.Email + "succesfuly deleted", "");
            }
            else
            {
                return new OperationDetails(false, "No account wuth such Email", "Email");
            }
        }
        #endregion

        #region CREATE
        /// <summary>
        /// User creation method.
        /// </summary>
        /// <param name="userDTO">Universal user data model. Contains user name, surname, nickname, email and password. 
        /// <seealso cref="UserDTO"/>
        /// </param>
        /// <returns>Returns user creation operation status like <see cref="OperatingSystem"/> instance.</returns>
        public async Task<OperationDetails> Create(UserDTO userDTO)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDTO.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDTO.Email, UserName = userDTO.UserName };
                await Database.UserManager.CreateAsync(user, userDTO.Password);

                await Database.UserManager.AddToRoleAsync(user.Id, userDTO.Role);

                PlayerProfile playerProfile = new PlayerProfile { Id = user.Id, Surname = userDTO.Surname, Name = userDTO.Name };
                await Database.PlayerManager.CreateAsync(playerProfile);
                await Database.SaveAsync();
                return new OperationDetails(true, "Registration completed successfully.", "");

            }
            else
            {
                return new OperationDetails(false, "Account with such login already exist.", "Email");
            }
        }
        #endregion

        #region SET_INITIAL_DATA
        /// <summary>
        /// Sets default roles and admin account.
        /// </summary>
        /// <param name="adminDTO">Universal user data model. Contains admins email.
        /// <seealso cref="UserDTO"/>
        /// </param>
        /// <param name="roles">List of roles to add.</param>
        public async Task SetInitialData(UserDTO adminDTO, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            var adminAccount = await Database.UserManager.FindByEmailAsync(adminDTO.Email);
            if(adminAccount != null && !adminAccount.Roles.Contains(new UserRole
            {
                UserId = adminAccount.Id,
                RoleId = Database.RoleManager.Roles.FirstOrDefault(r => r.Name == "admin").Id
            }))
            {
                var result = await Database.UserManager.AddToRoleAsync(adminAccount.Id, "admin");
            }
        }
        #endregion

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
