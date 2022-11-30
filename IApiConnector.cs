namespace GoogleSpreadsheetRepository;

public interface IApiConnector
{
    IEnumerable<string> GetAllTabsNames();
    Task<IEnumerable<string>> GetAllTabsNamesAsync();

    IEnumerable<T>? GetAllObjects<T>(string tabName)  where T : new();
    Task<IEnumerable<T>?> GetAllObjectsAsync<T>(string tabName, CancellationToken cancellationToken)  where T : new();
}