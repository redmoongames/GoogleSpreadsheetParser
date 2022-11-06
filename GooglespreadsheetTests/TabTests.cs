using GoogleSpreadsheetRepository;

namespace GooglespreadsheetTests;

public class TabTests
{
    [Test]
    public void Equals__ValuesIsEqual__ReturnTrue()
    {
        var firstTab = new Tab(
            name: "tab_name",
            headers: new[] { "column_A" },
            content: new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("column_A", "A1")
                }),
                new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("column_A", "A2")
                })
            });
        var secondTab = new Tab(
            name: "tab_name",
            headers: new[] { "column_A" },
            content: new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("column_A", "A1")
                }),
                new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("column_A", "A2")
                })
            });

        var result = firstTab.Equals(secondTab);
        
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals__TabsHaveDifferentNames__ReturnFalse()
    {
        var firstTab = new Tab(
            name: "tab_name",
            headers: new[] { "column_A" },
            content: new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("column_A", "A1")
                })
            });
        var secondTab = new Tab(
            name: "different_name",
            headers: new[] { "column_A" },
            content: new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("column_A", "A1")
                })
            });

        var result = firstTab.Equals(secondTab);
        
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals__TabsHaveDifferentHeaders__ReturnFalse()
    {
        var firstTab = new Tab(
            name: "tab_name",
            headers: new[] { "column_A" },
            content: new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("column_A", "A1")
                })
            });
        var secondTab = new Tab(
            name: "tab_name",
            headers: new[] { "differentName" },
            content: new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("column_A", "A1")
                })
            });

        var result = firstTab.Equals(secondTab);
        
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals__TabsHaveDifferentValues__ReturnFalse()
    {
        var firstTab = new Tab(
            name: "tab_name",
            headers: new[] { "column_A" },
            content: new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("column_A", "A1")
                })
            });
        var secondTab = new Tab(
            name: "tab_name",
            headers: new[] { "column_A" },
            content: new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("column_A", "BB")
                })
            });

        var result = firstTab.Equals(secondTab);
        
        Assert.That(result, Is.False);
    }
}