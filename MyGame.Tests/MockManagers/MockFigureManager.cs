﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MyGame.DAL.Entities;
using MyGame.DAL.Interfaces;
using MyGame.Tests.MockEnity;

namespace MyGame.Tests.MockManagers
{
    internal class MockFigureManager : Mock<IFigureManager>
    {
        internal List<MockApplicationUser> Users { get; set; }
        internal List<MockTable> Tables { get; set; }


        internal List<MockFigure> Figures { get; set; }

        internal MockFigureManager MockCreateAsync()
        {
            Setup(m => m.CreateAsync(
                It.IsAny<int>()
                )).ReturnsAsync(false);

            Setup(m => m.CreateAsync(
                It.Is<int>(id => (from fl in Figures
                                  where fl.Table.Id == id
                                  select fl).Count() == 0)))
                                  .ReturnsAsync(true);
            return this;
        }

        internal MockFigureManager MockDeleteAsync()
        {
            Setup(m => m.DeleteAsync(
                It.IsAny<int>()
                )).ReturnsAsync(false);

            Setup(m => m.DeleteAsync(
                It.Is<int>(id => (from t in Tables
                                  where t.Id == id
                                  select t).Count() == 1 
                                  &&
                                  (from f in Figures
                                  where f.Table.Id == id
                                  select f).Count() > 0)
                )).ReturnsAsync(true);
            return this;
        }





        internal void AddData(ref List<MockApplicationUser> users, ref List<MockTable> tables, ref List<MockFigure> figures)
        {
            Users = users;
            Figures = figures;
            Tables = tables;
        }

    }
}
