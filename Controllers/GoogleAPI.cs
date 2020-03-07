using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Threading;
using GoogleSheets.Models;
using static GoogleSheets.Models.SpreadsheetClass;

namespace Controllers
{
    class GoogleAPI
    {
        private static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private static string ApplicationName = "Google Sheets Exmx";
        private SheetsService service;

        public GoogleAPI(AccountGoogle account)
        {
            // Create Google Sheets API service.
            this.service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = this.getCredential(account),
                ApplicationName = ApplicationName,
            });
        }

        private UserCredential getCredential(AccountGoogle account)
        {
            UserCredential credential;
            string credPath = "token.json";
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = account.ClientId,
                    ClientSecret = account.ClientSecret
                },
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
            Console.WriteLine("Credential file saved to: " + credPath);
                
            
            return credential;
        }

        public SpreadsheetClass CreateSheet(SpreadsheetClass sheetEntities)
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.Properties = new SpreadsheetProperties();
            spreadsheet.Properties.Title = "Google Sheets Exm";
            List<Sheet> sheets = new List<Sheet>();
            foreach (GoogleSheet item in sheetEntities.Servers)
            {
                Sheet sheet = new Sheet();
                sheet.Properties = new SheetProperties();
                sheet.Properties.Title = item.serverName;
                sheets.Add(sheet);
            }
            spreadsheet.Sheets = sheets;
            SpreadsheetsResource.CreateRequest request = this.service.Spreadsheets.Create(spreadsheet);
            var x = request.Execute();
            sheetEntities.SpreadsheetId = x.SpreadsheetId;
            return sheetEntities;
        }

        public void UpdateEntity(SpreadsheetClass sheetEntities)
        {
            foreach (GoogleSheet item in sheetEntities.Servers)
            {
                string range = $"{item.serverName}!A1:D";                
                ValueRange requestBody = new ValueRange();
                requestBody.Values = new List<IList<object>>();
                foreach (var itm in item.dataBases)
                {
                    List<object> objectList = new List<object>();
                    objectList.Add(item.serverName);
                    objectList.Add(itm.Name);
                    objectList.Add(itm.Size);
                    objectList.Add(item.lastUpdate);
                    requestBody.Values.Add(objectList);
                }
                List<object> schemaInfo = new List<object>();
                schemaInfo.Add(item.serverName);
                schemaInfo.Add("Свободно");
                schemaInfo.Add($"{item.serverSpace - item.CurrentSize()}");
                schemaInfo.Add(DateTime.Now.ToString("D"));
                requestBody.Values.Add(schemaInfo);
                var appendRequest = service.Spreadsheets.Values.Append(requestBody, sheetEntities.SpreadsheetId, range);
                appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
                var appendResponse = appendRequest.Execute();
            }
                
        }
    }
}
