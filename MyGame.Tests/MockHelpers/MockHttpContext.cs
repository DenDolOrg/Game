using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyGame.Tests.MockHelpers
{
    internal class MockHttpContext
    {
        internal HttpContextBase CustomHttpContextBase { get; set; }
        internal MockHttpContext(string userName)
        {
            var context = new Mock<HttpContextBase>();
            var user = new Mock<IPrincipal>();
            var identity = new Mock<IIdentity>();

            var session = new MockHttpSession();

            context.Setup(c => c.Session).Returns(session);
            context.Setup(c => c.User).Returns(user.Object);
            user.Setup(u => u.Identity).Returns(identity.Object);
            identity.Setup(i => i.Name).Returns(userName);

            CustomHttpContextBase = context.Object;
        }

    }
}
