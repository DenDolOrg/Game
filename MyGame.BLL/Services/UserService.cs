using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            if(user != null)
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
        public async Task Delete(UserDTO userDTO)
        {
            ApplicationUser user = await Database.UserManager.FindByIdAsync(Int32.Parse(userDTO.Id));
            var logins = user.Logins;
            var rolesForUser = await Database.UserManager.GetRolesAsync(user.Id);
                
            foreach(var login in logins.ToList())
            {
                await Database.UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
            }

            if(rolesForUser.Count() > 0)
            {
                foreach(string roleName in rolesForUser)
                {
                    await Database.UserManager.RemoveFromRoleAsync(user.Id, roleName);
                }
            }

            await Database.PlayerManager.DeleteAsync(user.PlayerProfile);
            await Database.UserManager.DeleteAsync(user);
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
                await Database.PlayerManager.CreateAsync(playerProfile);
                return new OperationDetails(true);

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
            IEnumerable<ApplicationUser> users = await Database.UserManager.Users.ToListAsync();

            return CreateUserDTOs(users);
        }
        #endregion

        #region HELPERS
        private IEnumerable<UserDTO> CreateUserDTOs(IEnumerable<ApplicationUser> users)
        {
            List<UserDTO> userDTOs = new List<UserDTO>();
            foreach (ApplicationUser u in users)
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
