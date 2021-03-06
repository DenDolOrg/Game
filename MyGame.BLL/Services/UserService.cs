﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
            var user = await Database.UserManager.FindByEmailAsync(userDTO.Email);
            if(user != null)
            {
                if(await Database.UserManager.CheckPasswordAsync(user, userDTO.Password))
                {
                    return await Database.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                }
            }        
            return null;
        }
        #endregion

        #region DELETE
        public async Task<OperationDetails> Delete(UserDTO userDTO)
        {
            OperationDetails successOD = new OperationDetails(true);
            OperationDetails failOD = new OperationDetails(false);

            ApplicationUser user = await Database.UserManager.FindByIdAsync(userDTO.Id);
            if (user == null)
                return failOD;
            var logins = user.Logins;
            var rolesForUser = await Database.UserManager.GetRolesAsync(user.Id);
                
            foreach(var login in logins.ToList())
            {
                var LoginDelResult = await Database.UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                if (!LoginDelResult.Succeeded)
                    return failOD;
            }

            if(rolesForUser.Count() > 0)
            {
                foreach(string roleName in rolesForUser)
                {
                    var RoleDelResult = await Database.UserManager.RemoveFromRoleAsync(user.Id, roleName);
                    if (!RoleDelResult.Succeeded)
                        return failOD;
                }
            }

            bool PlayerDelResult = await Database.PlayerManager.DeleteAsync(user.PlayerProfile);
            var UserDelResult = await Database.UserManager.DeleteAsync(user);

            if (!(PlayerDelResult && UserDelResult.Succeeded))
                return failOD;

            return successOD;

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
                    return new OperationDetails(false, "Account with such Nickname already exist.", "Nickname");

                user = new ApplicationUser { Email = userDTO.Email, UserName = userDTO.UserName };

                var createUserRes = await Database.UserManager.CreateAsync(user, userDTO.Password);
                if (!createUserRes.Succeeded)
                    throw new HttpException(500, "Unexpected error.");

                var addToRoleRes = await Database.UserManager.AddToRoleAsync(user.Id, userDTO.Role);
                if(!addToRoleRes.Succeeded)
                    throw new HttpException(500, "Unexpected error.");

                var playerProfile = new PlayerProfile { Id = user.Id, Surname = userDTO.Surname, Name = userDTO.Name };

                if(!(await Database.PlayerManager.CreateAsync(playerProfile)))
                    throw new HttpException(500, "Unexpected error.");

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

                    var createRoleRes = await Database.RoleManager.CreateAsync(role);
                    if(!createRoleRes.Succeeded)
                        throw new HttpException(500, "Unexpected error.");
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
            var user = await Database.UserManager.FindByNameAsync(userDTO.UserName);

            if (user != null)
            {
                var userDtoToSend = new UserDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.PlayerProfile.Name,
                    Surname = user.PlayerProfile.Surname,
                    UserName = user.UserName
                };
                return userDtoToSend;
            }
            return null;
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
                    Id = u.Id,
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
