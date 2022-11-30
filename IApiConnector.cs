namespace GoogleSpreadsheetRepository;

public interface IApiConnector
{
    IEnumerable<string> GetAllTabsNames();
    Task<IEnumerable<string>> GetAllTabsNamesAsync();

    IEnumerable<T>? GetAllObjects<T>(string tabName, int maxCount = 60)  where T : new();
    Task<IEnumerable<T>?> GetAllObjectsAsync<T>(string tabName, CancellationToken cancellationToken, int maxCount = 60)  where T : new();
}