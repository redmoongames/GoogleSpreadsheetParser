namespace GoogleSpreadsheetRepository;

public interface IApiConnector
{
    string[] GetAllTabsNames();

    IList<IList<object>> LoadTabData(string tabName);
    Task<IList<IList<object>>> LoadTabDataAsync(string tabName);
}