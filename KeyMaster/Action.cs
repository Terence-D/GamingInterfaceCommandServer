using AutoIt;
using System;
using System.Threading;

namespace GIC.KeyMaster
{
    public static class Action
    {
        private static readonly object locker = new object();
        public static string Application { get; set; }

        public static bool SendCommand(int activator, string key, string[] modifier, bool quickCommand)
        {
            Command command = new Command()
            {
                ActivatorType = activator,
                Key = key,
                Modifier = modifier
            };
            return WindowsAction(command, quickCommand);
        }

        private static bool WindowsAction(Command command, bool quickCommand)
        {
            Monitor.Enter(locker);
            try
            {
                //long sec = DateTime.Now.Second;
                int rv = 0;
                try
                {
                    rv = AutoItX.WinWaitActive(Application);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //MessageBox.Show(e.Message);
                    return false;
                }
                if (rv == 0)
                {
                    return false;
                }
                else
                {
                    if (command.ActivatorType == Command.KEY_DOWN)
                    {
                        //if any modifiers, send them first
                        foreach (string modifier in command.Modifier)
                        {
                            AutoItX.Send("{" + modifier + "DOWN}");
                        }
                        //now send the key itself
                        AutoItX.Send("{" + command.Key + " down}");
                        if (quickCommand)
                        {
                            //keep everything pressed for 10ms
                            Thread.Sleep(10);
                            AutoItX.Send("{" + command.Key + " up}");
                            //if any modifiers, unset them last
                            foreach (string modifier in command.Modifier)
                            {
                                AutoItX.Send("{" + modifier + "UP}");
                            }
                        }
                    }
                    else if (command.ActivatorType == Command.KEY_UP && !quickCommand)
                    {
                        AutoItX.Send("{" + command.Key + " up}");
                        //if any modifiers, unset them last
                        foreach (string modifier in command.Modifier)
                        {
                            AutoItX.Send("{" + modifier + "UP}");
                        }
                    }
                    //Console.WriteLine("ending command for " + command.Key + " " + sec);
                }
                return true;
            }
            catch (Exception e)
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
