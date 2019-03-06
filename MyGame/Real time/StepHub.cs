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
        public void UpdateField(StepModel step)
        {
            Clients.User(step.ReceiverName).changeField(step);
        }

        public void JoinSignal(JoinSignalModel joinModel)
        {
            Clients.User(joinModel.ReceiverName).reciveJoinSignal(joinModel);
        }

        public void EndGame(EndGameModel endGameModel)
        {
            Clients.User(endGameModel.ReceiverName).reciveEndOfGame();
        }
    }

    public class StepModel
    {
        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("receiverName")]
        public string ReceiverName { get; set; }

        [JsonProperty("figureId")]
        public string FigureId { get; set; }

        [JsonProperty("figureDelId")]
        public string FigureIdsToDelete { get; set; }

        [JsonProperty("coordsToMove")]
        public string CoordsToMove { get; set; }

        [JsonProperty("superFigStatus")]
        public string NewSuperFigureStatus { get; set; }

    }

    public class JoinSignalModel
    {
        [JsonProperty("receiverName")]
        public string ReceiverName { get; set; }

        [JsonProperty("myName")]
        public string MyName { get; set; }
    }

    public class EndGameModel
    {
        [JsonProperty("receiverName")]
        public string ReceiverName { get; set; }

    }

}