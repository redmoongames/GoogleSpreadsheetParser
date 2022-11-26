namespace GoogleSpreadsheetRepository;

public interface IApiConnector
{
    IEnumerable<string> GetAllTabsNames();
    Task<IEnumerable<string>> GetAllTabsNamesAsync();

    IEnumerable<T> GetAllObjects<T>(string tabName, int maxCount)  where T : new();
    Task<IEnumerable<T>> GetAllObjectsAsync<T>(string tabName, int maxCount)  where T : new();
}