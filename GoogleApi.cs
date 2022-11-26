using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace GoogleSpreadsheetRepository;

public class GoogleApi
{
    private readonly string _googleSpreadsheetId;

    public enum JsonType
    {
        JsonIsFilePath,
        JsonIsRawData
    }
    
    private SheetsService _service;

    public GoogleApi(string json, string googleSpreadsheetId, JsonType jsonType = JsonType.JsonIsFilePath)
    {
        _googleSpreadsheetId = googleSpreadsheetId;
        
        
        GoogleCredential credential;
        switch (jsonType)
        {
            case JsonType.JsonIsFilePath:
                using (var stream = new FileStream(json, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream).CreateScoped(SheetsService.Scope.Spreadsheets);
                }

                break;
            case JsonType.JsonIsRawData:
                credential = GoogleCredential.FromJson(json).CreateScoped(SheetsService.Scope.Spreadsheets);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(jsonType), jsonType, null);
        }

        _service = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "ApplicationName"
        });
    }

    public IEnumerable<string> GetTabsNames()
    {
        return Task.Run(GetTabsNamesAsync).Result;
    }

    public async Task<IEnumerable<string>> GetTabsNamesAsync()
    {
        var request = _service.Spreadsheets.Get(_googleSpreadsheetId);
        try
        {
            var response = await request.ExecuteAsync();
            if (response == null) return new List<string>();
            var tabNames = new string[response.Sheets.Count];
            for (var i = 0; i < response.Sheets.Count; i++)
            {
                tabNames[i] = response.Sheets[i].Properties.Title;
            }

            return tabNames;
        }
        catch
        {
            return new List<string>();
        }
        
    }

    public IList<IList<object>> GetTabObjects(string tabName, int maxCount = 60) 
    {
        return Task.Run(() => GetTabObjectsAsync(tabName, maxCount)).Result;
    }

    public async Task<IList<IList<object>>> GetTabObjectsAsync(string tabName, int maxCount = 60)
    {
        if (maxCount <= 0) throw new ArgumentOutOfRangeException();
        var rangeFindColumn = $"{tabName}!A1:Z{maxCount}";
        var request = _service.Spreadsheets.Values.Get(_googleSpreadsheetId, rangeFindColumn);
        try
        {
            return (await request.ExecuteAsync()).Values;
        }
        catch
        {
            return new List<IList<object>>();
        }
    }
    
}