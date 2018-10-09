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
                Database.PlayerManager.Delete(user.PlayerProfile);
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
        public async Task<OperationDetails> Create(UserDTO userDTO)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDTO.Email);
            if (user == null)
            {
                user = await Database.UserManager.FindByNameAsync(userDTO.UserName);
                if(user != null)
                {
                    return new OperationDetails(false, "Account with such Nickname already exist.", "Nickname");
                }
                user = new ApplicationUser { Email = userDTO.Email, UserName = userDTO.UserName };
                var result = await Database.UserManager.CreateAsync(user, userDTO.Password);

                await Database.UserManager.AddToRoleAsync(user.Id, userDTO.Role);

                PlayerProfile playerProfile = new PlayerProfile { Id = user.Id, Surname = userDTO.Surname, Name = userDTO.Name };
                Database.PlayerManager.Create(playerProfile);
                await Database.SaveAsync();
                return new OperationDetails(true, "Registration completed successfully.", "");

            }
            else
            {
                return new OperationDetails(false, "Account with such email already exist.", "Email");
            }
        }
        #endregion

        #region SET_INITIAL_DATA
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

        #region GET_USER
        public async Task<UserDTO> GetUser(UserDTO userDTO)
        {
            UserDTO userDtoToSend = null;
            ApplicationUser user = await Database.UserManager.FindByNameAsync(userDTO.UserName);

            if (user != null)
            {
                userDtoToSend = new UserDTO
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    Name = user.PlayerProfile.Name,
                    Surname = user.PlayerProfile.Surname,
                    UserName = user.UserName
                };
            }
            return userDtoToSend;
        }
        #endregion

        #region GET_ALL_USERS
        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            IEnumerable<ApplicationUser> users = await Task<IEnumerable<ApplicationUser>>.Factory.StartNew(() => Database.UserManager.Users);

            List<UserDTO> userDTOs = new List<UserDTO>();
            foreach(ApplicationUser u in users)
            {
                userDTOs.Add(new UserDTO
                {
                    Id = u.Id.ToString(),
                    Name = u.PlayerProfile.Name,
                    Surname = u.PlayerProfile.Surname,
                    Email = u.Email,
                    UserName = u.UserName
                });
            }

            return userDTOs.AsEnumerable();
        }
        #endregion

        public void Dispose()
        {
            Database.Dispose();
        }


    }
}
