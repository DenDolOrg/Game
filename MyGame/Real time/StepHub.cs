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
        public string FigureIdToDelete { get; set; }

        [JsonProperty("newX")]
        public string NewXPos { get; set; }

        [JsonProperty("newY")]
        public string NewYPos { get; set; }
    }

    public class JoinSignalModel
    {
        [JsonProperty("receiverName")]
        public string ReceiverName { get; set; }

        [JsonProperty("myName")]
        public string MyName { get; set; }
    }

}