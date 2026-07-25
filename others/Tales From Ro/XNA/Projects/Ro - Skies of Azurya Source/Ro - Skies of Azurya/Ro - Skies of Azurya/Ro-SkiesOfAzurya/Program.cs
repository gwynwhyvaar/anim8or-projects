using System;
using SkiesOfAzurya;
using System.IO;

namespace Nazvhi_VydmanteaeanSkies
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SkiesOfAzuryaClass game = new SkiesOfAzuryaClass())
            {
                try
                {
                    game.Run();
                }
                catch (Exception exxxxx)
                {
                    File.AppendAllText("c:\\ro.azurya.log", DateTime.Now.ToString() + "\n\r#Error :" + exxxxx.Message + "\n\r\tStacktrace :" + exxxxx.StackTrace + "\n\r\n\r");
                }
            }
        }
    }
}

