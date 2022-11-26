using GoogleSpreadsheetRepository;

namespace qwdf;

public class ConverterTest
{
    private class Temp
    {
        public int Header1 { get; set; }
        public int Header2 { get; set; }
        public string Header3 { get; set; }
    }
    
    [Test]
    public void ConvertTo__WhenCall__DataSample()
    {
        IList<IList<object>> rowData = new List<IList<object>>
        {
            new List<object>
            {
                "Header1", "Header2", "header 3"
            },
            new List<object>
            {
                "1", "A2", "A3",
            },
            new List<object>
            {
                "2", "B2", 
            },
            new List<object>
            {
                "3", "C2", "C3", "C4",
            },
        };
        
        var result = new Converter().ConvertTo<Temp>(rowData);
        
        
        Assert.That(true);
    }
}

public class GoogleApiTests
{
    // [Test]
    // public void UpdateTab__WithEmptyCells_ReturnCorrectTabs()
    // {
    //     var api = new GoogleApi("secrets.json", "1DilmUmo9ngvqCh1RxdgFAdUu3eY-zdRaSBw0fXbl1OE");
    //     var x = api.GetTabObjects<ConfigStats>("__config_stats");
    // }
}

public class ConfigStats
{
    public string ConfigName { get; set; }
    public int Graphic { get; set; }
    public int Processor { get; set; }
    public int Memory { get; set; }
    public int Storage { get; set; }
}