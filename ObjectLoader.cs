// namespace GoogleSpreadsheetRepository;
//
// public class SpreadsheetTabNoHeadersException : Exception
// {
//     public SpreadsheetTabNoHeadersException() : base("Spreadsheet tab have no headers!")
//     {
//     }
// }
//
// public class ObjectLoader 
// {
//     private readonly IApiConnector _googleApiConnector;
//
//     private List<Tab> Tabs { get; }
//
//     public ObjectLoader(IApiConnector apiConnectorConnector)
//     {
//         _googleApiConnector = apiConnectorConnector;
//         Tabs = new List<Tab>();
//     }
//
//     public Tab[] LoadAllTabs()
//     {
//         var tabNames = _googleApiConnector.GetTabsNames();
//         var returnValue = new List<Tab>();
//         
//         Parallel.ForEach(tabNames, tabName =>
//         {
//             returnValue.Add(LoadTab(tabName));
//         });
//         
//         return returnValue.ToArray();
//     }
//
//     public Tab[] LoadAllTabsExcept(string? startPrefix = null)
//     {
//         var tabNames = _googleApiConnector.GetTabsNames();
//         var returnValue = new List<Tab>();
//         
//         Parallel.ForEach(tabNames, tabName =>
//         {
//             if (string.IsNullOrWhiteSpace(startPrefix) || tabName[..startPrefix.Length] != startPrefix)
//             {
//                 returnValue.Add(LoadTab(tabName));
//             }
//         });
//         
//         return returnValue.ToArray();
//     }
//     
//     public Tab LoadTab(string tabName)
//     {
//         var rowValues = _googleApiConnector.LoadRowData(tabName);
//         if (rowValues.Count < 1) throw new SpreadsheetTabNoHeadersException();
//
//         var values = rowValues
//             .Select(o => 
//                 o.Select(x => 
//                     (string)x).ToArray()
//             )
//             .ToList();
//
//         var headers = values.First();
//         var allObjects = RemoveTrashValues(values, headers.Length).GetRange(1, values.Count-1);
//         var objectDictionaries = new List<Dictionary<string, string>>();
//         foreach (var sa in allObjects)
//         {
//             var dictionary = new Dictionary<string, string>();
//             for (var i = 0; i < headers.Length; i++)
//             {
//                 dictionary.Add(key: headers[i], value: sa[i]);
//             }
//             objectDictionaries.Add(dictionary);
//         }
//
//         return new Tab(tabName, headers, objectDictionaries);
//     }
//     
//     public async Task<Tab> LoadTabAsync(string tabName)
//     {
//         var rowValues = await _googleApiConnector.LoadRowDataAsync(tabName);
//         if (rowValues.Count < 1) throw new SpreadsheetTabNoHeadersException();
//
//         var values = rowValues
//             .Select(o => 
//                 o.Select(x => 
//                     (string)x).ToArray()
//             )
//             .ToList();
//
//         var headers = values.First();
//         var allObjects = RemoveTrashValues(values, headers.Length).GetRange(1, values.Count-1);
//         var objectDictionaries = new List<Dictionary<string, string>>();
//         foreach (var sa in allObjects)
//         {
//             var dictionary = new Dictionary<string, string>();
//             for (var i = 0; i < headers.Length; i++)
//             {
//                 dictionary.Add(key: headers[i], value: sa[i]);
//             }
//             objectDictionaries.Add(dictionary);
//         }
//
//         return new Tab(tabName, headers, objectDictionaries);
//     }
// }