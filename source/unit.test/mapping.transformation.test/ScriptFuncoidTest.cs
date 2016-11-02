using Bespoke.Sph.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mapping.transformation.test
{
    [TestClass]
    public class ScriptFuncoidTest
    {
        [TestMethod]
        public void NormalScript()
        {
            var mapping = new TransformDefinition { InputType = typeof(Address), OutputType = typeof(AddressElement) };
            var script = new ScriptFunctoid
            {
                WebId = "get-postocde",
                Name = "GetPostcode",
                OutputType = typeof(string),
                Expression = "return item.Postcode;",
                TransformDefinition = mapping
            };

            
            mapping.AddFunctoids(script);

            var statement = script.GenerateStatementCode();
            StringAssert.Contains(statement, "Func<string> GetPostcode =");
            StringAssert.Contains(statement, "() =>");

            var assignment = script.GenerateAssignmentCode();
            StringAssert.Contains(assignment, "GetPostcode()");
            
        }

        [TestMethod]
        public void AsyncScript()
        {
            var mapping = new TransformDefinition { InputType = typeof(TextBox), OutputType = typeof(ComboBox) };
            var script = new ScriptFunctoid
            {
                WebId = "get-path",
                Name = "GetPath",
                OutputType = typeof(string),
                Expression = @"
                        await Task.Delay(500);
                        return textbox.Path;",
                TransformDefinition = mapping
            };

            
            mapping.AddFunctoids(script);

            var statement = script.GenerateStatementCode();
            StringAssert.Contains(statement, "async () =>");

            var assignment = script.GenerateAssignmentCode();
            StringAssert.Contains(assignment, "await GetPath()");
            
        }


        [TestMethod]
        public void ScriptFunctoidWithArgs()
        {
            var mapping = new TransformDefinition { InputType = typeof(Bespoke.Sph.Domain.Address), OutputType = typeof(Bespoke.Sph.Domain.AddressElement) };
            var script = new ScriptFunctoid { WebId = "add", Name = "Add", OutputType = typeof(int) };
            script.ArgumentCollection.Add(new FunctoidArg { Name = "a", Type = typeof(int), Functoid = "a" });
            script.ArgumentCollection.Add(new FunctoidArg { Name = "b", Type = typeof(int), Functoid = "b" });
            script.Expression = "return a + b;";
            script.TransformDefinition = mapping;

            var a = new ConstantFunctoid { Value = 2, Type = typeof(int), WebId = "a" };
            var b = new ConstantFunctoid { Value = 5, Type = typeof(int), WebId = "b" };


            mapping.AddFunctoids(script, a, b);

            var statement = script.GenerateStatementCode();
            StringAssert.Contains(statement, "Func<int, int, int> Add =");
            StringAssert.Contains(statement, "(a,b) =>");

            var assignment = script.GenerateAssignmentCode();
            StringAssert.Contains(assignment, $"Add({a.Value}, {b.Value})");
            
        }
        [TestMethod]
        public void AsyncScriptFunctoidWithArgs()
        {
            var mapping = new TransformDefinition { InputType = typeof(Bespoke.Sph.Domain.Address), OutputType = typeof(Bespoke.Sph.Domain.AddressElement) };
            var script = new ScriptFunctoid { WebId = "add", Name = "AddAsync", OutputType = typeof(int) };
            script.ArgumentCollection.Add(new FunctoidArg { Name = "a", Type = typeof(int), Functoid = "a" });
            script.ArgumentCollection.Add(new FunctoidArg { Name = "b", Type = typeof(int), Functoid = "b" });
            script.Expression = @"
                                        await Task.Delay(500);
                                        return a + b;";
            script.TransformDefinition = mapping;

            var a = new ConstantFunctoid { Value = 2, Type = typeof(int), WebId = "a" };
            var b = new ConstantFunctoid { Value = 5, Type = typeof(int), WebId = "b" };


            mapping.AddFunctoids(script, a, b);

            var statement = script.GenerateStatementCode();
            StringAssert.Contains(statement, "(a,b) =>");

            var assignment = script.GenerateAssignmentCode();
            StringAssert.Contains(assignment, $"await AddAsync({a.Value}, {b.Value})");


        }
    }
}