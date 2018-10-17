using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using System.Threading.Tasks;
using MyGame.BLL.Interfaces;
using MyGame.BLL.DTO;
using System.Security.Claims;
using MyGame.Models;
using MyGame.BLL.Infrastructure;
using MyGame.Tests.Models;

namespace MyGame.Tests.Services
{
    internal class MockTableService : Mock<ITableService>
    {
        internal int _tablePerUserNum = 2;

        internal  List<UserDTO> Users = new List<UserDTO>
            {
                new UserDTO{ Id = 1, Email = "email_1@gmail.com", Password = "111111", UserName = "username_1", Name = "name_1", Surname = "Surname_1"},
                new UserDTO{ Id = 2, Email = "email_2@gmail.com", Password = "222222", UserName = "username_2", Name = "name_2", Surname = "Surname_2"},
            };

        internal List<TableDTO> Tables { get; set; }

        internal List<UserTestModel> UserModels{ get; set; }
        

        internal MockTableService()
        {
            Tables = new List<TableDTO>();
            UserModels = new List<UserTestModel>();
            CreateTables();
            FillUserModels();     
        }

        internal MockTableService MockDeleteUserTables()
        {
            Setup(m => m.DeteteUserTables(
                It.IsAny<UserDTO>()
                )).ReturnsAsync(new OperationDetails(false));

            Setup(m => m.DeteteUserTables(
                It.Is<UserDTO>(u => (from um in UserModels
                                    where um.UserDTO.Id == u.Id
                                    select um).Count() == 1)))
                                    .ReturnsAsync(new OperationDetails(true));
            return this;
        }

        internal MockTableService MockGetUserTables(UserTestModel newUser)
        {
            UserModels.Add(newUser);

            Setup(m => m.GetUserTables(
                It.IsAny<UserDTO>()
                )).ReturnsAsync((ICollection<TableDTO>)null);

            Setup(m => m.GetUserTables(
                It.Is<UserDTO>(u => (from um in UserModels
                                     where um.UserDTO.UserName == u.UserName
                                     select um).Count() == 1)))
                                     .ReturnsAsync((UserDTO u) => (from um in UserModels
                                                                   where um.UserDTO.UserName == u.UserName
                                                                   select um.Tables).First());
            return this;
        }

        internal MockTableService MockGetAllTables()
        {
            Setup(m => m.GetAllTables())
                .ReturnsAsync(Tables);
            return this;
        }

        internal MockTableService MockGetAvailableTables()
        {
            Setup(m => m.GetAvailableTables(
                It.IsAny<UserDTO>()
                )).ReturnsAsync((List<TableDTO>)null);

            Setup(m => m.GetAvailableTables(
                It.Is<UserDTO>(u => (from um in UserModels
                                     where um.UserDTO.UserName == u.UserName
                                     select um).Count() == 1)))
                                     .ReturnsAsync((UserDTO u) => (from t in Tables
                                                                   where t.Opponents.Count == 1 &&
                                                                          (from o in t.Opponents
                                                                          where o.UserName == u.UserName
                                                                          select o).Count() == 0
                                                                   select t));
            return this;
        }

        internal MockTableService MockCreateTable()
        {
            Setup(m => m.CreateNewTable(
                It.IsAny<UserDTO>()
                )).ReturnsAsync(new OperationDetails(false));

            Setup(m => m.CreateNewTable(
                It.Is<UserDTO>(u => (from um in UserModels
                                     where um.UserDTO.UserName == u.UserName
                                     select um).Count() == 1)))
                                     .ReturnsAsync(new OperationDetails(true));
            return this;
        }

        internal MockTableService MockDeleteTable()
        {
            Setup(m => m.DeteteTable(
                It.IsAny<TableDTO>()
                )).ReturnsAsync(new OperationDetails(false));

            Setup(m => m.DeteteTable(
                It.Is<TableDTO>(t => (from tl in Tables
                                      where tl.Id == t.Id
                                      select tl).Count() == 1)))
                                      .ReturnsAsync(new OperationDetails(true));
            return this;
        }
        
        #region HELPERS
        private void CreateTables()
        {
            for(int i = 1; i <= Users.Count * _tablePerUserNum; i++)
            {
                Tables.Add(new TableDTO
                {
                    Id = i,
                    Opponents = new List<UserDTO>()
                });
            }
        }

        private bool ClearAndCheck(UserTestModel model)
        {
            if (model.Tables.Count == 0)
                return false;

            model.Tables.Clear();
 
            if (model.Tables.Count == 0)
                return true;

            return false;
        }
        private void FillUserModels()
        {
            for (int i = 0; i < Users.Count; i++)
            {
                List<TableDTO> userTables = new List<TableDTO>();
                for (int j = 0; j < _tablePerUserNum; j++)
                {
                    Tables[j + i * _tablePerUserNum].Opponents.Add(Users.ElementAt(i));
                    userTables.Add(Tables[j + i * _tablePerUserNum]);
                }
                UserModels.Add(
                    new UserTestModel
                    {
                        UserDTO = Users.ElementAt(i),
                        Tables = userTables
                    }
                );
            }
        }
        #endregion
    }
}
