using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace GoogleSpreadsheetRepository;

public class GoogleApi : IApiConnector
{
    private readonly string _googleSpreadsheetId;

    public enum JsonType
    {
        JsonIsFilePath,
        JsonIsRawData
    }
    
    private static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
    private SheetsService _service;

    public GoogleApi(string json, string googleSpreadsheetId, JsonType jsonType = JsonType.JsonIsFilePath)
    {
        _googleSpreadsheetId = googleSpreadsheetId;
        var credential = GenerateCredential(json, jsonType);

        _service = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "ApplicationName"
        });
    }

    /// <summary>
    /// Long operation...
    /// </summary>
    public string[] GetAllTabsNames()
    {
        var request = _service.Spreadsheets.Get(_googleSpreadsheetId);
        var response = request.Execute();
        var tabNames = new string[response.Sheets.Count];
        for (var i = 0; i < response.Sheets.Count; i++)
        {
            tabNames[i] = response.Sheets[i].Properties.Title;
        }

        return tabNames;
    }

    /// <summary>
    /// Long operation...
    /// </summary>
    public IList<IList<object>> LoadTabData(string tabName)
    {
        var rangeFindColumn = $"{tabName}!A1:Z30";
        var request = _service.Spreadsheets.Values.Get(_googleSpreadsheetId, rangeFindColumn);
        var response = request.Execute();
        return response.Values;
    }

    public async Task<IList<IList<object>>> LoadTabDataAsync(string tabName)
    {
        var rangeFindColumn = $"{tabName}!A1:Z30";
        var request = _service.Spreadsheets.Values.Get(_googleSpreadsheetId, rangeFindColumn);
        var response = await request.ExecuteAsync();
        return response.Values;
    }

    private static GoogleCredential GenerateCredential(string json, JsonType jsonType)
    {
        GoogleCredential credential;

        switch (jsonType)
        {
            case JsonType.JsonIsFilePath:
                using (var stream = new FileStream(json, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
                }

                break;
            case JsonType.JsonIsRawData:
                credential = GoogleCredential.FromJson(json).CreateScoped(Scopes);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(jsonType), jsonType, null);
        }

        return credential;
    }
}