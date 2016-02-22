using System;
using System.IO;
using System.Linq;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace domain.test.workflows
{
    [Trait("Category", "Workflow")]
    public class ExecuteActivitySortTest
    {
        private readonly ITestOutputHelper m_helper;

        public ExecuteActivitySortTest(ITestOutputHelper helper)
        {
            m_helper = helper;
        }

        [Fact]
        public void Sort()
        {
            var json = File.ReadAllText($@"{ConfigurationManager.Home}\..\source\unit.test\domain.test\workflows\tracker.json");
            var tracker = JsonConvert.DeserializeObject<Tracker>(json);
            Assert.NotNull(tracker);
            Assert.Equal(6, tracker.ExecutedActivityCollection.Count);
       
            m_helper.WriteLine("{0}{1}{2}", "Name".PadRight(35), "Initiated".PadRight(20), "Run".PadRight(20));
        
            foreach (var ea in tracker.ExecutedActivityCollection)
            {
                m_helper.WriteLine("{0}{1}{2}", ea.Name.PadRight(35), ea.Initiated.ToString().PadRight(20), ea.Run.ToString().PadRight(20));
            }
            var activities = tracker.ExecutedActivityCollection.ToList();
            activities.Sort(new ExecutedAcitivityComparer());

            m_helper.WriteLine("");
            m_helper.WriteLine("{0}{1}{2}", "Name".PadRight(35), "Initiated".PadRight(20), "Run".PadRight(20));

            foreach (var ea in activities)
            {
                m_helper.WriteLine("{0}{1}{2}", ea.Name.PadRight(35), ea.Initiated.ToString().PadRight(20), ea.Run.ToString().PadRight(20));
            }

        }

        [Fact]
        public void SortDate()
        {
            Assert.Equal(1, DateTime.Now.CompareTo(DateTime.Now.AddMilliseconds(-5)));
            
        }
    }
}
