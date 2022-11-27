namespace GoogleSpreadsheetRepository;

public class SpreadSheetAccess : IApiConnector
{
    private readonly GoogleApi _api;
    private readonly Converter _converter;

    public SpreadSheetAccess(string jsonPath, string spreadsheetId)
    {
        _api = new GoogleApi(jsonPath, spreadsheetId);
        _converter = new Converter();
    }

    public IEnumerable<string> GetAllTabsNames()
    {
        return Task.Run(GetAllTabsNamesAsync).Result;
    }

    public async Task<IEnumerable<string>> GetAllTabsNamesAsync()
    {
        return await _api.GetTabsNamesAsync();
    }

    public IEnumerable<T> GetAllObjects<T>(string tabName, int maxCount = 60) where T : new()
    {
        return Task.Run(() => GetAllObjectsAsync<T>(tabName, maxCount)).Result;
    }

    public async Task<IEnumerable<T>> GetAllObjectsAsync<T>(string tabName, int maxCount = 60) where T : new()
    {
        var objects = await _api.GetTabObjectsAsync(tabName);
        var result = _converter.ConvertTo<T>(objects);
        return result;
    }
}