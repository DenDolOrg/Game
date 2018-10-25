using Microsoft.AspNet.Identity;
using Moq;
using MyGame.DAL.Entities;
using MyGame.DAL.Identity;
using MyGame.DAL.Interfaces;
using MyGame.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Tests.MockManagers
{
    internal class MockUnitOfWork : Mock<IUnitOfWork>
    {

        internal void SetManagers(MockUserManager um = null, MockGameManager gm = null, MockTableManager tm = null,
                                   MockFigureManager fm = null, MockRoleManager rm = null, MockPlayerManager pm = null)
        {

            if (um != null)
                Setup(m => m.UserManager).Returns(um.Object);

            if (tm != null)
                Setup(m => m.TableManager).Returns(tm.Object);

            if (fm != null)
                Setup(m => m.FigureManager).Returns(fm.Object);

            if (gm != null)
                Setup(m => m.GameManager).Returns(gm.Object);

            if (pm != null)
                Setup(m => m.PlayerManager).Returns(pm.Object);

            if (rm != null)
                Setup(m => m.RoleManager).Returns(rm.Object);
        }
    }
}
