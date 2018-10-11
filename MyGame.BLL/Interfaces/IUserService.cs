using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using MyGame.BLL.Infrastructure;
using MyGame.BLL.DTO;
namespace MyGame.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {

        /// <summary>
        /// User creation method.
        /// </summary>
        /// <param name="userDTO">Universal user data model. Contains user name, surname, nickname, email and password. 
        /// <seealso cref="UserDTO"/>
        /// </param>
        /// <returns>Returns user creation operation status like <see cref="OperatingSystem"/> instance.</returns>
        Task<OperationDetails> Create(UserDTO userDTO);

        /// <summary>
        /// Delete user method.
        /// </summary>
        /// <param name="userDTO">Universal user data model. Contains user email.
        /// <seealso cref="UserDTO"/>
        /// </param>
        /// <returns>Returns detetion operation status like <see cref="OperatingSystem"/> instance.</returns>
        Task Delete(UserDTO userDTO);

        /// <summary>
        /// User authentication method.
        /// </summary>
        /// <param name="userDTO">Universal user data model. Contains user email and password.
        /// <seealso cref="UserDTO"/>
        /// </param>
        /// <returns>Returns claim of authenticated user.</returns>
        Task<ClaimsIdentity> Authenticate(UserDTO userDTO);

        /// <summary>
        /// Sets default roles and admin account.
        /// </summary>
        /// <param name="adminDTO">Universal user data model. Contains admins email.
        /// <seealso cref="UserDTO"/>
        /// </param>
        /// <param name="roles">List of roles to add.</param>
        Task SetInitialData(UserDTO adminDTO, List<String> roles);

        /// <summary>
        /// Returns full user information by his nickname.
        /// </summary>
        /// <param name="userDTO">Universal user data model. Contains user's nickname.</param>
        /// <returns>Universal user data model. Contains full user information except password and role.</returns>
        Task<UserDTO> GetUser(UserDTO userDTO);

        /// <summary>
        /// Returns all users information from DB.
        /// </summary>
        /// <returns>List of universal user data model.</returns>
        Task<IEnumerable<UserDTO>> GetAllUsers();
    }
}
