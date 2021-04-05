using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Utilities.Logger
{
    public class LogWorker
    {
        string module;
        string function;
        string message;


        public LogWorker(string module, string function, string message)
        {
            this.module = module;
            this.function = function;
            this.message = message;

            Thread t = new Thread(this.Run);
            t.Start();
            Thread.Sleep(5);
        }


        private void Run()
        {

            try
            {
                string filpath = "C:\\XChangeApiLog\\";
                if (!Directory.Exists(filpath))
                {
                    Directory.CreateDirectory(filpath);
                }
                filpath += "\\Logs\\";
                if (!Directory.Exists(filpath))
                {
                    Directory.CreateDirectory(filpath);
                }
                filpath += @"\" + DateTime.Now.ToString("ddMMMyyy") + ".txt";
                File.AppendAllLines(filpath,
                    new string[] {
                        "Event Time: "      + DateTime.Now.ToString() + "\t" +
                        "Module Name: "     + module + "\t" +
                        "Function Name: "   + function + "\t" +
                        "Message: "         + message
                    });
                //backup
                FileInfo i = new FileInfo(filpath);
                if (i.Length > 10000000)
                {
                    i.MoveTo(DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".txt");
                }



            }
            catch (Exception er) { }
        }
    }

    public class Logger
    {
        public void LogError(string module, string function, string message)
        {
            new LogWorker(module, function, message);
        }


    }
}