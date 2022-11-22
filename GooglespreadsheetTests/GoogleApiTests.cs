using GoogleSpreadsheetRepository;

namespace GooglespreadsheetTests;

public class GoogleApiTests
{
    [Test]
    public void UpdateTab__WithEmptyCells_ReturnCorrectTabs()
    {
        var api = new GoogleApi("secrets.json", "1DilmUmo9ngvqCh1RxdgFAdUu3eY-zdRaSBw0fXbl1OE");
        var x = api.GetTabObjects<ConfigStats>("__config_stats");
        Console.WriteLine("e");
    }
}

public class ConfigStats
{
    public string ConfigName { get; set; }
    public int Graphic { get; set; }
    public int Processor { get; set; }
    public int Memory { get; set; }
    public int Storage { get; set; }
}