using GIC.RestApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIC.Windows
{
    class Listener : IListener
    {
        public void KeystrokeReceived(int activator, string key, string[] modifier, bool quickCommand)
        {
            throw new NotImplementedException();
        }

        public void Output(string message)
        {
            throw new NotImplementedException();
        }
    }
}
