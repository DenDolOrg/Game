using System;
using Microsoft.AspNet.Identity.EntityFramework;
using MyGame.DAL.Entities;
using MyGame.DAL.EntityFramework;
/// <summary>
/// Custom user role with <see cref="Int32"/> key.
/// </summary>
/// <seealso cref="IdentityRole"/>
public class UserRole : IdentityUserRole<int>
{
}

/// <summary>
/// Custom user claim with <see cref="Int32"/> key.
/// </summary>
/// <seealso cref="IdentityUserClaim"/>
public class UserClaim : IdentityUserClaim<int>
{
}

/// <summary>
/// Custom user login with <see cref="Int32"/> key.
/// </summary>
/// <seealso cref="IdentityUserLogin"/>
public class UserLogin : IdentityUserLogin<int>
{
}

/// <summary>
/// Custom user stote with <see cref="ApplicationUser"/> as user, <see cref="ApplicationRole"/> as role defifnition, 
/// <see cref="Int32"/> key, <see cref="UserLogin"/> login, <see cref="UserRole"/> as users role, <see cref="UserClaim"/> as users claim.
/// </summary>
public class UserStore : UserStore<ApplicationUser, ApplicationRole, int, UserLogin, UserRole, UserClaim>
{
    /// <summary>
    /// Initializes a new instance of <see cref="UserStore"/>.
    /// </summary>
    /// <param name="context">Database context.</param>
    public UserStore(ApplicationContext context) : base(context)
    {

    }
}

/// <summary>
/// Custom role store with <see cref="ApplicationRole"/> as role defitition, <see cref="Int32"/> as key, <see cref="UserRole"/> as users role.
/// </summary>
public class RoleStore : RoleStore<ApplicationRole, int, UserRole>
{
    /// <summary>
    /// Initializes a new instance of<see cref= "RoleStore" />.
    /// </summary>
    /// <param name="context">Database context.</param>
    public RoleStore(ApplicationContext context) : base(context)
    {

    }
}