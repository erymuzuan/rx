using System;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using Bespoke.SphCommercialSpaces.Domain;
using InvokeSolutions.Docx;
using InvokeSolutions.Docx.Data;
using System.Linq;

namespace Bespoke.Sph.WordGenerator
{
    public class WordGenerator : IDocumentGenerator
    {
        public string StartingParameterIdentifierToken { get; set; }
        public string EndingParameterIdentifierToken { get; set; }
        public char AccessorOperator { get; set; }
        public string DefaultNamespace { get; set; }
        //Bespoke.SphCommercialSpaces.Domain


        public WordGenerator()
        {
            AccessorOperator = '_';
            EndingParameterIdentifierToken = "%";
            StartingParameterIdentifierToken = "%";
        }

        public void GenerateWithObject(string output, params DomainObject[] data)
        {
            var dataSource = new DocumentDataSource
            {
                StartingToken = StartingParameterIdentifierToken,
                EndingToken = EndingParameterIdentifierToken
            };

            foreach (var o in data)
            {
                dataSource[o.GetType().Name] = DataItem.ConvertFrom(o);
            }

            // standard items
            dataSource["DateTime_Now"] = DateTime.Now;
            dataSource["DateTime_Today"] = DateTime.Today;
            dataSource["UserName"] = Thread.CurrentPrincipal.Identity.Name;
            dataSource["Current_Month"] = DateTime.Today.Month;
            dataSource["Current_Year"] = DateTime.Today.Year;


            DocumentRenderer.ProcessDocument(output, dataSource);
        }

        public void Generate(string output, params DomainObject[] data)
        {
            var dataSource = new DocumentDataSource
                                 {
                                     StartingToken = StartingParameterIdentifierToken,
                                     EndingToken = EndingParameterIdentifierToken
                                 };

            foreach (var o in data)
            {
                ProcessTemplate2(dataSource, o, o, string.Empty);
            }

            // add standards items
            dataSource["DateTime_Now"] = DateTime.Now;
            dataSource["DateTime_Today"] = DateTime.Today;
            dataSource["UserName"] = Thread.CurrentPrincipal.Identity.Name;
            dataSource["Current_Month"] = DateTime.Today.Month;
            dataSource["Current_Year"] = DateTime.Today.Year;

            DocumentRenderer.ProcessDocument(output, dataSource);
        }



        public void ProcessTemplate2(DocumentDataSource dataSource, DomainObject data, DomainObject element, string prepend)
        {
            if (null != data){
            string name = data.GetType().Name;
            var elementType = element.GetType();

            if (elementType.Name.EndsWith("Collection"))
            {
                // ProcessCollection(dataSource, element);
            }

            var properties = elementType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField |BindingFlags.GetField)
                                          .Where(p => p.GetIndexParameters().Length == 0)
                                          .ToList(); // indexer
            foreach (var a in properties)
            {
                if (!a.CanRead) continue;
                //if (a.PropertyType.Namespace == "Bespoke.BphEtemujanji.Domain") continue;

                var placeHolder = prepend == string.Empty ? string.Format("{0}{1}{4}{2}{3}", StartingParameterIdentifierToken, name, a.Name, EndingParameterIdentifierToken, AccessorOperator)
                    : string.Format("{0}{1}{5}{2}{5}{3}{4}", StartingParameterIdentifierToken, name, prepend, a.Name, EndingParameterIdentifierToken, AccessorOperator);
                var value = a.GetValue(element, null).ToEmptyString();
                if (!string.IsNullOrEmpty(value))
                {
                    dataSource[placeHolder] = value;
                }

            }


            foreach (var e in properties)
            {
                if (e.PropertyType.Namespace != this.DefaultNamespace) continue;


                var child = e.GetValue(element, null) as DomainObject;
                if (null == child) continue;
                //  other complex element
                ProcessTemplate2(dataSource, data, child,
                    string.IsNullOrEmpty(prepend) ? e.Name :
                    prepend + AccessorOperator + e.Name);
            }
            }
        }




        public void ProcessTemplate(DocumentDataSource dataSource, DomainObject data, XElement element, string prepend)
        {

            string name = data.GetType().Name;

            if (element.Name.LocalName.EndsWith("Collection"))
            {
                ProcessCollection(dataSource, element);
            }

            foreach (var a in element.Attributes())
            {
                if (a.IsNamespaceDeclaration) continue;

                var placeHolder = prepend == string.Empty ? string.Format("{0}{1}{4}{2}{3}", StartingParameterIdentifierToken, name, a.Name.LocalName, EndingParameterIdentifierToken, AccessorOperator)
                    : string.Format("{0}{1}{5}{2}{5}{3}{4}", StartingParameterIdentifierToken, name, prepend, a.Name.LocalName, EndingParameterIdentifierToken, AccessorOperator);
                var value = a.Value.GetFormattedValue();
                if (!string.IsNullOrEmpty(value))
                {
                    dataSource[placeHolder] = value;
                }

            }


            foreach (var e in element.Elements())
            {
                // !e.HasElements is simple element and it could be element with just attributes
                if (!e.HasElements)
                {
                    var placeHolder = prepend == string.Empty
                                          ? string.Format("{0}{1}{2}{3}{4}", StartingParameterIdentifierToken, name,
                                                          AccessorOperator, e.Name.LocalName,
                                                          EndingParameterIdentifierToken)
                                          : string.Format("{0}{1}{2}{3}{4}{5}{6}", StartingParameterIdentifierToken,
                                                          name, AccessorOperator, prepend, AccessorOperator,
                                                          e.Name.LocalName, EndingParameterIdentifierToken);
                    var value = e.Value.GetFormattedValue();
                    if (!string.IsNullOrEmpty(value))
                    {
                        Console.WriteLine("{0} : {1}", placeHolder, value);
                        dataSource[placeHolder] = value;
                    }
                    if (!e.HasAttributes)
                        continue;
                }

                //  other complex element
                ProcessTemplate(dataSource, data, e,
                    string.IsNullOrEmpty(prepend) ? e.Name.LocalName :
                    prepend + AccessorOperator + e.Name.LocalName);
            }
        }



        private  void ProcessCollection(DocumentDataSource source, XElement element)
        {
            var property = element.Name.LocalName;

            var csharpTypeNames = new[] { "string", "int", "double", "decimal", "float" };
            var list = from e in element.Elements()
                       let dataType = this.DefaultNamespace + "." + e.Name.LocalName
                       where !csharpTypeNames.Contains(e.Name.LocalName)
                       select e.DeserializeFromXml(dataType);

            source[property] = DataItem.ConvertFrom(list.ToArray());
        }
    }


}
