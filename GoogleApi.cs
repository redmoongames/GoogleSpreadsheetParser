using System.Reflection;
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
        var credential = GenerateApiCredential(json, jsonType);

        _service = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "ApplicationName"
        });
    }

    public string[]? GetTabsNames()
    {
        return Task.Run(GetTabsNamesAsync).Result;
    }

    public async Task<string[]?> GetTabsNamesAsync()
    {
        var request = _service.Spreadsheets.Get(_googleSpreadsheetId);
        try
        {
            var response = await request.ExecuteAsync();
            if (response == null) return null;
            var tabNames = new string[response.Sheets.Count];
            for (var i = 0; i < response.Sheets.Count; i++)
            {
                tabNames[i] = response.Sheets[i].Properties.Title;
            }

            return tabNames;
        }
        catch
        {
            return null;
        }
        
    }

    public T[]? GetTabObjects<T>(string tabName) where T : new()
    {
        return Task.Run(() => GetTabObjectsAsync<T>(tabName)).Result ?? null;
    }

    public async Task<T[]?> GetTabObjectsAsync<T>(string tabName) where T : new()
    {
        var rowData = await LoadRowDataAsync(tabName);
        if (rowData == null) return null;
        var headers = GetHeaders<T>(rowData);
        var rowObjectsArray = RefactorRowData(rowData, headers);
        var indexesOfPropNamesInRowData = GetRowDataIndexesOfGenericClass<T>(headers);

        var returnList = new List<T>();
        foreach (var rowObject in rowObjectsArray)
        {
            var genericItem = new T();
            foreach (var propertyPair in indexesOfPropNamesInRowData)
            {
                try
                {
                    var targetType = propertyPair.Key.PropertyType;
                    var index = propertyPair.Value ?? -1;
                    
                    var value = Convert.ChangeType(rowObject[index], targetType);
                    
                    propertyPair.Key.SetValue(genericItem, value);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    propertyPair.Key.SetValue(genericItem, null);
                }
            }
            returnList.Add(genericItem);
        }

        return returnList.ToArray();
    }

    private static Dictionary<PropertyInfo, int?> GetRowDataIndexesOfGenericClass<T>(string[] headers) where T : new()
    {
        var indexesOfPropNamesInRowData = new Dictionary<PropertyInfo, int?>();
        foreach (var propertyInfo in typeof(T).GetProperties())
        {
            var index = FindIndexOfPropertyName(propertyInfo, headers);
            indexesOfPropNamesInRowData.Add(propertyInfo, index);
        }

        return indexesOfPropNamesInRowData;
    }

    private static IEnumerable<object[]> RefactorRowData(IList<IList<object>> rowData, string[] headers)
    {
        var arrayOfArrayOfProperties = new object[rowData.Count - 1][];
        for (var i = 1; i < rowData.Count; i++)
        {
            arrayOfArrayOfProperties[i-1] = rowData[i].ToArray();
            Array.Resize(ref arrayOfArrayOfProperties[i-1], headers.Length);
        }

        return arrayOfArrayOfProperties;
    }

    private static string[] GetHeaders<T>(IList<IList<object>> rowData) where T : new()
    {
        var headers = rowData
            .First(x => true)
            .Select(x => (string)x)
            .ToArray();
        return headers;
    }

    private static int? FindIndexOfPropertyName(MemberInfo propertyInfo, IReadOnlyList<string> targets)
    {
        for (var i = 0; i < targets.Count; i++)
        {
            if (targets[i] == propertyInfo.Name) return i;
        }
        return null;
    }

    private static GoogleCredential GenerateApiCredential(string json, JsonType jsonType)
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
    

    private IList<IList<object>>? LoadRowData(string tabName)
    {
        return Task.Run(() => LoadRowDataAsync(tabName)).Result;
    }
    

    private async Task<IList<IList<object>>?> LoadRowDataAsync(string tabName)
    {
        var rangeFindColumn = $"{tabName}!A1:Z30";
        var request = _service.Spreadsheets.Values.Get(_googleSpreadsheetId, rangeFindColumn);
        try
        {
            return (await request.ExecuteAsync()).Values;
        }
        catch
        {
            return null;
        }
    }
}