namespace GoogleSpreadsheetRepository;

public class Tab
{
    public Tab(string name, string[] headers, List<Dictionary<string, string>> content)
    {
        Name = name;
        Headers = headers;
        Content = content;
    }

    public string Name { get; set; }
    public List<Dictionary<string, string>> Content { get; set; }
    public string[] Headers { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Tab other) return false;
        if (Name != other.Name) return false;
        if (!Headers.SequenceEqual(other.Headers)) return false;
        foreach (var dictionary in Content)
        {
            foreach (var otherDictionary in other.Content)
            {
                if (dictionary.SequenceEqual(otherDictionary)) break;
                if (otherDictionary == other.Content.Last()) return false;
            }
        }
        
        return true;
    }
}