using Controllers;
using System;
using System.Collections.Generic;

namespace GoogleSheets.Models
{
    public class GoogleSheet
    {
        public class DataBase
        {
            public DataBase(string name)
            {
                this.Name = name;
            }
            public string Name { get; set; }
            public string Size { get; set; }
        }
        public GoogleSheet(string connectionString)
        {
            this.connectionString = connectionString;
            Load();
        }
        
        public string serverName { get; set; }

        public string connectionString { get; set; }
        public List<DataBase> dataBases { get; set; }
        public double serverSpace { get; set; }
        public string lastUpdate { get; set; }

        public double CurrentSize()
        {
            double size = 0;
            foreach (var item in dataBases)
            {
                string[] sizeString = item.Size.Split(' ');
                if (sizeString[1] == "kB")
                {
                    size += double.Parse(sizeString[0]) / (1024 * 1024 * 1024);
                }
                else if (sizeString[1] == "MB")
                {
                    size += double.Parse(sizeString[0]) / 1024;
                }
                else if (sizeString[1] == "GB")
                {
                    size += double.Parse(sizeString[0]);
                }
            }
            return size;
        }

        private void Load()
        {
            PsGre db = new PsGre(connectionString);
            List<DataBase> dataBasesList = new List<DataBase>();
            foreach (var item in db.SelectDatabases())
            {
                DataBase newdb = new DataBase(item);
                dataBasesList.Add(newdb);
            }
            dataBases = dataBasesList;
            return;
        }

        public void Update()
        {
            PsGre db = new PsGre(connectionString);
            foreach (var item in dataBases)
                item.Size = db.SelectSize(item.Name);
            lastUpdate = DateTime.Now.ToString("D");
        }
    }
}
