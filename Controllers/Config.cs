using System.IO;
using Newtonsoft.Json;
using GoogleSheets.Models;

namespace ConConfig
{
    class Config
    {
        public SpreadsheetClass Settings { get; set; }
        public Config()
        {
            using (StreamReader r = new StreamReader("settings.json"))
            {
                string json = r.ReadToEnd();
                this.Settings = (SpreadsheetClass)JsonConvert.DeserializeObject(json, typeof(SpreadsheetClass));   
            }
        }
    }
}