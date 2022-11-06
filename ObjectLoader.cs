namespace GoogleSpreadsheetRepository;

public class SpreadsheetTabNoHeadersException : Exception
{
    public SpreadsheetTabNoHeadersException() : base("Spreadsheet tab have no headers!")
    {
    }
}

public class ObjectLoader 
{
    private readonly IApiConnector _googleApiConnector;

    private List<Tab> Tabs { get; }

    public ObjectLoader(IApiConnector apiConnectorConnector)
    {
        _googleApiConnector = apiConnectorConnector;
        Tabs = new List<Tab>();
    }

    public Tab[] LoadAllTabs()
    {
        var tabNames = _googleApiConnector.GetAllTabsNames();
        var returnValue = new List<Tab>();
        
        Parallel.ForEach(tabNames, tabName =>
        {
            returnValue.Add(LoadTab(tabName));
        });
        
        return returnValue.ToArray();
    }

    public Tab[] LoadAllTabsExcept(string? startPrefix = null)
    {
        var tabNames = _googleApiConnector.GetAllTabsNames();
        var returnValue = new List<Tab>();
        
        Parallel.ForEach(tabNames, tabName =>
        {
            if (string.IsNullOrWhiteSpace(startPrefix) || tabName[..startPrefix.Length] != startPrefix)
            {
                returnValue.Add(LoadTab(tabName));
            }
        });
        
        return returnValue.ToArray();
    }
    
    public Tab LoadTab(string tabName)
    {
        var rowValues = _googleApiConnector.LoadTabData(tabName);
        if (rowValues.Count < 1) throw new SpreadsheetTabNoHeadersException();

        var values = rowValues
            .Select(o => 
                o.Select(x => 
                    (string)x).ToArray()
            )
            .ToList();

        var headers = values.First();
        var allObjects = RemoveTrashValues(values, headers.Length).GetRange(1, values.Count-1);
        var objectDictionaries = new List<Dictionary<string, string>>();
        foreach (var sa in allObjects)
        {
            var dictionary = new Dictionary<string, string>();
            for (var i = 0; i < headers.Length; i++)
            {
                dictionary.Add(key: headers[i], value: sa[i]);
            }
            objectDictionaries.Add(dictionary);
        }

        return new Tab(tabName, headers, objectDictionaries);
    }
    
    public async Task<Tab> LoadTabAsync(string tabName)
    {
        var rowValues = await _googleApiConnector.LoadTabDataAsync(tabName);
        if (rowValues.Count < 1) throw new SpreadsheetTabNoHeadersException();

        var values = rowValues
            .Select(o => 
                o.Select(x => 
                    (string)x).ToArray()
            )
            .ToList();

        var headers = values.First();
        var allObjects = RemoveTrashValues(values, headers.Length).GetRange(1, values.Count-1);
        var objectDictionaries = new List<Dictionary<string, string>>();
        foreach (var sa in allObjects)
        {
            var dictionary = new Dictionary<string, string>();
            for (var i = 0; i < headers.Length; i++)
            {
                dictionary.Add(key: headers[i], value: sa[i]);
            }
            objectDictionaries.Add(dictionary);
        }

        return new Tab(tabName, headers, objectDictionaries);
    }

    private List<string[]> RemoveTrashValues(List<string[]> values, int maxColumns, bool allowEmtyCells = true)
    {
        if (maxColumns <= 0) throw new ArgumentOutOfRangeException();

        var outList = new List<string[]>();

        for (var i = 0; i < values.Count; i++)
        {
            if (values[i].Length == 0) continue;
            
            var value = values[i];
            if (values[i].Length < maxColumns)
            {
                if (!allowEmtyCells) throw new Exception("Empty cells not allowed!");
                var newValue = new string[maxColumns];
                for (var index = 0; index < newValue.Length; index++)
                {
                    newValue[index] = index < values[i].Length ? value[index] : "";
                }

                value = newValue;
            }
            else
            {
                value = values[i].Take(maxColumns).ToArray();
            }
            outList.Add(value);
            
        }

        return outList;
    }
}