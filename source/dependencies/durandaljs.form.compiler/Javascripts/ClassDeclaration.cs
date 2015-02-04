using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs.Javascripts
{
    internal class ClassDeclaration
    {

        public void AddReturn(string identifier, string body = null)
        {
            var statment = string.IsNullOrWhiteSpace(body) ? identifier : body;
            if (this.ReturnStatement.ValueCollection.ContainsKey(identifier))
            {
                Console.WriteLine("The return statement already contains [{0}] : {1}", identifier, statment);
                return;
            }
            this.ReturnStatement.ValueCollection.Add(identifier, statment);
        }
        public void AddDependency(string location, string identifier)
        {
            this.DependeciesCollection.Add(location, identifier);
        }
        public FieldDeclaration AddField(string identifier, string initializer = null)
        {
            var field = new FieldDeclaration
            {
                Name = identifier,
                Intializer = string.IsNullOrWhiteSpace(initializer) ? "ko.observable()" : initializer
            };
            this.FieldCollection.Add(field);
            return field;
        }

        public FunctionDeclaration AddFunction(string name, string body, params string[] arguments)
        {
            var function = new FunctionDeclaration()
            {
                Name = name,
                Body = body
            };
            function.ArgumentCollection.AddRange(arguments);
            this.FunctionCollection.Add(function);
            return function;
        }
        public FunctionDeclaration AddFunction(FunctionDeclaration function)
        {
            this.FunctionCollection.Add(function);
            return function;
        }

        public string Name { get; set; }
        public string FileName { get; set; }
        private readonly Dictionary<string, string> m_dependeciesCollection = new Dictionary<string, string>();
        private readonly ObjectCollection<FieldDeclaration> m_fieldCollection = new ObjectCollection<FieldDeclaration>();
        private readonly ObjectCollection<FunctionDeclaration> m_functionCollection = new ObjectCollection<FunctionDeclaration>();
        public ReturnStatement ReturnStatement { get; set; }

        public ClassDeclaration()
        {
            this.ReturnStatement = new ReturnStatement();
        }

        public ObjectCollection<FunctionDeclaration> FunctionCollection
        {
            get { return m_functionCollection; }
        }
        public ObjectCollection<FieldDeclaration> FieldCollection
        {
            get { return m_fieldCollection; }
        }
        public Dictionary<string, string> DependeciesCollection
        {
            get { return m_dependeciesCollection; }
        }

        public override string ToString()
        {
            var code = new StringBuilder();
            var keys = this.DependeciesCollection.Keys.Select(x => string.Format("\"{0}\"", x));
            var values = this.DependeciesCollection.Values.Select(x => string.Format("{0}", x));

            code.AppendLinf("define([{0}],", string.Join(", ", keys));
            code.AppendLinf("   function({0}){{", string.Join(", ", values));

            if (this.FieldCollection.Any())
            {
                code.AppendLine();
                code.Append("   var ");
                code.Append(string.Join(",\r\n", this.FieldCollection.Select(x => x.ToString())).Trim() + ";");
                code.AppendLine();
                code.AppendLine();

            }

            if (this.FunctionCollection.Any())
            {
                code.AppendLine();
                code.Append("var ");
                code.Append(string.Join(",\r\n", this.FunctionCollection.Select(x => x.ToString())).Trim() + ";");
                code.AppendLine();
                code.AppendLine();

            }
            code.AppendLine(this.ReturnStatement.ToString());

            code.AppendLine("}");
            code.AppendLine(");");

            return code.ToString();
        }
    }
}