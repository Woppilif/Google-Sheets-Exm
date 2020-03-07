using System.Collections.Generic;

namespace GoogleSheets.Models
{
    class SpreadsheetClass
    {
        public class AccountGoogle
        {
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
        }
        public List<GoogleSheet> Servers { get; set; }
        public string SpreadsheetId { get; set; }

        public AccountGoogle GoogleAccount { get; set; }

        public void UpdateServers()
        {
            foreach (var item in Servers)
                item.Update();
        }

    }
}
