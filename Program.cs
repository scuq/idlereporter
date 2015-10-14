using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;


namespace idlereporter
{
    class Program
    {

        static void Main(string[] args)
        {

            string sSource = "IdleReporter";
            string sLog = "Application";
            string sEvent = "UserIsIdle";

            int iAalarmIdleTimeInSeconds = 1020;

            bool toFile = false;
            bool toEventLog = false;

            bool idle = false;
            bool lastidle = false;

            string toFileDirectory = System.Environment.GetEnvironmentVariable("TEMP");



            // check if we have args
            if (args.Length > 0) { 

                // if arg 0 is --install, register eventlog source, elevated cli needed
                if (args[0] == @"--install")
                {
                    Console.WriteLine("installing eventlog source");
                    if (!EventLog.SourceExists(sSource))
                      EventLog.CreateEventSource(sSource, sLog);
                    
                    Console.WriteLine("idle event will be logged as event-id 65201.");
                    Environment.Exit(0);
                }

                // parse idle seconds arg
                try
                {
                    iAalarmIdleTimeInSeconds = Int32.Parse(args[0]);

                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                    System.Environment.Exit(1);
                }

                if (args.Length > 1)
                {
                    if (System.IO.Directory.Exists(args[1]))
                    {
                        toFile = true;
                        toFileDirectory = args[1];
                        Console.WriteLine("idle-2-file mode.");
                    } else
                    {
                        toEventLog = true;
                        toFile = false;
                        Console.WriteLine("idle-2-eventlog mode.");
                    }

                }

            } else
            {
                Console.WriteLine("usage:");
                Console.WriteLine("idlereporter.exe --install # install eventlogsource in admin cli");
                Console.WriteLine("idlereporter.exe <secondsbeforeidle> <directory>  # create file in directory,filenmae=hostname ");
                Console.WriteLine("idlereporter.exe <secondsbeforeidle>  # write event 65201 to eventlog");
                System.Environment.Exit(0);
            }


            lastidle = false;
            while (true) {

                Thread.Sleep(5000);

                if (GetLastUserInput.GetIdleTickCount() >= iAalarmIdleTimeInSeconds * 1000)
                {
                   
                    idle = true;
                } else
                {
                    
                    idle = false;
                }

                

                if (lastidle != idle)
                {
                    lastidle = idle;

                    if (idle == false)
                    {


                        Console.WriteLine("UserIsNotIdle");


                        if (toEventLog)
                        {
                            EventLog.WriteEntry(sSource, sEvent);
                            EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Information, 65201);
                        }
                        else
                        {
                            if (toFile)
                            {
                                Presence.setIdle(toFileDirectory, false);
                            }
                        }




                    }
                    else
                    {
                  




                        Console.WriteLine("UserIsIdle");
                        idle = false;
                        if (toEventLog)
                        {
                            EventLog.WriteEntry(sSource, sEvent);
                            EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Information, 65202);
                        }
                        else
                        {
                            if (toFile)
                            {
                                Presence.setIdle(toFileDirectory, true);
                            }
                        }


                    }

                }
            
            }

        }
    }
}
