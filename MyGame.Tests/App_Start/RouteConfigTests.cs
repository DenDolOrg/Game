using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace MyGame.Tests
{
    [TestClass()]
    public class RouteConfigTests
    {
        
        [TestMethod()]
        public void RoutesHomeTest()
        {
            TestRouteMatch("~/", "Home", "Index");
            TestRouteMatch("~/Some/Index", "Some", "Index");
            TestRouteMatch("~/Home/Index/2", "Home", "Index", new { id  = "2" });

            TestRouteFail("~/Home/Index/Test3/123");
        }

        [TestMethod()]
        public void RoutesGameListTest()
        {
            TestRouteMatch("~/Game/GameList/Some", "Game", "GameList", new { gameType = "Some" });
            TestRouteMatch("~/Game/GameList", "Game", "GameList", new { gameType = "myGames" });

            TestRouteFail("~/Game/GameList/Test3/123");
        }

        [TestMethod()]
        public void RoutesGameTableTest()
        {
            TestRouteMatch("~/Game/EnterGame/2", "Game", "EnterGame", new { gameId = "2" });
            TestRouteMatch("~/Game/EnterGame", "Game", "EnterGame", new { gameId = "0" });

            TestRouteFail("~/Game/EnterGame/Test3/123");
        }


        #region HELPERS
        private HttpContextBase CreateHttpContext(string targetUrl = null, string httpMethod = "GET")
        {
            //Mock-request.
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath)
                .Returns(targetUrl);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);

            //Mock-response
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>()))
                .Returns<string>(s => s);

            //Mock-context
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);

            return mockContext.Object;
        }

        private void TestRouteMatch(string url, string controller, string action, object routeProperties = null, string httpMethod = "GET")
        {
            //Arrange 
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            //Act
            RouteData result = routes.GetRouteData(CreateHttpContext(url, httpMethod));

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestIncomingRouteResult(result, controller, action, routeProperties));

        }

        private bool TestIncomingRouteResult(RouteData routeResult,
                                            string controller, string action, object propertySet = null)
        {
            bool valCompare(object v1, object v2)
            {
                return StringComparer.InvariantCultureIgnoreCase
                .Compare(v1, v2) == 0;
            }
            bool result = valCompare(routeResult.Values["controller"], controller)
            && valCompare(routeResult.Values["action"], action);
            if (propertySet != null)
            {
                PropertyInfo[] propInfo = propertySet.GetType().GetProperties();
                foreach (PropertyInfo pi in propInfo)
                {
                    if (!(routeResult.Values.ContainsKey(pi.Name)
                    && valCompare(routeResult.Values[pi.Name],
                    pi.GetValue(propertySet, null))))
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

        private void TestRouteFail(string url)
        {
            //Arrange
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            //Act
            RouteData result = routes.GetRouteData(CreateHttpContext(url));

            //Assert
            Assert.IsTrue(result == null || result.Route == null);
        }
        #endregion

    }
}