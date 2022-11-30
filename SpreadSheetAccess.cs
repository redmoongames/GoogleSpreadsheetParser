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

    public IEnumerable<T>? GetAllObjects<T>(string tabName) where T : new()
    {
        return Task.Run(() => GetAllObjectsAsync<T>(tabName, CancellationToken.None)).Result;
    }

    public async Task<IEnumerable<T>?> GetAllObjectsAsync<T>(string tabName, CancellationToken cancellationToken) where T : new()
    {
        IList<IList<object>> objects = null;
        var delay = 1_000;
        const int maxDelay = 120_000;
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                objects = await _api.GetTabObjectsAsync(tabName);
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine($"CANNOT LOAD FROM SPREADSHEET\n{e.Message}");
                await Task.Delay(delay, cancellationToken);
                delay = delay * 2 <= maxDelay ? delay * 2 : maxDelay;
            }
        }

        return objects == null ? null : _converter.ConvertTo<T>(objects);
    }
}