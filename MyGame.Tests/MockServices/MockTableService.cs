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

        private  List<UserDTO> users = new List<UserDTO>
            {
                new UserDTO{ Id = 1, Email = "email_1@gmail.com", Password = "111111", UserName = "username_1", Name = "name_1", Surname = "Surname_1"},
                new UserDTO{ Id = 2, Email = "email_2@gmail.com", Password = "222222", UserName = "username_2", Name = "name_2", Surname = "Surname_2"},
                new UserDTO{ Id = 3, Email = "email_3@gmail.com", Password = "333333", UserName = "username_3", Name = "name_3", Surname = "Surname_3"},
                new UserDTO{ Id = 4, Email = "email_4@gmail.com", Password = "444444", UserName = "username_4", Name = "name_4", Surname = "Surname_4"}
            };

        private List<TableDTO> tables = new List<TableDTO>();

        private List<UserTestModel> userModels = new List<UserTestModel>();
        

        internal MockTableService()
        {
            CreateTables();
            FillUserModels();     
        }

        internal MockTableService MockDeleteUserTables()
        {
            List<UserTestModel> models = new List<UserTestModel>(userModels);

            Setup(m => m.DeteteUserTables(
                It.Is<UserDTO>(u => ((models.FirstOrDefault(um => um.UserDTO.Id == u.Id) != null) &&
                                       ClearAndCheck(models.FirstOrDefault(um => um.UserDTO.Id == u.Id))))
                )).ReturnsAsync(new OperationDetails(true));

            Setup(m => m.DeteteUserTables(
                It.Is<UserDTO>(u => models.FirstOrDefault(um => um.UserDTO.Id == u.Id) == null)
                )).ReturnsAsync(new OperationDetails(false));
            return this;
        }


        #region HELPERS
        private void CreateTables()
        {
            for(int i = 1; i <= users.Count * 3; i++)
            {
                tables.Add(new TableDTO
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
            for (int i = 0; i < users.Count; i++)
            {
                List<TableDTO> userTables = new List<TableDTO>();
                for (int j = 0; j < 3; j++)
                {
                    tables[j + i * 3].Opponents.Append(users.ElementAt(i));
                    userTables.Add(tables[j + i * 3]);
                }
                userModels.Add(
                    new UserTestModel
                    {
                        UserDTO = users.ElementAt(i),
                        Tables = userTables
                    }
                );
            }
        }
        #endregion
    }
}
