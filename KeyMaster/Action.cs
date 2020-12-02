using AutoIt;
using System;
using System.Threading;

namespace GIC.KeyMaster
{
    public static class Action
    {
        private static readonly object locker = new Object();

        public static bool SendCommand(Command command, bool quickCommand)
        {
            Monitor.Enter(locker);
            try
            {
                //long sec = DateTime.Now.Second;
                int rv = 0;
                try
                {
                    rv = AutoItX.WinWaitActive("*Untitled - Notepad");// GICValues.Instance.Application);
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
                    if (command.activatorType == Command.KEY_DOWN)
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
                            System.Threading.Thread.Sleep(10);
                            AutoItX.Send("{" + command.Key + " up}");
                            //if any modifiers, unset them last
                            foreach (string modifier in command.Modifier)
                            {
                                AutoItX.Send("{" + modifier + "UP}");
                            }
                        }
                    }
                    else if (command.activatorType == Command.KEY_UP && !quickCommand)
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
