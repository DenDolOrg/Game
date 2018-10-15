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

        internal  List<UserDTO> Users = new List<UserDTO>
            {
                new UserDTO{ Id = 1, Email = "email_1@gmail.com", Password = "111111", UserName = "username_1", Name = "name_1", Surname = "Surname_1"},
                new UserDTO{ Id = 2, Email = "email_2@gmail.com", Password = "222222", UserName = "username_2", Name = "name_2", Surname = "Surname_2"},
                new UserDTO{ Id = 3, Email = "email_3@gmail.com", Password = "333333", UserName = "username_3", Name = "name_3", Surname = "Surname_3"},
                new UserDTO{ Id = 4, Email = "email_4@gmail.com", Password = "444444", UserName = "username_4", Name = "name_4", Surname = "Surname_4"},
                new UserDTO{ Id = 5, Email = "email_5@gmail.com", Password = "555555", UserName = "username_5", Name = "name_5", Surname = "Surname_5"}
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
            List<UserTestModel> models = new List<UserTestModel>(UserModels);

            Setup(m => m.DeteteUserTables(
                It.IsAny<UserDTO>()
                )).ReturnsAsync(new OperationDetails(false));

            Setup(m => m.DeteteUserTables(
                It.Is<UserDTO>(u => ((models.FirstOrDefault(um => um.UserDTO.Id == u.Id) != null) &&
                                       ClearAndCheck(models.FirstOrDefault(um => um.UserDTO.Id == u.Id))))
                )).ReturnsAsync(new OperationDetails(true));
            return this;
        }

        internal MockTableService MockGetUserTables(UserTestModel newUser)
        {
            UserModels.Add(newUser);

            Setup(m => m.GetUserTables(
                It.IsAny<UserDTO>()
                )).ReturnsAsync((ICollection<TableDTO>)null);

            Setup(m => m.GetUserTables(
                It.Is<UserDTO>(u => UserModels.FirstOrDefault(um => um.UserDTO.UserName == u.UserName) != null)
                )).ReturnsAsync((UserDTO user) => UserModels.Find(um => um.UserDTO.UserName == user.UserName).Tables);

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
                It.Is<UserDTO>(u => UserModels.FirstOrDefault(um => um.UserDTO.UserName == u.UserName) != null)
                )).ReturnsAsync((UserDTO u) => Tables.Where(t => (t.Opponents.Count == 1) &&
                                                            t.Opponents.Where(o => o.UserName == u.UserName).Count() == 0));
            return this;
        }

        internal MockTableService MockCreateTable()
        {
            Setup(m => m.CreateNewTable(
                It.IsAny<UserDTO>()
                )).ReturnsAsync(new OperationDetails(false));

            Setup(m => m.CreateNewTable(
                It.Is<UserDTO>(u => UserModels.FirstOrDefault(um => um.UserDTO.UserName == u.UserName) != null)
                )).ReturnsAsync(new OperationDetails(true)).Callback((UserDTO u) => UserModels.Find(um => u.UserName == um.UserDTO.UserName)
                                                            .Tables.Add(new TableDTO()));
            return this;
        }

        internal MockTableService MockDeleteTable()
        {
            Setup(m => m.DeteteTable(
                It.IsAny<TableDTO>()
                )).ReturnsAsync(new OperationDetails(false));

            Setup(m => m.DeteteTable(
                It.Is<TableDTO>(t => Tables.FirstOrDefault(tl => tl.Id == t.Id) != null)
                )).ReturnsAsync(new OperationDetails(true)).Callback((TableDTO t) => Tables.Remove(Tables.Find(tl => tl.Id == t.Id)));
            return this;
        }
        
        #region HELPERS
        private void CreateTables()
        {
            for(int i = 1; i <= Users.Count * 3; i++)
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
                for (int j = 0; j < 3; j++)
                {
                    Tables[j + i * 3].Opponents.Add(Users.ElementAt(i));
                    userTables.Add(Tables[j + i * 3]);
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
