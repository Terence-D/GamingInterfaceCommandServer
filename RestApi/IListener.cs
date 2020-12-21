using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIC.RestApi
{
    public interface IListener
    {
        void KeystrokeReceived(int activator, string key, string[] modifier, bool quickCommand);
        void Output(string message);
    }
}
