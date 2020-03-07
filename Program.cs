using System;
using System.Timers;
using ConConfig;
using Controllers;
using GoogleSheets.Models;

namespace GoogleSheets
{
    class Program
    {
        static Config config = new Config();
        static SpreadsheetClass spreadsheet = config.Settings;
        static GoogleAPI google = new GoogleAPI(config.Settings.GoogleAccount);
        static void Main(string[] args)
        {
            spreadsheet = google.CreateSheet(spreadsheet);
            Console.WriteLine(spreadsheet.SpreadsheetId);
            SetTimer();
            Console.ReadKey();
        }

        private static void SetTimer()
        {
            Timer  aTimer = new Timer(10000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            spreadsheet.UpdateServers();
            google.UpdateEntity(spreadsheet);
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                              e.SignalTime);
        }
    }
}
