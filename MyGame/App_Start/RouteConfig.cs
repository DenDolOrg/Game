using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyGame
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "GameTable",
                url: "{controller}/{action}/{gameId}",
                defaults: new {gameId = "0" },
                constraints: new { controller = "Game", action = "EnterGame" }
                );

            routes.MapRoute(
                name: "GameList",
                url: "{controller}/{action}/{gameType}",
                defaults: new { gameType = "myGames" },
                constraints: new { controller = "Game", action = "GameList" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
