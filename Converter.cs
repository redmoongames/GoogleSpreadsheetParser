using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace GoogleSpreadsheetRepository;


public class Converter
{
    public IEnumerable<T> ConvertTo<T>(IList<IList<object>> rowData) 
        where T : new()
    {
        var headers = rowData
            .First()
            .Select(x => ((string)x)
                .ToLower()
                .Replace(" ","")
            )
            .ToList();

        var allRows = rowData
            .Where(val => !Equals(val, rowData.First()))
            .Select(row => row
                .Select(item => (string)item)
                .ToArray()
            );
        
        var indexesOfProperties = IndexesOfGenericClassPropertiesInList<T>(headers);

        var returnObjects = new List<T>();
        foreach (var rowOfStrings in allRows)
        {
            var returnObject = new T();
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                SetPropertyOfObject(indexesOfProperties, propertyInfo, rowOfStrings, returnObject);
            }
            returnObjects.Add(returnObject);
        }

        return returnObjects;
    }

    private static void SetPropertyOfObject<T>(Dictionary<PropertyInfo, int> indexesOfProperties, PropertyInfo propertyInfo, string[] row,
        [DisallowNull] T cell) where T : new()
    {
        object? spreadsheetValue = null;
        if (indexesOfProperties.TryGetValue(propertyInfo, out var indexOfProperty))
        {
            if (indexOfProperty < row.Length)
            {
                try
                {
                    spreadsheetValue = Convert.ChangeType(row[indexOfProperty], propertyInfo.PropertyType);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    spreadsheetValue = null;
                }
            }
        }

        propertyInfo.SetValue(cell, spreadsheetValue);
    }


    private static Dictionary<PropertyInfo, int> IndexesOfGenericClassPropertiesInList<T>(IList<string> headers) where T : new()
    {
        var indexesOfPropNamesInRowData = new Dictionary<PropertyInfo, int>();
        foreach (var propertyInfo in typeof(T).GetProperties())
        {
            var currentPropertyName = propertyInfo.Name.ToLower();
            var index = headers.IndexOf(currentPropertyName);
            indexesOfPropNamesInRowData.Add(propertyInfo, index);
        }

        return indexesOfPropNamesInRowData;
    }

}