using Bespoke.SphCommercialSpaces.Domain;
using NUnit.Framework;
using roslyn.scriptengine;

namespace domain.test.triggers
{
    [TestFixture]
    internal class CustomFieldValueTest
    {

        [Test]
        public void ComplaintValue()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            var cp = new Complaint { ComplaintId = 10, Note = "Whatever" };
            cp.CustomFieldValueCollection.Add(new CustomFieldValue { Name = "a", Value = "A" });
            cp.CustomFieldValueCollection.Add(new CustomFieldValue { Name = "b", Value = "B" });

            var docf = new DocumentField
            {
                Type = typeof(string),
                Path = "[a]"
            };

            var a = docf.GetValue(new RuleContext(cp));
            Assert.AreEqual("A", a);
        }    
        [Test]
        public void ComplaintValueWithLinq()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            var cp = new Complaint { ComplaintId = 10, Note = "Whatever" };
            cp.CustomFieldValueCollection.Add(new CustomFieldValue { Name = "a", Value = "A" });
            cp.CustomFieldValueCollection.Add(new CustomFieldValue { Name = "b", Value = "B" });

            var docf = new DocumentField
            {
                Type = typeof(string),
                Path = "CustomFieldValueCollection.Single(f => f.Name ==\"a\").Value"
            };

            var a = docf.GetValue(new RuleContext(cp));
            Assert.AreEqual("A", a);
        }
    }
}