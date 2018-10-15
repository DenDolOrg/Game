using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.BLL.Infrastructure
{
    public class OperationDetails
    {
        public bool Succedeed { get; private set; }
        public string ErrorMessage { get; private set; }
        public string PropErrorName { get; private set; }
        public OperationDetails(bool succedeed, string errorMessage = "", string propErrorName = "")
        {
            Succedeed = succedeed;
            ErrorMessage = errorMessage;
            PropErrorName = propErrorName;
        }
    }
}
