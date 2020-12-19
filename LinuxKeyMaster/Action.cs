using System;
using System.Threading;

namespace GIC.KeyMaster
{
    public static class Action
    {
        public static string Application { get; set; }
        private static readonly object locker = new object();

        public static bool SendCommand(int activator, string key, string[] modifier, bool quickCommand)
        {
            Command command = new Command()
            {
                ActivatorType = activator,
                Key = key,
                Modifier = modifier
            };
            Monitor.Enter(locker);
            try
            {

                string toExecute = $"xdotool windowactivate --sync \"$(xdotool search--class ${Application} | head -1)\" key ${command.Key}";

                ShellHelper.Bash(toExecute);
                return true;
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                Monitor.Exit(locker);
            }
        }
    }
}
