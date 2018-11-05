using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using MyGame.BLL.DTO;
using MyGame.BLL.Interfaces;
using MyGame.Infrastructure;
using Newtonsoft.Json;

namespace MyGame.Real_time
{
    public class StepHub : Hub
    {
        //private IGameService GameService { get; }
        //public StepHub(IGameService gameService)
        //{
        //    GameService = gameService;
        //}
        public void UpdateField(StepModel step)
        {
            var currentUserName = HttpContextManager.Current.User.Identity.Name;
            //var game = GameService.GetGame(new GameDTO { Id = Int32.Parse(step.GameId) }).Result;
            //var opponent = game.Opponents.First(o => o.UserName != currentUserName);
            FigureDTO figureDTO = new FigureDTO
            {
                Id = Int32.Parse(step.FigureId),
                XCoord = Int32.Parse(step.NewXPos),
                YCoord = Int32.Parse(step.NewYPos)
            };
            //var changePosResult = GameService.ChangeFigurePos(figureDTO).Result;

            //if (changePosResult.Succedeed)
                Clients.User("123").changePosition(step);
        }

        //public async Task Join(StepModel step)
        //{
        //    FigureDTO figureDTO = new FigureDTO
        //    {
        //        Id = Int32.Parse(step.FigureId),
        //        XCoord = Int32.Parse(step.NewXPos),
        //        YCoord = Int32.Parse(step.NewYPos)
        //    };
        //    var changePosResult = await GameService.ChangeFigurePos(figureDTO);

        //    if (changePosResult.Succedeed)
        //        Clients.User(step.WhoToSend).changePosition(step);
        //}
    }

    public class StepModel
    {
        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("figureId")]
        public string FigureId { get; set; }

        [JsonProperty("newX")]
        public string NewXPos { get; set; }

        [JsonProperty("newY")]
        public string NewYPos { get; set; }
    }
}