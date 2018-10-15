using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using System.Security.Claims;

namespace MyGame.Tests.Services
{
    internal class MockAuthenticationManager : Mock<IAuthenticationManager>
    {
        internal MockAuthenticationManager MockSignOut()
        {
            Setup(m => m.SignOut());
            return this;
        }

        internal MockAuthenticationManager MockSignIn()
        {
            Setup(m => m.SignIn(
                It.Is<AuthenticationProperties>(a => a.IsPersistent == true),
                It.Is<ClaimsIdentity>(c => c != null)));
            return this;
        }
    }
}
