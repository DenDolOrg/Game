using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyGame.Tests.MockHelpers
{
    internal class MockHttpSession : HttpSessionStateBase
    {
        readonly Dictionary<string, object> sessionDictionary = new Dictionary<string, object>();

        public override object this[string name]
        {
            get { return sessionDictionary[name]; }
            set { sessionDictionary[name] = value; }
        }
    }
}
