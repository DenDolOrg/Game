using Microsoft.AspNet.Identity;
using Moq;
using MyGame.DAL.Entities;
using MyGame.DAL.Identity;
using MyGame.DAL.Interfaces;
using MyGame.Tests.MockEnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Tests.MockManagers
{
    internal class MockUnitOfWork : Mock<IUnitOfWork>
    {
        public int _tablePerUserNum = 2;
        public int _userNum = 2;

        private List<MockApplicationUser> Users;
        private List<MockFigure> Figures;
        private List<MockTable> Tables;

        private MockTableManager _tm;
        private MockUserManager _um;
        private MockFigureManager _fm;
        private MockPlayerManager _pm;

        internal void  SetManagers(MockUserManager um = null, MockTableManager tm = null,
                                   MockFigureManager fm = null, MockPlayerManager pm = null)
        {

            Users = new List<MockApplicationUser>();
            Figures = new List<MockFigure>();
            Tables = new List<MockTable>();

            for (int i = 0; i < _userNum; i++)
                Users.Add(CreateUser(i + 1));

            if(um != null)
            { _um = um; Setup(m => m.UserManager).Returns(_um.Object); }

            if (fm != null)
            {_fm = fm; Setup(m => m.FigureManager).Returns(_fm.Object); }

            if (tm != null)
            { _tm = tm; Setup(m => m.TableManager).Returns(_tm.Object); }

            if (pm != null)
            { _pm = pm; Setup(m => m.PlayerManager).Returns(_pm.Object); }           

            AddDataToManagers();
        }

        private void AddDataToManagers()
        {
            if (_um != null)
                _um.AddData(ref Users);

            if (_fm != null)
                _fm.AddData(ref Users, ref Tables, ref Figures);

            if (_pm != null)
                _pm.AddData(ref Users);

            if (_tm != null)
                _tm.AddData(ref Users, ref Tables);
        }

        internal MockUserStore GetUserStore()
        {
            return new MockUserStore();
        }
        internal MockUnitOfWork MockSaveChangesAsync()
        { 
            Setup(m => m.SaveChangesAsync());
            return this;
        }
  

        #region HELPERS


        private MockApplicationUser CreateUser(int id)
        {

            List<MockTable> tables = new List<MockTable>();
            for(int i = 1; i <= _tablePerUserNum; i++)
            {
                tables.Add(new MockTable { Id = (id - 1) * _tablePerUserNum + i, CreationTime = DateTime.Now }); 
            }

            MockApplicationUser newUser = new MockApplicationUser
            {
                Id = id,
                Email = "email_" + id + "@gmail.com",
                PlayerProfile = new MockPlayerProfile { Name = "name_" + id, Surname = "surname_" + id },
                UserName = "username_" + id,
                Tables = tables
            };

            newUser.PlayerProfile.ApplicationUser = newUser;
            newUser.PlayerProfile.SetupPlayerProfile();

            for (int i = 0; i < newUser.Tables.Count; i++)
            {
                Figures.AddRange(SetFigures(newUser.Tables.ElementAt(i)));
                newUser.Tables.ElementAt(i).Opponents.Add(newUser);
                newUser.Tables.ElementAt(i).SetupTable();
                Tables.Add(newUser.Tables.ElementAt(i));
            }
            newUser.SetupApplicationUser();
            return newUser;
        }

        internal void AddUserToTable(MockApplicationUser user)
        {
            Users.Add(user);
            if (user.Tables != null)
            {
                Tables.AddRange(user.Tables);
                foreach(var t in user.Tables)
                {
                    t.SetupTable();
                    Users.AddRange(t.Opponents);
                }
            }               
        }
        internal static ICollection<MockFigure> SetFigures(MockTable table)
        {
            List<MockFigure> figures = new List<MockFigure>();
            Colors color = Colors.Black;
            int y0 = 1;
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 20; i++)
                {
                    int xCoord;
                    int yCoord = i / 5;

                    if (yCoord % 2 == 1)
                        xCoord = 2 * (i - yCoord * 5) + 1;
                    else
                        xCoord = 2 * (i - yCoord * 5 + 1);

                    figures.Add(new MockFigure
                    {
                        Id = figures.Count + 1,
                        Color = color,
                        Table = table,
                        X = xCoord,
                        Y = yCoord + y0
                    }.SetupFigure());
                }
                y0 = 7;
                color = Colors.White;
            }

            return figures;
        }
        #endregion
    }
}
