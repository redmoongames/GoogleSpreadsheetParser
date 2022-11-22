// using GoogleSpreadsheetRepository;
// using Moq;
//
// namespace GooglespreadsheetTests;
//
// public class SpreadsheetRepositoryTests
// {
//     private Mock<IApiConnector> _apiConnector;
//     
//     [SetUp]
//     public void Setup()
//     {
//         _apiConnector = new Mock<IApiConnector>();
//     }
//
//     [Test]
//     public void UpdateTab__WhenCalled_ReturnCorrectTabs()
//     {
//         _apiConnector
//             .Setup(con => con.GetTabsNames())
//             .Returns(() => new []{"tab_name"});
//         _apiConnector
//             .Setup(con => con.LoadRowData(It.IsAny<string>()))
//             .Returns(() => new List<IList<object>>
//             {
//                 new List<object>
//                 {
//                     "column_A",
//                     "column_B"
//                 },
//                 new List<object>
//                 {
//                     "A1",
//                     "B1"
//                 },
//                 new List<object>
//                 {
//                     "A2",
//                     "B2"
//                 }
//             });
//         var repository = new ObjectLoader(_apiConnector.Object);
//         var result = repository.LoadAllTabs();
//         var target = new Tab[]
//         {
//             new Tab(
//                 name: "tab_name",
//                 headers: new[] { "column_A", "column_B" },
//                 content: new List<Dictionary<string, string>>
//                 {
//                     new Dictionary<string, string>(new List<KeyValuePair<string, string>>
//                     {
//                         new KeyValuePair<string, string>("column_A", "A1"),
//                         new KeyValuePair<string, string>("column_B", "B1")
//                     }),
//                     new Dictionary<string, string>(new List<KeyValuePair<string, string>>
//                     {
//                         new KeyValuePair<string, string>("column_A", "A2"),
//                         new KeyValuePair<string, string>("column_B", "B2")
//                     })
//                 })
//         };
//
//         var resultBool = result.SequenceEqual(target);
//         
//         Assert.That(resultBool, Is.True);
//     }
//
//     [Test]
//     public void UpdateTab__WithTrashValues_ReturnCorrectTabs()
//     {
//         _apiConnector
//             .Setup(con => con.GetTabsNames())
//             .Returns(() => new []{"tab_name"});
//         _apiConnector
//             .Setup(con => con.LoadRowData(It.IsAny<string>()))
//             .Returns(() => new List<IList<object>>
//             {
//                 new List<object>
//                 {
//                     "column_A",
//                     "column_B"
//                 },
//                 new List<object>
//                 {
//                     "A1",
//                     "B1",
//                     "TRASH VALUE",
//                     "TRASH VALUE",
//                 },
//                 new List<object>
//                 {
//                     "A2",
//                     "B2",
//                     "",
//                     "",
//                     "",
//                     "",
//                     "TRASH VALUE",
//                 }
//             });
//         var repository = new ObjectLoader(_apiConnector.Object);
//         var result = repository.LoadAllTabs();
//         var target = new Tab[]
//         {
//             new Tab(
//                 name: "tab_name",
//                 headers: new[] { "column_A", "column_B" },
//                 content: new List<Dictionary<string, string>>
//                 {
//                     new Dictionary<string, string>(new List<KeyValuePair<string, string>>
//                     {
//                         new KeyValuePair<string, string>("column_A", "A1"),
//                         new KeyValuePair<string, string>("column_B", "B1")
//                     }),
//                     new Dictionary<string, string>(new List<KeyValuePair<string, string>>
//                     {
//                         new KeyValuePair<string, string>("column_A", "A2"),
//                         new KeyValuePair<string, string>("column_B", "B2")
//                     })
//                 })
//         };
//
//         var resultBool = result.SequenceEqual(target);
//         
//         Assert.That(resultBool, Is.True);
//     }
//
//     [Test]
//     public void UpdateTab__WithEmptyCells_ReturnCorrectTabs()
//     {
//         _apiConnector
//             .Setup(con => con.GetTabsNames())
//             .Returns(() => new []{"tab_name"});
//         _apiConnector
//             .Setup(con => con.LoadRowData(It.IsAny<string>()))
//             .Returns(() => new List<IList<object>>
//             {
//                 new List<object>
//                 {
//                     "column_A",
//                     "column_B",
//                     "column_with_empty_values"
//                 },
//                 new List<object>
//                 {
//                     "A1",
//                     "B1"
//                 },
//                 new List<object>
//                 {
//                     "A2",
//                     "B2"
//                 }
//             });
//         var repository = new ObjectLoader(_apiConnector.Object);
//         var result = repository.LoadAllTabs();
//         var target = new Tab[]
//         {
//             new Tab(
//                 name: "tab_name",
//                 headers: new[] { "column_A", "column_B", "column_with_empty_values" },
//                 content: new List<Dictionary<string, string>>
//                 {
//                     new Dictionary<string, string>(new List<KeyValuePair<string, string>>
//                     {
//                         new KeyValuePair<string, string>("column_A", "A1"),
//                         new KeyValuePair<string, string>("column_B", "B1"),
//                         new KeyValuePair<string, string>("column_with_empty_values", "")
//                     }),
//                     new Dictionary<string, string>(new List<KeyValuePair<string, string>>
//                     {
//                         new KeyValuePair<string, string>("column_A", "A2"),
//                         new KeyValuePair<string, string>("column_B", "B2"),
//                         new KeyValuePair<string, string>("column_with_empty_values", "")
//                     })
//                 })
//         };
//
//         var resultBool = result.SequenceEqual(target);
//         
//         Assert.That(resultBool, Is.True);
//     }
// }