namespace GoogleSpreadsheetRepository;

public interface IApiConnector
{
    string[] GetTabsNames();
    Task<string[]> GetTabsNamesAsync();

    T[] GetTabObjects<T>(string tabName) where T : new();
    Task<T[]> GetTabObjectsAsync<T>(string tabName) where T : new();

}