// using System.Reflection;
//
// namespace GoogleSpreadsheetRepository;
//
// public class SpreadSheetRepository 
// {
//     private readonly IApiConnector _apiConnectorConnector;
//     private readonly IApiConnector _googleApiConnector;
//
//     public SpreadSheetRepository(IApiConnector apiConnectorConnector)
//     {
//         _apiConnectorConnector = apiConnectorConnector;
//     }
//
//     public string[] LoadAllTabNames()
//     {
//         return _googleApiConnector.GetTabsNames();
//     }
//
//     public T[] LoadAllObjects<T>(string tabName) where T : new()
//     {
//         var columnNames = _apiConnectorConnector.GetTabsNames();
//
//         return _apiConnectorConnector
//             .LoadRowData(tabName)
//             .Select(rowObject => TransformToObject<T>(rowObject, columnNames))
//             .ToArray();
//     }
//
//     private T TransformToObject<T>(IList<object> rowObjects, IList<string> columnNames) where T : new()
//     {
//         var genericProperties = typeof(T)
//             .GetProperties()
//             .Select(prop => prop)
//             .ToArray();
//
//         var tabData = TransformValues(rowTabData, columnNames.Length);
//
//         var genericPropAndIndexesInColumns = FindIndexOfMatchesArray(columnNames);
//         
//         
//         for (int row = 1; row < tabData.Count; row++)
//         {
//             var returnValue = new T();
//             foreach (var property in genericProperties)
//             {
//                 var columnIndex = genericPropAndIndexesInColumns[property];
//                 var value = tabData[row][columnIndex];
//                 
//                 property.SetValue(returnValue, value, null);
//             }
//         }
//         
//         
//         return returnValue;
//     }
//
//     private Dictionary<PropertyInfo, int> FindIndexOfMatchesArray(string[] values, PropertyInfo[] propertyInfos)
//     {
//         foreach (var property in propertyInfos)
//         {
//             var haveColumnWithSameName = false;
//             for (var columnNum = 0; columnNum < values.Length; columnNum++)
//             {
//                 if (values[columnNum] != property.Name) continue;
//                 haveColumnWithSameName = true;
//                 WhatGenericPropInWhatColumnNum.Add(property, columnNum);
//             }
//
//             if (!haveColumnWithSameName) throw new NullReferenceException();
//         }
//     }
//
//     private List<string[]> TransformValues(IList<IList<object>> values, int maxColumns, bool allowEmtyCells = true)
//     {
//         if (maxColumns <= 0) throw new ArgumentOutOfRangeException();
//
//         var outList = new List<string[]>();
//
//         for (var i = 0; i < values.Count; i++)
//         {
//             if (values[i].Length == 0) continue;
//             
//             var value = values[i];
//             if (values[i].Length < maxColumns)
//             {
//                 if (!allowEmtyCells) throw new Exception("Empty cells not allowed!");
//                 var newValue = new string[maxColumns];
//                 for (var index = 0; index < newValue.Length; index++)
//                 {
//                     newValue[index] = index < values[i].Length ? value[index] : "";
//                 }
//
//                 value = newValue;
//             }
//             else
//             {
//                 value = values[i].Take(maxColumns).ToArray();
//             }
//             outList.Add(value);
//             
//         }
//
//         return outList;
//     }
// }